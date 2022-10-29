using System;
using PrecompiledExtensions;
using UnityEngine;
using System.Collections.Generic;

namespace VarietyPack
{
    public class TurnT3 : LiftTurnCablePathFetcher
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //Right Wheel Parent
        private const int RWP = 0;
        //Left Wheel Parent
        private const int LWP = 1;
        //Right Wheel Mesh Generator Transform
        private const int RWMGT = 2;
        //Left Wheel Mesh Generator Transform
        private const int LWMGT = 3;
        //Catwalk Prefab
        private const int CWP = 4;
        //Wheel Prefab
        private const int WP = 5;
        //INT PARAM LAYOUT
        //FLOAT PARAM LAYOUT
        //Tower Width
        private const int TW = 0;
        //Wheels per degrees
        private const int WPD = 1;
        //Wheel Spacing
        private const int WS = 2;
        //Mesh Thickness
        private const int MT = 3;
        //Wheels per segment
        private const int WPS = 4;

        public override void OnParameterUpdate(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            angle1 *= -1;
            angle2 *= -1;

            PlaceWheels(angle1, angle2, prevTower, nextTower, currentTowerPos);
        }

        private void PlaceWheels(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            float radius = GetRadiusC(nextTower, prevTower);
            float length = GetLengthC(angle1, angle2, nextTower, prevTower, currentTowerPos);
            float theta = Mathf.PI - AngleDiff(angle1, angle2);

            //Get starting pos
            float os = angle1 + Mathf.PI;
            os %= (Mathf.PI * 2);
            Vector2 temp1 = new Vector2(Mathf.Cos(os), Mathf.Sin(os));
            Vector2 startPos = temp1 * length;

            //Get curve direction
            //-1 means curve to the right, 1 means curve to the left
            //right curve is if angle2 < angle1 (in annoying angle world) i.e. 45 degrees going to 20 degrees
            int curveDir;
            if (angle2 < angle1)
            {
                if (angle1 - angle2 < Mathf.PI)
                {
                    //simple case
                    curveDir = 1;
                }
                else
                {
                    curveDir = -1;
                }
            }
            else
            {
                if (angle2 - angle1 < Mathf.PI)
                {
                    //simple case
                    curveDir = -1;
                }
                else
                {
                    curveDir = 1;
                }
            }
            //Determine pivot point
            float sp = os + Mathf.PI * 0.5f * curveDir;
            sp %= (Mathf.PI * 2);
            Vector2 temp2 = new Vector2(Mathf.Cos(sp), Mathf.Sin(sp));
            Vector2 pivotPos = startPos + (temp2 * radius);

            //Determine left and right radii
            float rightRadius = radius - FloatParameters[TW] * curveDir;
            float leftRadius = radius + FloatParameters[TW] * curveDir;

            //Actually generate
            float degreesNeeded = (Mathf.PI - theta) * Mathf.Rad2Deg;
            float wheelsNeeded = degreesNeeded * FloatParameters[WPD];
            int wheelsNeededRight = Mathf.CeilToInt(rightRadius * wheelsNeeded / radius);
            int wheelsNeededLeft = Mathf.CeilToInt(leftRadius * wheelsNeeded / radius);

            float initialAngle = sp + Mathf.PI;
            initialAngle %= (Mathf.PI * 2);
            float angleIncreasePerWheel = (1 / FloatParameters[WPD]) * Mathf.Deg2Rad;
            float angleIncreasePerWheelRight = radius * angleIncreasePerWheel / rightRadius;
            float angleIncreasePerWheelLeft = radius * angleIncreasePerWheel / leftRadius;

            int rightBump = 1 - ((int)(wheelsNeededRight / FloatParameters[WPS])) % 2;
            float rightAdjuster = Mathf.RoundToInt(rightBump + wheelsNeededRight / FloatParameters[WPS]) * FloatParameters[WPS] / wheelsNeededRight;

            int leftBump = 1 - ((int)(wheelsNeededLeft / FloatParameters[WPS])) % 2;
            float leftAdjuster = Mathf.RoundToInt(leftBump + wheelsNeededLeft / FloatParameters[WPS]) * FloatParameters[WPS] / wheelsNeededLeft;

            InitializeArrays(wheelsNeededRight, wheelsNeededLeft);

            Vector3 temp = nextTower.position - prevTower.position;
            float dy = temp.y;
            temp.y = 0;
            float dx = temp.magnitude;
            Vector2 tempvec = new Vector2(dx, dy).normalized;

            float nominalIncrease = tempvec.y;
            float rightIncrease = 0.5f * nominalIncrease * wheelsNeeded / wheelsNeededRight;
            float leftIncrease = 0.5f * nominalIncrease * wheelsNeeded / wheelsNeededLeft;

            float centralHeight = (prevTower.position.y + nextTower.position.y) / 2;
            float heightAdjust = centralHeight - transform.position.y;
            float startHeight = heightAdjust - nominalIncrease * wheelsNeeded * 0.5f;

            for (int i = 0; i < wheelsNeededRight; i++)
            {
                rightArray[i].SetActive(true);
                float angle = initialAngle - angleIncreasePerWheelRight * i * curveDir;
                angle %= (Mathf.PI * 2);
                Vector3 pos = new Vector3(Mathf.Cos(angle) * rightRadius + pivotPos.x, startHeight + rightIncrease * i, Mathf.Sin(angle) * rightRadius + pivotPos.y);
                rightArray[i].transform.localPosition = pos;

                float wheelAngle = (angle + Mathf.PI * 0.5f * curveDir) * Mathf.Rad2Deg;

                int segment = (int)((i / FloatParameters[WPS]) * rightAdjuster);

                if (segment % 2 == 1)
                {
                    rightArray[i].transform.rotation = Quaternion.Euler(new Vector3(0, -wheelAngle - 90, 90));
                }
                else
                {
                    rightArray[i].transform.rotation = Quaternion.Euler(new Vector3(0, -wheelAngle + 90, 270));
                }
            }
            for (int i = 0; i < wheelsNeededLeft; i++)
            {
                leftArray[i].SetActive(true);
                float angle = initialAngle - angleIncreasePerWheelLeft * i * curveDir;
                angle %= (Mathf.PI * 2);
                Vector3 pos = new Vector3(Mathf.Cos(angle) * leftRadius + pivotPos.x, startHeight + leftIncrease * i, Mathf.Sin(angle) * leftRadius + pivotPos.y);
                leftArray[i].transform.localPosition = pos;

                float wheelAngle = (angle - Mathf.PI * 0.5f * curveDir) * Mathf.Rad2Deg;

                int segment = (int)((i / FloatParameters[WPS]) * leftAdjuster);

                if (segment % 2 == 1)
                {
                    leftArray[i].transform.rotation = Quaternion.Euler(new Vector3(0, -wheelAngle - 90, 90));
                }
                else
                {
                    leftArray[i].transform.rotation = Quaternion.Euler(new Vector3(0, -wheelAngle + 90, 270));
                }
            }
        }

