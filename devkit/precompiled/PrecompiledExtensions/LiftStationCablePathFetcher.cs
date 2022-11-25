using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrecompiledExtensions
{
    public class LiftStationCablePathFetcher : ExtensionItem
    {
        public virtual void OnParameterUpdate(Transform otherTower, Transform stationPos)
        {

        }

        public virtual List<Transform> GetCablePath(Transform otherTower, Transform stationPos, Transform relevantCablePoint, bool right)
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

        public virtual List<Transform> GetWirePoints()
        {
            return new List<Transform>();
        }
    }
}
