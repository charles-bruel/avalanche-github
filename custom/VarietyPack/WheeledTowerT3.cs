using PrecompiledExtensions;
using UnityEngine;
using System.Collections.Generic;

namespace VarietyPack
{
    public class WheeledTowerT3 : LiftTowerCablePathFetcher
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
        //Right Wheel Attachment Transform
        private const int RWAT = 6;
        //Left Wheel Attachment Transform
        private const int LWAT = 7;
        //Right Wheel Attachment Base Transform
        private const int RWABT = 8;
        //Left Wheel Attachment Base Transform
        private const int LWABT = 9;
        //Catwalk Prefab
        private const int CWP = 10;
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

            WheelAttachmentUpdate();
        }

        public override List<Transform> GetCablePath(Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            WheelScript[] wheels = (right ? RightWheelGenerator : LeftWheelGenerator).GetAllWheels();
            List<Transform> toReturn = new List<Transform>(wheels.Length);
            for(int i = 0; i < wheels.Length;i++)
            {
                toReturn.Add(wheels[i].cableAttachPoint);
            }
            return toReturn;
        }

        private void WheelAttachmentUpdate()
        {
            WheelScript[] rightWheels = RightWheelGenerator.GetAllWheels();
            int offset = IntParameters[WC];
            offset = offset < 3 ? offset : 3;
            Vector3 rightWheelA = rightWheels[rightWheels.Length / 2 - offset].transform.position;
            Vector3 rightWheelB = rightWheels[rightWheels.Length / 2 + offset].transform.position;

            float RdeltaX = rightWheelA.x - rightWheelB.x;
            float RdeltaY = rightWheelA.y - rightWheelB.y;
            float RdeltaZ = rightWheelA.z - rightWheelB.z;
            float RdeltaH = Mathf.Sqrt(RdeltaX * RdeltaX + RdeltaZ * RdeltaZ);

            LoadedData[RWAT].transform.localEulerAngles = new Vector3(-90 - Mathf.Atan2(RdeltaY, RdeltaH) * Mathf.Rad2Deg, 0, 0);

            WheelScript[] leftWheels = LeftWheelGenerator.GetAllWheels();
            Vector3 leftWheelA = leftWheels[leftWheels.Length / 2 - 3].transform.position;
            Vector3 leftWheelB = leftWheels[leftWheels.Length / 2 + 3].transform.position;

            float LdeltaX = leftWheelA.x - leftWheelB.x;
            float LdeltaY = leftWheelA.y - leftWheelB.y;
            float LdeltaZ = leftWheelA.z - leftWheelB.z;
            float LdeltaH = Mathf.Sqrt(LdeltaX * LdeltaX + LdeltaZ * LdeltaZ);

            LoadedData[LWAT].transform.localEulerAngles = new Vector3(-90 - Mathf.Atan2(LdeltaY, LdeltaH) * Mathf.Rad2Deg, 0, 0);

            Vector3 rightDelta = LoadedData[RWABT].transform.position - rightWheelA;
            RightWheelGenerator.transform.position += rightDelta;

            Vector3 leftDelta = LoadedData[LWABT].transform.position - leftWheelA;
            LeftWheelGenerator.transform.position += leftDelta;
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
            for(int i = 0;i < child2childCount;i++)
            {
                wheelScript.meshPoints[i] = child2.GetChild(i);
            }

            RightWheelGenerator.wheelPrefab = wheelPrefab;
            LeftWheelGenerator.wheelPrefab = wheelPrefab;

            RightWheelGenerator.catwalkPrefab = LoadedData[CWP];
            LeftWheelGenerator.catwalkPrefab = LoadedData[CWP];

            RightWheelGenerator.lowerWheelCount = IntParameters[WC];
            RightWheelGenerator.upperWheelCount = IntParameters[WC];
            LeftWheelGenerator.lowerWheelCount = IntParameters[WC];
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
