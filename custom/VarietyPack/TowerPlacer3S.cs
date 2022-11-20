using System;
using System.Collections.Generic;
using UnityEngine;
using PrecompiledExtensions;

namespace VarietyPack
{
    class TowerPlacer3S : TowerPlacementAlgorithm
    {
        public override void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)
        {
            base.PlaceTowers(constructionConstraintsData, terrainPositions, towerIndices);

        Start:
            for (int i = 0; i < towerIndices.Count - 2; i++)
            {
                if (CheckSag(terrainPositions, towerIndices[i], towerIndices[i + 1], towerIndices[i + 2]))
                {
                    towerIndices.RemoveAt(i + 1);
                    goto Start;
                }
            }
        }

        //https://math.stackexchange.com/questions/3557767/how-to-construct-a-catenary-of-a-specified-length-through-two-specified-points
        private bool CheckSag(List<Vector3> terrainPositions, int first, int second, int third)
        {
            Vector3 t1 = terrainPositions[first];
            Vector3 t2 = terrainPositions[second];
            Vector3 t3 = terrainPositions[third];

            float horizontalDistance, verticalDistance;
            Vector3 temp = t3 - t1;
            verticalDistance = temp.y;
            temp.y = 0;
            horizontalDistance = temp.magnitude;

            float a, b, c;//Answer goes here
            Vector2 p1 = new Vector2(0, 0);
            Vector2 p2 = new Vector2(horizontalDistance, verticalDistance);
            Vector3 solution = Utils.GetCaternery(p1, p2, 0.5f);

            a = solution.x;
            b = solution.y;
            c = solution.z;
            
            //Now going to make the assumption that t2 is colinear with t1 and t3
            //Since p1 is 0, 0, the horizontal distance between t1 and t2 can be used as x
            //That then outputs the relative height from the sag at t2 to the height of t1
            temp = t2 - t1;
            verticalDistance = temp.y;
            temp.y = 0;
            horizontalDistance = temp.magnitude;
            Vector2 p3 = new Vector2(horizontalDistance, verticalDistance);//p3 is for t2

            float sagHeight = (float)Utils.EvalCaternery(a, b, c, p3.x);
            //so if the sag height is above the tower minus a margin, then the tower is unnessecary.
            return sagHeight > p3.y - 8;
        }

    }
}
