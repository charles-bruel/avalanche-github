using PrecompiledExtensions;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SheaveSystem
{
    public class Station : LiftStationCablePathFetcher
    {
        private float Length;
        private float Distance;
        private float TowerHeightDelta;
        private TowerAssemblyScript TowerAssembly;
        private bool IsTop;
        private float WheelSize;

        private bool Initialized = false;
        private bool Above;
        private Vector3 TowerOGPos;
        private float DroopAmount;

        void Initialize()
        {
            Length = FloatParameters[0];
            Distance = FloatParameters[1];
            TowerHeightDelta = FloatParameters[2];
            TowerAssembly = LoadedData[0].GetComponent<TowerAssemblyScript>();
            IsTop = IntParameters[0] != 0;
            TowerOGPos = TowerAssembly.transform.localPosition;
            WheelSize = FloatParameters[3];
            DroopAmount = FloatParameters[4];

            Initialized = true;
        }


        public override List<Transform> GetCablePath(Transform otherTower, Transform stationPos, Transform relevantCablePoint, bool right)
        {
            //this.OnParameterUpdate(otherTower, stationPos);
            if (!IsTop) right =! right;
            List<Transform> wheelPoints = (right ? TowerAssembly.SheaveScriptRight : TowerAssembly.SheaveScriptLeft).GetAllCablePoints(Above);
            List<Transform> stationPoints = base.GetCablePath(otherTower, stationPos, relevantCablePoint, right);
            if (!IsTop)
            {
                if (right)
                {
                    wheelPoints.Reverse();
                    stationPoints.AddRange(wheelPoints);
                    return stationPoints;
                }
                else
                {
                    stationPoints.AddRange(wheelPoints);
                    return stationPoints;
                }
            }
            else
            {
                if (right)
                {
                    wheelPoints.AddRange(stationPoints);
                    return wheelPoints;
                }
                else
                {
                    wheelPoints.Reverse();
                    wheelPoints.AddRange(stationPoints);
                    return wheelPoints;
                }
            }
        }

        public override void OnParameterUpdate(Transform otherTower, Transform stationPos)
        {
            if (!Initialized) Initialize();
            Vector3 dif = otherTower.position - stationPos.position;
            float yDif = dif.y;
            dif.y = 0;
            float xDif = dif.magnitude - Distance;

            float xOff = 0, yOff = 0;

            float angle = 0;
            for (int i = 0;i < 1; i++)
            {
                angle = Mathf.Atan((yDif + yOff) / (xDif + xOff));
                float radius = Length / angle;
                yOff = radius - radius * Mathf.Cos(angle);
                xOff = -radius * Mathf.Sin(angle);
            }

            angle *= Mathf.Rad2Deg;
            angle -= DroopAmount;

            if (float.IsNaN(angle))
            {
                TowerAssembly.EndAngle = 0;
                Above = true;
            }
            else
            {
                TowerAssembly.EndAngle = angle;
                if(angle > 0)
                {
                    Above = false;
                }
                else
                {
                    Above = true;
                }
            }
            if (Above)
            {
                TowerAssembly.transform.localPosition = TowerOGPos;
            } 
            else
            {
                Vector3 temp = TowerOGPos;
                temp.y += WheelSize;
                TowerAssembly.transform.localPosition = temp;
            }
            TowerAssembly.Reset();
        }
    }
}