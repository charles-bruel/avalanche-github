using System.Collections.Generic;
using PrecompiledExtensions;
using UnityEngine;

namespace VarietyPack
{
    class PlacerT1 : TowerPlacementAlgorithm
    {
        public override void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)
        {
            constructionConstraintsData[4][2] = 50;
            constructionConstraintsData[4][3] = 25;
            base.PlaceTowers(constructionConstraintsData, terrainPositions, towerIndices);
            if (towerIndices.Count > 3)
            {
                towerIndices.RemoveAt(1);
                towerIndices.RemoveAt(towerIndices.Count - 2);
            }
        }
    }
}
