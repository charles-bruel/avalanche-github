using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrecompiledExtensions
{
    public class SimpleWirePoint : LiftStationCablePathFetcher
    {
        public override List<Transform> GetWirePoints()
        {
            var temp = new List<Transform>();
            temp.Add(LoadedData[0].transform);
            return temp;
        }
    }
}
