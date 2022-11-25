using PrecompiledExtensions;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VarietyPack
{
    public class Station3S : LiftStationCablePathFetcher
    {
        public SaddleControl right;
        public SaddleControl left;

        public GameObject[] supports;

        public bool Initialized;

        void Start()
        {
        }

        public void Initialize()
        {
            if (Initialized) return;
            Initialized = true;

            right = LoadedData[0].GetComponent<SaddleControl>();
            left = LoadedData[1].GetComponent<SaddleControl>();

            supports = new GameObject[IntParameters[1]];
            for(int i = 0;i < IntParameters[1];i ++)
            {
                supports[i] = Instantiate<GameObject>(LoadedData[3], LoadedData[2].transform);
            }
        }

        public override void OnParameterUpdate(Transform otherTower, Transform stationPos)
        {
            Initialize();

            float horizontalDistance, verticalDistance;
            Vector3 temp = otherTower.position - stationPos.position;
            verticalDistance = temp.y;
            temp.y = 0;
            horizontalDistance = temp.magnitude;

            Vector2 z = new Vector2(0, 0);
            Vector2 p = new Vector2(horizontalDistance, verticalDistance);

            Vector3 sol = Utils.GetCaternery(z, p, 0.5f);
            double slope = Utils.EvalCaterneryPrime(sol.x, sol.y, sol.z, 0);
            float angle = (float)Math.Atan(slope) * Mathf.Rad2Deg;

            right.TargetAngle = -angle;
            left.TargetAngle = -angle;
            right.Apply();
            left.Apply();

            for(int i = 0; i < IntParameters[1]; i++)
            {
                int bi = 5 + i * 2;
                Vector3 p1 = LoadedData[bi + 0].transform.position;
                Vector3 p2 = LoadedData[bi + 1].transform.position;
                Vector3 avg = (p1 + p2) * 0.5f;
                supports[i].transform.position = avg;

                temp = avg - LoadedData[4].transform.position;
                float y = temp.y;
                temp.y = 0;
                float x = temp.magnitude;
                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                supports[i].transform.localEulerAngles = new Vector3(0, 0, -90 + angle);
            }
        }

        public override List<Transform> GetCablePath(Transform otherTower, Transform stationPos, Transform relevantCablePoint, bool right)
        {
            OnParameterUpdate(otherTower, stationPos);
            SaddleControl saddle = right ? this.right : left;
            List<Transform> to_return = new List<Transform>(saddle.Depth + 1);
            to_return.Add(relevantCablePoint);
            Transform current = saddle.FirstBone;
            bool flag = true;
            while (flag)
            {
                int num = current.childCount;
                if (num == 1)
                {
                    //Base case
                    flag = false;
                    to_return.Add(current.GetChild(0));
                }
                else
                {
                    to_return.Add(current.GetChild(1));
                    current = current.GetChild(0);
                }
            }
            if (IntParameters[0] != 0) to_return.Reverse();
            return to_return;
        }
    }
}
