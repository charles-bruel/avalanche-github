using System.Collections.Generic;
using UnityEngine;

namespace PrecompiledExtensions
{
	public class TowerPlacementAlgorithm
	{
		public virtual Enums.TowerPlacementAlgorithmType TowerPlacementAlgorithmType()
		{
			return Enums.TowerPlacementAlgorithmType.D2;
		}

		public virtual List<Vector3> PlaceTowers3D(float[][] constructionConstraintsData, List<Vector3> terrainPositions, float[] FloatParams, int[] IntParams)
        {
			return null;
        }

        public virtual void PlaceTowers(float[][] constructionConstraintsData, List<Vector3> terrainPositions, List<int> towerIndices)//No good variable names cause copied from my modded copy of 0.13.11 from dnSpy
        {//Slight change had to be made to the parameters. Couldn't pass a template, so I passed the construction constraints.
		 // The 5 contrainsts are in order elev high, elev low, slope high, slope low, tower dist. The values of the constraints are in cost, max var, max val, min val. Same order as they appear in the inspector in unity
			float maxBest = constructionConstraintsData[4][2] * 0.5f;
			List<int> list = new List<int>();
			int num = (int)(constructionConstraintsData[4][2] * 0.35f);
			list.Add(0);
			list.Add(num);
			list.Add(terrainPositions.Count - num);
			list.Add(terrainPositions.Count - 1);
			list.AddRange(FindTowers((int)(maxBest / 2f), terrainPositions));
			Merge(list, (int)(maxBest / 3));
			list.Sort();
			List<int> list2 = new List<int>();
			for (int i = 0; i < list.Count - 1; i++)
			{
				if ((list[i] >= num && list[i] <= terrainPositions.Count - num) || list[i] == 0 || list[i] == terrainPositions.Count)
				{
					list2.Add(list[i]);
					if ((float)(list[i + 1] - list[i]) > maxBest)
					{
						int num2 = list[i + 1] - list[i];
						int num3 = (int)((float)num2 / maxBest) + 1;
						int num4 = num2 / num3;
						for (int j = 0; j < num3; j++)
						{
							list2.Add(list[i] + j * num4);
						}
					}
				}
			}
			list2.Add(list[list.Count - 1]);
			foreach (int item in list2)
			{
				if (!towerIndices.Contains(item))
				{
					towerIndices.Add(item);
				}
			}
		}

		public static List<int> FindTowers(int testDist, List<Vector3> data)
		{
			if (testDist * 2 > data.Count)
			{
				return new List<int>();
			}
			float[] array = new float[data.Count];
			for (int i = 0; i < data.Count; i++)
			{
				if (i < testDist)
				{
					Vector3 a = data[0] - data[1];
					a.y = 0f;
					int num = testDist - i;
					Vector3 linePoint = data[0] + a * (float)num;
					array[i] = SignedDistanceFromLineAssumeCenter(data[i], linePoint, data[i + testDist]);
				}
				else if (i >= data.Count - testDist)
				{
					Vector3 a2 = data[data.Count - 1] - data[data.Count - 2];
					a2.y = 0f;
					int num2 = testDist + i - (data.Count - 1);
					Vector3 linePoint2 = data[data.Count - 1] + a2 * (float)num2;
					array[i] = SignedDistanceFromLineAssumeCenter(data[i], data[i - testDist], linePoint2);
				}
				else
				{
					array[i] = SignedDistanceFromLineAssumeCenter(data[i], data[i - testDist], data[i + testDist]);
				}
			}
			return FindPeaks(array, 1f, testDist);
		}

		public static float SignedDistanceFromLineAssumeCenter(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
		{
			return DistanceFromLine(point, linePoint1, linePoint2) * (float)(((linePoint1.y + linePoint2.y) / 2f < point.y) ? 1 : -1);//Dirty dirty hack
		}

		public static List<int> FindPeaks(float[] data, float threshold, int lineTestDist)
		{
			float[] array = new float[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				bool flag = true;
				int num = 0;
				float num2 = -1f;
				while (flag)
				{
					num++;
					if (num > 1000)
					{
						flag = false;
					}
					float num3 = -10000f;
					if (i - num >= 0 && data[i - num] > num3)
					{
						num3 = data[i - num];
					}
					if (i + num <= data.Length - 1 && data[i + num] > num3)
					{
						num3 = data[i + num];
					}
					if (num3 > data[i])
					{
						flag = false;
					}
					if (num3 < data[i] - threshold)
					{
						num2 = data[i] - num3;
					}
				}
				if (num2 > 0f)
				{
					array[i] = num2 + data[i] / 5f;
				}
				else
				{
					array[i] = -1f;
				}
			}
			List<int> list = new List<int>();
			bool flag2 = true;
			while (flag2)
			{
				float num4 = -1f;
				int num5 = -1;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] > 0f && array[j] > num4)
					{
						num4 = array[j];
						num5 = j;
					}
				}
				if (num5 != -1)
				{
					list.Add(num5);
					array[num5] = -1f;
				}
				else
				{
					flag2 = false;
				}
			}
			return list;
		}

		public static float DistanceFromLine(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
		{
			return Vector3.Cross(linePoint2 - point, linePoint1 - point).magnitude / (linePoint1 - linePoint2).magnitude;
		}

		public static void Merge(List<int> toMerge, int min)
		{
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			while (!flag && num < 32767)
			{
				num++;
				for (int i = 0; i < toMerge.Count; i++)
				{
					for (int j = i + 1; j < toMerge.Count; j++)
					{
						if (Mathf.Abs(toMerge[j] - toMerge[i]) < min)
						{
							toMerge.RemoveAt(j);
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						flag2 = false;
						break;
					}
					if (i == toMerge.Count - 1)
					{
						flag = true;
					}
				}
			}
		}

	}
}
