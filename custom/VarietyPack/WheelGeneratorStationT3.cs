using System;
using UnityEngine;

namespace VarietyPack
{
    public class WheelGeneratorStationT3 : MonoBehaviour, IWheelGenerator
    {

        public void UpdatePositions()
        {
            Initialize();
            if (!bottom)
            {
                if (!above)
                {
                    Vector3 offset = new Vector3(0, radius + (cableAboveWheel ? -wheelRadius : wheelRadius), 0);
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        float theta = i * anglePerWheel * Mathf.Deg2Rad;
                        Vector3 angleOffset = new Vector3(0, -Mathf.Cos(theta) * radius, -Mathf.Sin(theta) * radius);
                        wheels[i].transform.localPosition = offset + angleOffset;
                        wheels[i].transform.localRotation = Quaternion.Euler(-theta * Mathf.Rad2Deg, 0, 90);
                        wheels[i].GetComponent<WheelScript>().above = cableAboveWheel;
                    }
                }
                else
                {
                    Vector3 offset = new Vector3(0, -radius + (cableAboveWheel ? -wheelRadius : wheelRadius), 0);
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        float theta = i * anglePerWheel * Mathf.Deg2Rad;
                        Vector3 angleOffset = new Vector3(0, Mathf.Cos(theta) * radius, -Mathf.Sin(theta) * radius);
                        wheels[i].transform.localPosition = offset + angleOffset;
                        wheels[i].transform.localRotation = Quaternion.Euler(-theta * Mathf.Rad2Deg, 0, 90);
                        wheels[i].GetComponent<WheelScript>().above = cableAboveWheel;
                    }
                }
            }
            else
            {
                if (!above)
                {
                    Vector3 offset = new Vector3(0, radius + (cableAboveWheel ? -wheelRadius : wheelRadius), 0);
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        float theta = i * anglePerWheel * Mathf.Deg2Rad;
                        Vector3 angleOffset = new Vector3(0, -Mathf.Cos(theta) * radius, -Mathf.Sin(theta) * radius);
                        wheels[i].transform.localPosition = offset + angleOffset;
                        wheels[i].transform.localRotation = Quaternion.Euler(theta * Mathf.Rad2Deg, 0, 90);
                        wheels[i].GetComponent<WheelScript>().above = cableAboveWheel;
                    }
                }
                else
                {
                    Vector3 offset = new Vector3(0, -radius + (cableAboveWheel ? -wheelRadius : wheelRadius), 0);
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        float theta = i * anglePerWheel * Mathf.Deg2Rad;
                        Vector3 angleOffset = new Vector3(0, Mathf.Cos(theta) * radius, -Mathf.Sin(theta) * radius);
                        wheels[i].transform.localPosition = offset + angleOffset;
                        wheels[i].transform.localRotation = Quaternion.Euler(theta * Mathf.Rad2Deg, 0, 90);
                        wheels[i].GetComponent<WheelScript>().above = cableAboveWheel;
                    }
                }
            }
            for(int i = 0;i < wheels.Length;i++)
            {
                WheelScript temp = wheels[i].GetComponent<WheelScript>();
                Vector3 temp2 = temp.transform.eulerAngles;
                temp2.x = 180;
                temp.catwalk.transform.eulerAngles = temp2;
            }
        }


        public float lerp(float a, float b, float c)
        {
            return a * (1f - c) + b * c;
        }

        public WheelScript[] GetAllWheels()
        {
            Initialize();
            WheelScript[] array = new WheelScript[wheels.Length];
            for (int i = 0; i < wheels.Length; i++)
            {
                array[i] = this.wheels[wheels.Length - (i + 1)].GetComponent<WheelScript>();
            }
            return array;
        }

        public void Initialize()
        {
            float distanceMax = 360f * wheelSpacing / anglePerWheel;
            radius = distanceMax / (Mathf.PI * 2f);

            Vector3 delta = target.position - basePos.position;
            Vector2 flattenedDelta = new Vector2(Mathf.Sqrt(delta.x * delta.x + delta.z * delta.z), delta.y);

            if (flattenedDelta.y < 0)
            {
                above = true;
                cableAboveWheel = true;
            }
            else
            {
                above = false;
                cableAboveWheel = false;
            }

            Vector4 temp = GetLinesFromTangent(new Vector2(0, above ? -radius : radius), radius, flattenedDelta);

            //float targetAngle = Mathf.Atan2(flattenedDelta.y, flattenedDelta.x) * 57.29578f;
            float targetAngle;
            if (above)
            {
                targetAngle = Mathf.Atan(temp.x) * 57.29578f;
            }
            else
            {
                targetAngle = Mathf.Atan(temp.z) * 57.29578f;
            }

            while (targetAngle < 0)
            {
                targetAngle += 360;
            }

            while (targetAngle > 360)
            {
                targetAngle -= 360;
            }

            
            if (targetAngle > 180)
            {
                targetAngle = 360 - targetAngle;
            } 

            int num = (int)Mathf.Floor(targetAngle / anglePerWheel);
            num--;

            if (num < 3) num = 3;

            if (num < 10) cableAboveWheel = true;

            GameObject[] newWheels = new GameObject[num];

            if (num < wheels.Length)
            {
                for (int i = wheels.Length - 1; i >= num; i--)
                {
                    DestroyImmediate(wheels[i]);
                }
                for (int i = 0; i < num; i++)
                {
                    newWheels[i] = wheels[i];
                }
                wheels = newWheels;
            }
            else if (num > wheels.Length)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    newWheels[i] = wheels[i];
                }
                for (int i = wheels.Length; i < num; i++)
                {
                    newWheels[i] = Instantiate(wheelPrefab, transform);
                    newWheels[i].GetComponent<WheelScript>().catwalk = Instantiate<GameObject>(catwalkPrefab, newWheels[i].transform);
                    newWheels[i].GetComponent<WheelScript>().catwalk.transform.localScale = new Vector3(1, 1.25f, 1);
                }
                wheels = newWheels;
            }

            if (HasInitialized)
            {
                return;
            }

            HasInitialized = true;
        }

        //Adapted from https://math.stackexchange.com/questions/543496/how-to-find-the-equation-of-a-line-tangent-to-a-circle-that-passes-through-a-g
        //The answer with python pseudocode
        private Vector4 GetLinesFromTangent(Vector2 circleCenter, float circleRadius, Vector2 targetPoint)
        {
            float dx = targetPoint.x - circleCenter.x;
            float dy = targetPoint.y - circleCenter.y;
            float dxr = -dy;
            float dyr = dx;
            float d = Mathf.Sqrt(dx * dx + dy * dy);

            if(d < circleRadius)
            {
                return default(Vector4);
            }

            float rho = circleRadius / d;
            float ad = rho * rho;
            float bd = rho * Mathf.Sqrt(1 - (rho*rho));

            float T1x = circleCenter.x + ad * dx + bd * dxr;
            float T1y = circleCenter.y + ad * dy + bd * dyr;
            float T2x = circleCenter.x + ad * dx - bd * dxr;
            float T2y = circleCenter.y + ad * dy - bd * dyr;

            float E1a = targetPoint.y - T1y;
            float E1b = T1x - targetPoint.x;
            float E1c = -(T1y * targetPoint.x - T1x * targetPoint.y);
            float E1M = (T1y - targetPoint.y) / (T1x - targetPoint.x);
            float E1B = T1y - E1M * T1x;

            float E2a = targetPoint.y - T2y;
            float E2b = T2x - targetPoint.x;
            float E2c = -(T2y * targetPoint.x - T2x * targetPoint.y);
            float E2M = (T2y - targetPoint.y) / (T2x - targetPoint.x);
            float E2B = T2y - E2M * T2x;

            return new Vector4(E1M, E1B, E2M, E2B);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        private float radius;

        public Transform target;

        public Transform basePos;

        public float anglePerWheel;

        public GameObject wheelPrefab;

        public float wheelSpacing;

        public GameObject[] wheels = new GameObject[0];

        public int totalWheelCount;

        public float totalWheelLength;

        private bool HasInitialized;

        public bool above = false;

        public bool bottom = false;

        public bool cableAboveWheel = false;

        public float wheelRadius;

        public GameObject catwalkPrefab;
    }

}
