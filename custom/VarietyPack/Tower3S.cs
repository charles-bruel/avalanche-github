using UnityEngine;
using PrecompiledExtensions;
using System;
using System.Collections.Generic;

namespace VarietyPack
{
    public class Tower3S : LiftTowerCablePathFetcher
    {
        public SaddleControl right;
        public SaddleControl left;

        void Start()
        {
            right = LoadedData[0].GetComponent<SaddleControl>();
            left = LoadedData[1].GetComponent<SaddleControl>();
        }

        public override void OnParameterUpdate(Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            float horizontalDistance, verticalDistance;
            Vector3 temp = nextTower.position - prevTower.position;
            verticalDistance = temp.y;
            temp.y = 0;
            horizontalDistance = temp.magnitude;
            temp = currentTowerPos.position - prevTower.position;
            float verticalOffset = temp.y;
            temp.y = 0;

            Vector2 z = new Vector2(0, 0);
            Vector2 p2 = new Vector2(temp.magnitude, verticalOffset);
            Vector2 p3 = new Vector2(horizontalDistance, verticalDistance) - p2;

            Vector3 sol1 = Utils.GetCaternery(z, p2, 0.5f);
            Vector3 sol2 = Utils.GetCaternery(z, p3, 0.5f);
            double intoSlope = Utils.EvalCaterneryPrime(sol1.x, sol1.y, sol1.z, p2.x);
            double outofSlope = Utils.EvalCaterneryPrime(sol2.x, sol2.y, sol2.z, 0);
            float intoAngle = (float)Math.Atan(intoSlope);
            float outofAngle = (float)Math.Atan(outofSlope);

            if(intoAngle - outofAngle < 0)
            {
                //outofAngle *= -1;
            }

            float delta = (intoAngle - outofAngle) * Mathf.Rad2Deg;
            float avg = (intoAngle + outofAngle) * 0.5f;

            LoadedData[2].transform.localEulerAngles = new Vector3(-avg * Mathf.Rad2Deg, 90, 0);

            right.TargetAngle = delta;
            left.TargetAngle = delta;
            right.Apply();
            left.Apply();
        }

        public override List<Transform> GetCablePath(Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            OnParameterUpdate(prevTower, nextTower, currentTowerPos);
            SaddleControl saddle = right ? this.right : left;
            List<Transform> to_return = new List<Transform>(saddle.Depth);
            Transform current = saddle.FirstBone;
            bool flag = true;
            while(flag)
            {
                int num = current.childCount;
                if(num == 1)
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
            to_return.Reverse();
            return to_return;
        }
    }
}
