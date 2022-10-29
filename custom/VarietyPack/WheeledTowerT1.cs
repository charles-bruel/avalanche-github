using PrecompiledExtensions;
using UnityEngine;
using System.Collections.Generic;

namespace VarietyPack
{
    public class WheeledTowerT1 : LiftTowerCablePathFetcher
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //Right Wheel Generator Transform
        private const int RWGT = 0;
        //Left Wheel Generator Transform
        private const int LWGT = 1;
        //Right Wheel Mesh Generator Transform
        private const int RWMGT = 2;
        //Left Wheel Mesh Generator Transform
        private const int LWMGT = 3;
        //Wheel Prefab
        private const int WP = 4;
        //Base Pos
        private const int BRP = 5;
        //Catwalk Prefab
        private const int CWP = 6;
        //INT PARAM LAYOUT
        //Wheel Count
        private const int WC = 0;
        //FLOAT PARAM LAYOUR
        //Wheel Spacing
        private const int WS = 0;
        //Mesh Thickness
        private const int MT = 1;
        //Under Cable Offset Value
        private const int UCOV = 2;

        public override void OnParameterUpdate(Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            Initialize();

            RightWheelGenerator.lowerTarget = prevTower;
            RightWheelGenerator.upperTarget = nextTower;
            RightWheelGenerator.basePos = LoadedData[BRP].transform;

            LeftWheelGenerator.lowerTarget = prevTower;
            LeftWheelGenerator.upperTarget = nextTower;
            LeftWheelGenerator.basePos = LoadedData[BRP].transform;

            RightWheelGenerator.UpdatePositions();
            LeftWheelGenerator.UpdatePositions();

            RightWheelMeshGenerator.GenerateMesh();
            LeftWheelMeshGenerator.GenerateMesh();
        }

        public override List<Transform> GetCablePath(Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            WheelScript[] wheels = (right ? RightWheelGenerator : LeftWheelGenerator).GetAllWheels();
            List<Transform> toReturn = new List<Transform>(wheels.Length);
            for (int i = 0; i < wheels.Length; i++)
            {
                toReturn.Add(wheels[i].cableAttachPoint);
            }
            return toReturn;
        }

        private void Initialize()
        {
            if (HasInitialized) return;

            RightWheelGenerator = LoadedData[RWGT].AddComponent<WheelGeneratorTower>();
            LeftWheelGenerator = LoadedData[LWGT].AddComponent<WheelGeneratorTower>();
            RightWheelMeshGenerator = LoadedData[RWMGT].AddComponent<WheelMeshGenerator>();
            LeftWheelMeshGenerator = LoadedData[LWMGT].AddComponent<WheelMeshGenerator>();

            RightWheelGenerator.generator = RightWheelMeshGenerator;
            RightWheelMeshGenerator.wheelGenerator = RightWheelGenerator;
            LeftWheelGenerator.generator = LeftWheelMeshGenerator;
            LeftWheelMeshGenerator.wheelGenerator = LeftWheelGenerator;

            RightWheelMeshGenerator.thickness = FloatParameters[MT];
            LeftWheelMeshGenerator.thickness = FloatParameters[MT];

            RightWheelGenerator.UnderCableOffsetValue = FloatParameters[UCOV];
            LeftWheelGenerator.UnderCableOffsetValue = FloatParameters[UCOV];

            GameObject wheelPrefab = LoadedData[WP];

            //Dirty harcoded hack
            WheelScript wheelScript = wheelPrefab.AddComponent<WheelScript>();

            wheelScript.cableAttachPoint = wheelPrefab.transform.GetChild(0).GetChild(0);
            //Does performance matter this much? Probably not. Is this comment nessecary? Probably not but because dnSpy doesn't let you do any comments AT ALL, I take my opprotunity.
            Transform child2 = wheelPrefab.transform.GetChild(0).GetChild(2);
            int child2childCount = child2.childCount;
            wheelScript.meshPoints = new Transform[child2.childCount];
            for (int i = 0; i < child2childCount; i++)
            {
                wheelScript.meshPoints[i] = child2.GetChild(i);
            }

            RightWheelGenerator.wheelPrefab = wheelPrefab;
            LeftWheelGenerator.wheelPrefab = wheelPrefab;

            RightWheelGenerator.catwalkPrefab = LoadedData[CWP];
            LeftWheelGenerator.catwalkPrefab = LoadedData[CWP];

            RightWheelGenerator.lowerWheelCount = IntParameters[WC] - 1;
            RightWheelGenerator.upperWheelCount = IntParameters[WC];
            LeftWheelGenerator.lowerWheelCount = IntParameters[WC] - 1;
            LeftWheelGenerator.upperWheelCount = IntParameters[WC];

            RightWheelGenerator.wheelSpacing = FloatParameters[WS];
            LeftWheelGenerator.wheelSpacing = FloatParameters[WS];

            HasInitialized = true;
        }

        private bool HasInitialized = false;

        private WheelGeneratorTower RightWheelGenerator;
        private WheelGeneratorTower LeftWheelGenerator;
        private WheelMeshGenerator RightWheelMeshGenerator;
        private WheelMeshGenerator LeftWheelMeshGenerator;
    }
}
