using UnityEngine;
using System.Collections.Generic;
using PrecompiledExtensions;

namespace VarietyPack
{
    public class SegmentCarpet : LiftTowerCablePathFetcher
    {
        private Transform pivot;

        public override void OnParameterUpdate(Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            if(pivot == null)
            {
                pivot = transform.GetChild(0);
            }
            Vector3 delta = nextTower.position - currentTowerPos.position;
            float len = delta.magnitude;
            float y = delta.y;
            delta.y = 0;
            float x = delta.magnitude;
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            pivot.localEulerAngles = new Vector3(angle, -90, 0);
            pivot.localScale = new Vector3(0.5f, 1, len);
        }
    }
}
