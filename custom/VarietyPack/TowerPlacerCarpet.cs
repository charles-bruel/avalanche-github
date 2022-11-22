using UnityEngine;
using System.Collections.Generic;
using PrecompiledExtensions;

namespace VarietyPack
{
    class TowerPlacerCarpet : TowerPlacementAlgorithm
    {
        public override void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)
        {
            towerIndices.Capacity = terrainPositions.Count;
            for(int i = 0;i < terrainPositions.Count;i ++)
            {
                towerIndices.Add(i);
            }
        }
    }
}