        private float GetEffectiveWheelSpacing(Transform prevTower, Transform nextTower)
        {
            Vector3 temp = nextTower.position - prevTower.position;
            float dy = temp.y;
            temp.y = 0;
            float dx = temp.magnitude;
            Vector2 tempvec = new Vector2(dx, dy).normalized;
            return FloatParameters[WS] * tempvec.x;
        }

        public override List<Transform> GetCablePath(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            List<GameObject> relevant = right ? rightArray : leftArray;
            List<Transform> toReturn = new List<Transform>(relevant.Count);
            foreach(GameObject g in relevant)
            {
                if (!g.activeSelf) break;
                toReturn.Add(g.transform);
            }
            return toReturn;
        }

        public float GetLengthC(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            float radius = GetRadiusC(prevTower, nextTower);
            float length = radius / Mathf.Tan((Mathf.PI - AngleDiff(angle1, angle2)) / 2);
            return length;
        }

        public override float GetLength(float angle1, float angle2)
        {
            float radius = GetRadius();
            float length = radius / Mathf.Tan((Mathf.PI - AngleDiff(angle1, angle2)) / 2);
            return length;
        }

        private float GetRadiusC(Transform prevTower, Transform nextTower)
        {
            float nwic = 360 * FloatParameters[WPD];
            float c = GetEffectiveWheelSpacing(prevTower, nextTower) * nwic;
            float r = c / (2 * Mathf.PI);
            return r;
        }

        private float GetRadius()
        {
            float nwic = 360 * FloatParameters[WPD];
            float c = FloatParameters[WS] * nwic;
            float r = c / (2 * Mathf.PI);
            return r;
        }

        private float AngleDiff(float a1, float a2)
        {
            a1 %= (Mathf.PI * 2);
            a2 %= (Mathf.PI * 2);
            float d1 = Mathf.Abs(a2 - a1);
            if (d1 > Mathf.PI)
            {
                return (Mathf.PI * 2) - d1;
            }
            return d1;
        }

        private void InitializeArrays(int rightWheelCount, int leftWheelCount)
        {
            if(rightWheelCount > rightArray.Count)
            {
                while (rightWheelCount > rightArray.Count)
                {
                    GameObject temp = Instantiate<GameObject>(LoadedData[WP], LoadedData[RWP].transform);
                    rightArray.Add(temp);
                }
            } 
            else
            {
                for(int i = rightWheelCount;i < rightArray.Count;i++)
                {
                    rightArray[i].SetActive(false);
                }
            }

            if (leftWheelCount > leftArray.Count)
            {
                while (leftWheelCount > leftArray.Count)
                {
                    GameObject temp = Instantiate<GameObject>(LoadedData[WP], LoadedData[RWP].transform);
                    leftArray.Add(temp);
                }
            }
            else
            {
                for (int i = leftWheelCount; i < leftArray.Count; i++)
                {
                    leftArray[i].SetActive(false);
                }
            }
        }

        private List<GameObject> rightArray = new List<GameObject>();
        private List<GameObject> leftArray = new List<GameObject>();
    }
}