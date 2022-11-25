using System;
using System.Collections.Generic;
using System.Text;
using PrecompiledExtensions;
using UnityEngine;

namespace SheaveSystem
{
    class Tower : LiftTowerCablePathFetcher
    {
        private TowerAssemblyScript TowerAssembly;
        private float WheelSize;

        private bool Initialized = false;
        private bool Above;
        private Vector3 TowerOGPos;
        private float DroopAmount;

        private void Initialize()
        {
            TowerAssembly = LoadedData[0].GetComponent<TowerAssemblyScript>();
            WheelSize = FloatParameters[0];
            TowerOGPos = TowerAssembly.transform.localPosition;
            DroopAmount = FloatParameters[1];

            Initialized = true;
        }

        public override List<Transform> GetCablePath(Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            List<Transform> temp = (right ? TowerAssembly.SheaveScriptRight : TowerAssembly.SheaveScriptLeft).GetAllCablePoints(Above);
            if (!right) temp.Reverse();
            return temp;
        }

        public override void OnParameterUpdate(Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            if (!Initialized) Initialize();

            Vector3 dif = prevTower.position - currentTowerPos.position;
            float yDif = dif.y;
            dif.y = 0;
            float xDif = dif.magnitude;
            float endAngle = Mathf.Atan(yDif / xDif) * Mathf.Rad2Deg;

            dif = currentTowerPos.position - nextTower.position;
            yDif = dif.y;
            dif.y = 0;
            xDif = dif.magnitude;
            float startAngle = Mathf.Atan(yDif / xDif) * Mathf.Rad2Deg + 180;

            endAngle -= DroopAmount;
            startAngle += DroopAmount;

            TowerAssembly.EndAngle = endAngle;
            TowerAssembly.StartAngle = startAngle;
            TowerAssembly.CurrentDroopAmount = DroopAmount;

            if (startAngle < 0)
            {
                startAngle += 360;
            }
            if (endAngle < 0)
            {
                endAngle += 360;
            }
            if (endAngle > startAngle)
            {
                startAngle += 360;
            }
            float RequiredAngle = startAngle - endAngle;
            if (RequiredAngle > 180)
            {
                RequiredAngle = -360 + RequiredAngle;
            }

            Above = RequiredAngle < 0;

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

        public override List<Transform> GetWirePoints()
        {
            if (TowerAssembly != null && TowerAssembly.Towers != null)
            {
                List<Transform> to_return = new List<Transform>(TowerAssembly.Towers.Count);
                for (int i = 0; i < TowerAssembly.Towers.Count; i++)
                {
                    if (TowerAssembly.Towers[i].isActiveAndEnabled && TowerAssembly.Towers[i].WirePoint != null)
                        to_return.Add(TowerAssembly.Towers[i].WirePoint);
                }
                to_return.Reverse();
                return to_return;
            }
            else
            {
                return base.GetWirePoints();
            }
        }
    }
}
