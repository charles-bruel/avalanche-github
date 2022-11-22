using UnityEngine;
using System.Collections.Generic;
using PrecompiledExtensions;

namespace VarietyPack
{
    public class SegmentLowerCarpet : LiftStationCablePathFetcher
    {
        private Transform pivot;

        public override void OnParameterUpdate(Transform otherTower, Transform stationPos)
        {
            if (pivot == null)
            {
                pivot = transform.GetChild(1);
            }
            Vector3 delta = otherTower.position - stationPos.position;
            float len = delta.magnitude;
            float y = delta.y;
            delta.y = 0;
            float x = delta.magnitude;
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            pivot.localEulerAngles = new Vector3(angle, -90, 0);
            pivot.localScale = new Vector3(0.5f, 1, len * 1.1f);
        }
    }
}
