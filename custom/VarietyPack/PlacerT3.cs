using System.Collections.Generic;
using PrecompiledExtensions;
using UnityEngine;

namespace VarietyPack
{
    class PlacerT3 : TowerPlacementAlgorithm
    {
        public override void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)
        {
            constructionConstraintsData[4][2] = 120;
            base.PlaceTowers(constructionConstraintsData, terrainPositions, towerIndices);
            if (towerIndices.Count > 2)
            {
                Vector3 tempLower = terrainPositions[towerIndices[towerIndices.Count - 3]];
                Vector3 tempHigher = terrainPositions[towerIndices[towerIndices.Count - 1]];
                Vector3 tempVal = terrainPositions[towerIndices[towerIndices.Count - 2]];
                if (SignedDistanceFromLineAssumeCenter(tempVal, tempLower, tempHigher) < -10)
                {
                    towerIndices.RemoveAt(towerIndices.Count - 2);
                }
            }
        }
    }
}
