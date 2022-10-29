using PrecompiledExtensions;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VarietyPack
{
    public class StationT1 : LiftStationCablePathFetcher
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
        //Wheel Count
        private const int WC = 1;
        //FLOAT PARAM LAYOUR
        //Wheel Spacing
        private const int WS = 0;
        //Mesh Thickness
        private const int MT = 1;
        //Wheel radius
        private const int WR = 2;
        //Offset
        private const int O = 3;
        //Tower Prefab Chord Length
        private const int CL = 4;

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

            if (tower == null)
            {
                tower = Instantiate(LoadedData[TP], LoadedData[TPT].transform);
            }

            int middleIndex = (IntParameters[WC] + 2) / 2 - 1;

            //Transform LeftAttachment = tower.transform.GetChild(0).GetChild(1).GetChild(2);
            //Transform RightAttachment = tower.transform.GetChild(0).GetChild(1).GetChild(3);

            //float value = -90 - LeftWheelGenerator.anglePerWheel * middleIndex * (RightWheelGenerator.above ? -1 : 1);
            //LeftAttachment.localEulerAngles = new Vector3(value, 0, 0);
            //RightAttachment.localEulerAngles = new Vector3(value, 0, 0);

            //float xOffset = -Mathf.Cos(Mathf.Deg2Rad * value);
            //float yOffset = -Mathf.Sin(Mathf.Deg2Rad * value);
            //float length = GetNormalOffset(RightWheelGenerator.radius, FloatParameters[CL]);
            //xOffset *= length;
            //yOffset *= length;

            //float theta = LeftAttachment.eulerAngles.y * Mathf.Deg2Rad;

            //Vector3 fullOffset = new Vector3(xOffset * Mathf.Sin(theta), yOffset, xOffset * Mathf.Cos(theta));

            Vector3 temp = rightWheels[middleIndex].transform.position;
            temp += transform.forward * FloatParameters[O] * (IntParameters[BV] != 0 ? 1 : -1);
            //temp += fullOffset;
            tower.transform.position = temp;
        }

        private void Initialize()
        {
            if (HasInitialized)
            {
                return;
            }

            RightWheelGenerator = LoadedData[RWGT].AddComponent<WheelGeneratorStationT2>();
            LeftWheelGenerator = LoadedData[LWGT].AddComponent<WheelGeneratorStationT2>();
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

            RightWheelGenerator.totalWheelCount = IntParameters[WC];
            LeftWheelGenerator.totalWheelCount = IntParameters[WC];

            RightWheelGenerator.wheelRadius = FloatParameters[WR];
            LeftWheelGenerator.wheelRadius = FloatParameters[WR];

            HasInitialized = true;
        }

        public float GetNormalOffset(float r, float chordLength)
        {
            float y = chordLength;
            float a = Mathf.Sqrt(r * r - (y / 2) * (y / 2));
            float x = r - a;
            return x;//Weird variable names are from whiteboard math
        }

        private bool HasInitialized = false;

        private WheelGeneratorStationT2 RightWheelGenerator;
        private WheelGeneratorStationT2 LeftWheelGenerator;
        private WheelMeshGenerator RightWheelMeshGenerator;
        private WheelMeshGenerator LeftWheelMeshGenerator;

        private GameObject tower;
    }
}
