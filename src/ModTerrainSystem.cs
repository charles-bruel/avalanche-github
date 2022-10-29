using System;
using TMPro;
using UnityEngine;

// Token: 0x0200091F RID: 2335
public partial class ModTerrainSystem
{
	// Token: 0x06002D19 RID: 11545 RVA: 0x000B23C0 File Offset: 0x000B05C0
	public static void RebumpLodge(Terrain terrain)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>(true);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].ToString();
			if (text.Contains("Snowfront") && !text.Contains("Snowfronts"))
			{
				Vector3 position = array[i].gameObject.transform.position;
				position.y = terrain.SampleHeight(position);
				array[i].gameObject.transform.position = position;
			}
			else if (text.Contains("Village") || text.Contains("House") || text.Contains("Water_Lake"))
			{
				UnityEngine.Object.DestroyImmediate(array[i]);
			}
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000B2470 File Offset: 0x000B0670
	public static float[,] Smooth(float[,] data, int size, int blurSize)
	{
		float[,] array = new float[size, size];
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (i < blurSize || j < blurSize || i >= size - blurSize || j >= size - blurSize)
				{
					array[j, i] = data[j, i];
				}
				else
				{
					float num = 0f;
					for (int k = -blurSize; k <= blurSize; k++)
					{
						for (int l = -blurSize; l <= blurSize; l++)
						{
							num += data[j + k, i + l];
						}
					}
					array[j, i] = num / (float)((blurSize * 2 + 1) * (blurSize * 2 + 1));
				}
			}
		}
		return array;
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x000B2628 File Offset: 0x000B0828
	public static void CreateLodges(ModdedMap map)
	{
		GameObject gameObject = null;
		foreach (GameObject gameObject2 in UnityEngine.Object.FindObjectsOfType<GameObject>())
		{
			if (gameObject2.ToString().Contains("Snowfronts"))
			{
				gameObject = gameObject2;
			}
		}
		if (gameObject == null)
		{
			Console.WriteLine("Unable to get Snowfront");
			return;
		}
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		GameObject[] array2 = new GameObject[gameObject.transform.childCount];
		for (int j = 0; j < gameObject.transform.childCount; j++)
		{
			array2[j] = gameObject.transform.GetChild(j).gameObject;
		}
		for (int k = 0; k < array2[0].transform.childCount; k++)
		{
			if (k != 1)
			{
				array2[0].transform.GetChild(k).transform.localPosition -= array2[0].transform.GetChild(1).transform.localPosition;
			}
			array2[0].transform.GetChild(k).transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		array2[0].transform.GetChild(2).transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		array2[0].transform.GetChild(1).transform.localPosition = new Vector3(-10f, 0f, 0f);
		foreach (Vector3 vector in map.Fronts)
		{
			UnityEngine.Object.Instantiate<GameObject>(array2[0], new Vector3(vector.x * (float)map.Size - (float)(map.Size / 2), 0f, vector.y * (float)map.Size - (float)(map.Size / 2)), Quaternion.Euler(new Vector3(0f, vector.z + ((map.BaseID > 1) ? 180f : 0f), 0f)), gameObject.transform);
		}
		for (int l = 0; l < array2.Length; l++)
		{
			if (array2[l] != ModTerrainSystem.cached_lodge)
			{
				UnityEngine.Object.DestroyImmediate(array2[l]);
			}
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x000B28B8 File Offset: 0x000B0AB8
	public static void LoadTerrain(Texture2D texture2D, TerrainData aTerrain, Terrain terrain, ModdedMap moddedMap)
	{
		aTerrain.heightmapResolution = texture2D.width;
		Vector3 size = aTerrain.size;
		size.y = moddedMap.Height;
		size.x = (float)moddedMap.Size;
		size.z = (float)moddedMap.Size;
		aTerrain.size = size;
		int width = texture2D.width;
		float[,] array = new float[width, width];
		Color[] pixels = texture2D.GetPixels(0, 0, width, width);
		for (int i = 0; i < pixels.Length; i++)
		{
			Color color = pixels[i];
			int num = (int)(color.b * 255f) << 16 | (int)(color.g * 255f) << 8 | (int)(color.r * 255f);
			array[i / width, i % width] = (float)num / 16777215f;
		}
		try
		{
			int blurRadius = moddedMap.BlurRadius;
			if (moddedMap.SmoothMap)
			{
				aTerrain.SetHeights(0, 0, ModTerrainSystem.Smooth(ModTerrainSystem.Smooth(ModTerrainSystem.Smooth(array, width, blurRadius), width, blurRadius), width, blurRadius));
			}
			else
			{
				aTerrain.SetHeights(0, 0, ModTerrainSystem.Smooth(array, width, blurRadius));
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Console.WriteLine(ex.StackTrace);
			Console.WriteLine("Error setting size. Make sure your heightmap is square and 2^n or 2^n+1 in size.");
		}
		terrain.gameObject.transform.position = new Vector3(-(float)moddedMap.Size / 2f, 0f, -(float)moddedMap.Size / 2f);
		ModTerrainSystem.DoneLoading = true;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x000B2A3C File Offset: 0x000B0C3C
	public static void DeleteRocks()
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].ToString().Contains("Rocks"))
			{
				UnityEngine.Object.DestroyImmediate(array[i]);
			}
		}
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x000B2A7C File Offset: 0x000B0C7C
	public Vector3 PositionToVector3(Transform transform, ModdedMap map)
	{
		Vector3 result;
		result.x = (transform.position.x + (float)(map.Size / 2)) / (float)map.Size;
		result.y = (transform.position.z + (float)(map.Size / 2)) / (float)map.Size;
		result.z = transform.rotation.y;
		return result;
	}

	// Token: 0x04002170 RID: 8560
	public static bool DoneLoading;

	// Token: 0x04002171 RID: 8561
	public static GameObject cached_lodge;
}
