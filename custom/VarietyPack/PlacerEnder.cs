using System.Collections.Generic;
using PrecompiledExtensions;
using UnityEngine;

namespace VarietyPack
{
    class PlacerEnder : TowerPlacementAlgorithm
    {
        public override void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)
        {
            base.PlaceTowers(constructionConstraintsData, terrainPositions, towerIndices);
            if (terrainPositions.Count > 50 && towerIndices.Count > 5)
            {
                int temp = towerIndices[towerIndices.Count - 1];
                towerIndices[towerIndices.Count - 1] = terrainPositions.Count - 10;
                towerIndices.Add(temp);

            }
        }
    }
}
