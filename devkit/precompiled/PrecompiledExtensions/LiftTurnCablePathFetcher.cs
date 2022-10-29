using System;
using UnityEngine;
using System.Collections.Generic;

namespace PrecompiledExtensions
{
	public class LiftTurnCablePathFetcher : ExtensionItem
	{
        public virtual void OnParameterUpdate(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {

        }

        public virtual float GetLength(float angle1, float angle2)
        {
            return 0;
        }

        public virtual List<Transform> GetCablePath(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            if (relevantCablePoint.childCount == 0)
            {
                return new List<Transform>(new Transform[] { relevantCablePoint });
            }
            else
            {
                List<Transform> toReturn = new List<Transform>(relevantCablePoint.childCount);
                for (int i = 0; i < relevantCablePoint.childCount; i++)
                {
                    toReturn.Add(relevantCablePoint.GetChild(i));
                }
                return toReturn;
            }
        }
    }
}