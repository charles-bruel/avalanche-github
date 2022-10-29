using PrecompiledExtensions;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VarietyPack
{
    public class StationT3 : LiftStationCablePathFetcher
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //Right Wheel Generator Transform
        private const int RWGT = 0;
        //Right Wheel Mesh Generator Transform
        private const int RWMGT = 1;
        //Left Wheel Generator Transform
        private const int LWGT = 2;
        //Left Wheel Mesh Generator Transform
        private const int LWMGT = 3;
        //Wheel Prefab
        private const int WP = 4;
        //Tower Prefab
        private const int TP = 5;
        //Tower Parent Transform
        private const int TPT = 6;
        //Catwalk Prefab
        private const int CP = 7;
        //INT PARAM LAYOUT
        //Bottom Value
        private const int BV = 0;
        //Wheels Per Tower
        private const int WPT = 1;
        //FLOAT PARAM LAYOUR
        //Wheel Spacing
        private const int WS = 0;
        //Angle Per Wheel
        private const int APW = 1;
        //Mesh Thickness
        private const int MT = 2;
        //Wheel radius
        private const int WR = 3;
        //Offset
        private const int O = 4;

        public override void OnParameterUpdate(Transform otherTower, Transform stationPos)
        {
            Initialize();

            RightWheelGenerator.target = otherTower;
            RightWheelGenerator.basePos = stationPos;
            LeftWheelGenerator.target = otherTower;
            LeftWheelGenerator.basePos = stationPos;

            RightWheelGenerator.UpdatePositions();
            LeftWheelGenerator.UpdatePositions();

            RightWheelMeshGenerator.GenerateMesh();
            LeftWheelMeshGenerator.GenerateMesh();

            UpdateTowers();
        }

        public override List<Transform> GetCablePath(Transform otherTower, Transform stationPos, Transform relevantCablePoint, bool right)
        {
            WheelScript[] wheels = (right ? RightWheelGenerator : LeftWheelGenerator).GetAllWheels();
            List<Transform> wheelPoints = new List<Transform>(wheels.Length);
            for (int i = 0; i < wheels.Length; i++)
            {
                wheelPoints.Add(wheels[i].cableAttachPoint);
            }
            List<Transform> stationPoints = base.GetCablePath(otherTower, stationPos, relevantCablePoint, right);
            if (IntParameters[BV] == 0)
            {
                wheelPoints.AddRange(stationPoints);
                return wheelPoints;
            } 
            else
            {
                wheelPoints.Reverse();
                stationPoints.AddRange(wheelPoints);
                return stationPoints;
            }
        }

        private void UpdateTowers()
        {
            WheelScript[] rightWheels = RightWheelGenerator.GetAllWheels();
            List<int> toPlace = GeneratePlacementIds(rightWheels);

            GameObject[] newTowers = new GameObject[toPlace.Count];

            if (toPlace.Count < towers.Length)
            {
                for (int i = towers.Length - 1; i >= toPlace.Count; i--)
                {
                    DestroyImmediate(towers[i]);
                }
                for (int i = 0; i < toPlace.Count; i++)
                {
                    newTowers[i] = towers[i];
                }
                towers = newTowers;
            }
            else if (toPlace.Count > towers.Length)
            {
                for (int i = 0; i < towers.Length; i++)
                {
                    newTowers[i] = towers[i];
                }
                for (int i = towers.Length; i < toPlace.Count; i++)
                {
                    newTowers[i] = Instantiate(LoadedData[TP], LoadedData[TPT].transform);
                }
                towers = newTowers;
            }

            for(int i = 0;i < toPlace.Count;i++)
            {
                Vector3 temp = rightWheels[toPlace[i]].transform.position;
                temp += transform.forward * FloatParameters[O] * (IntParameters[BV] != 0 ? 1 : -1);
                towers[i].transform.position = temp;
                float mulFactor = (RightWheelGenerator.bottom ^ RightWheelGenerator.above) ? -1 : 1;
                towers[i].transform.localRotation = Quaternion.Euler(0, 0, mulFactor * rightWheels[toPlace[i]].transform.localEulerAngles.x);
                towers[i].transform.localScale = new Vector3(RightWheelGenerator.above ? 1 : -1, 1, 1);
            }
        }

        private List<int> GeneratePlacementIds(WheelScript[] wheels)
        {
            int numberOfTowers = (int) Mathf.Max(Mathf.Ceil((float) wheels.Length / IntParameters[WPT]), 1);//Min of one tower
            float wheelsPerHalfSegment = (float)wheels.Length / (numberOfTowers * 2);
            List<int> toReturn = new List<int>(numberOfTowers);
            for(int i = 0;i < numberOfTowers;i ++)
            {
                float approxValue = ((i * 2) + 1) * wheelsPerHalfSegment;//Evenly space them
                int exactValue = Mathf.RoundToInt(approxValue);
                toReturn.Add(exactValue);
            }
            return toReturn;
        }

        private void Initialize()
        {
            if (HasInitialized)
            {
                return;
            }

            RightWheelGenerator = LoadedData[RWGT].AddComponent<WheelGeneratorStationT3>();
            LeftWheelGenerator = LoadedData[LWGT].AddComponent<WheelGeneratorStationT3>();
            RightWheelMeshGenerator = LoadedData[RWMGT].AddComponent<WheelMeshGenerator>();
            LeftWheelMeshGenerator = LoadedData[LWMGT].AddComponent<WheelMeshGenerator>();

            RightWheelGenerator.bottom = IntParameters[BV] != 0;
            LeftWheelGenerator.bottom = IntParameters[BV] != 0;

            RightWheelGenerator.catwalkPrefab = LoadedData[CP];
            LeftWheelGenerator.catwalkPrefab = LoadedData[CP];

            RightWheelMeshGenerator.wheelGenerator = RightWheelGenerator;
            LeftWheelMeshGenerator.wheelGenerator = LeftWheelGenerator;

            RightWheelMeshGenerator.thickness = FloatParameters[MT];
            LeftWheelMeshGenerator.thickness = FloatParameters[MT];

            GameObject wheelPrefab = LoadedData[WP];

            //Dirty harcoded hack
            WheelScript wheelScript = wheelPrefab.AddComponent<WheelScript>();

            wheelScript.cableAttachPoint = wheelPrefab.transform.GetChild(0).GetChild(0);

            //Does performance matter this much? Probably not. Is this comment nessecary? Probably not but because dnSpy doesn't let you do any comments AT ALL, I take my opprotunity.
            Transform child2 = wheelPrefab.transform.GetChild(0).GetChild(2);

            int child2childCount = child2.childCount;

            wheelScript.meshPoints = new Transform[child2childCount];

            for (int i = 0; i < child2childCount; i++)
            {
                wheelScript.meshPoints[i] = child2.GetChild(i);
            }

            RightWheelGenerator.wheelPrefab = wheelPrefab;
            LeftWheelGenerator.wheelPrefab = wheelPrefab;

            RightWheelGenerator.wheelSpacing = FloatParameters[WS];
            LeftWheelGenerator.wheelSpacing = FloatParameters[WS];

            RightWheelGenerator.anglePerWheel = FloatParameters[APW];
            LeftWheelGenerator.anglePerWheel = FloatParameters[APW];

            RightWheelGenerator.wheelRadius = FloatParameters[WR];
            LeftWheelGenerator.wheelRadius = FloatParameters[WR];

            HasInitialized = true;
        }

        private bool HasInitialized = false;

        private WheelGeneratorStationT3 RightWheelGenerator;
        private WheelGeneratorStationT3 LeftWheelGenerator;
        private WheelMeshGenerator RightWheelMeshGenerator;
        private WheelMeshGenerator LeftWheelMeshGenerator;

        private GameObject[] towers = new GameObject[0];
    }
}
