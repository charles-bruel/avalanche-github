using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x0200091E RID: 2334
public class ModdedMap
{
	// Token: 0x06002D12 RID: 11538 RVA: 0x000B1BBC File Offset: 0x000AFDBC
	private ModdedMap()
	{
		this.Trees = -1;
		this.Fronts = new List<Vector3>();
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000B1CB0 File Offset: 0x000AFEB0
	public static ModdedMap Load(string path)
	{
		CultureInfo.CurrentCulture = new CultureInfo("en-US");
		ModdedMap result;
		try
		{
			float num = 50000f;
			ModdedMap moddedMap = new ModdedMap();
			moddedMap.MapPath = path;
			if (!File.Exists(path + "/meta"))
			{
				Console.WriteLine("Couldn't find map at " + path + ", assuming its been uninstalled");
				return null;
			}
			string[] array = File.ReadAllLines(path + "/meta");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i][0] != '#')
				{
					string[] array2 = array[i].Split(new char[]
					{
						'='
					});
					string text = array2[0].ToLower();
					if (text == "diversity")
					{
						moddedMap.ParameterDiversity = float.Parse(array2[1]);
					}
					else if (text == "difficulty")
					{
						moddedMap.Difficulty = float.Parse(array2[1]);
					}
					else if (text == "name")
					{
						moddedMap.Name = array2[1];
					}
					else if (text == "accessability")
					{
						moddedMap.ParameterAccessibility = float.Parse(array2[1]);
					}
					else if (text == "homogeneity" || text == "homogenity")
					{
						moddedMap.ParameterHomogeneity = float.Parse(array2[1]);
					}
					else if (text == "obstacles" || text == "obstacle")
					{
						moddedMap.ParameterObstacles = float.Parse(array2[1]);
					}
					else if (text == "usedefaultfronts")
					{
						moddedMap.UseDefaultFronts = bool.Parse(array2[1]);
					}
					else if (text == "thumb")
					{
						Texture2D texture2D = ModdedMap.LoadPNG(path + "/" + array2[1]);
						moddedMap.Thumb = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					}
					else if (text == "heightmap")
					{
						moddedMap.HeightMapPath = path + "/" + array2[1];
					}
					else if (text == "decoration")
					{
						moddedMap.DecorationMapsPath = path + "/" + array2[1];
					}
					else if (text == "height")
					{
						moddedMap.Height = float.Parse(array2[1]);
					}
					else if (text == "size")
					{
						moddedMap.Size = int.Parse(array2[1]);
					}
					else if (text == "buildable")
					{
						moddedMap.ParameterBuildable = float.Parse(array2[1]);
					}
					else if (text == "base")
					{
						int num2 = int.Parse(array2[1]);
						moddedMap.Base = Core.Levels[num2];
						moddedMap.BaseID = num2;
					}
					else if (text == "snowfront")
					{
						string[] array3 = array2[1].Split(new char[]
						{
							','
						});
						if (array3.Length != 3)
						{
							Console.WriteLine("Incorrect number of parameters for snowfront!");
						}
						else
						{
							moddedMap.Fronts.Add(new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2])));
						}
					}
					else if (text == "treelower")
					{
						moddedMap.LowerBound = float.Parse(array2[1]);
					}
					else if (text == "treeupper")
					{
						moddedMap.UpperBound = float.Parse(array2[1]);
					}
					else if (text == "treeupperslope")
					{
						moddedMap.UpperSlopeBound = float.Parse(array2[1]);
					}
					else if (text == "treelowerslope")
					{
						moddedMap.LowerSlopeBound = float.Parse(array2[1]);
					}
					else if (text == "treesrel")
					{
						num = float.Parse(array2[1]);
					}
					else if (text == "treesraw")
					{
						moddedMap.Trees = int.Parse(array2[1]);
						if (ConfigHelper.GetFloat("ModData\\basic.cfg", "tree_cloning_refine") == -1f)
						{
							moddedMap.Trees = (int)Math.Round((double)((float)moddedMap.Trees * ConfigHelper.GetFloat("ModData\\basic.cfg", "tree_multiplier")));
						}
					}
					else if (text == "rocks")
					{
						moddedMap.Rocks = int.Parse(array2[1]);
					}
					else if (text == "smoothmap")
					{
						moddedMap.SmoothMap = bool.Parse(array2[1]);
					}
					else if (text == "rockstyle")
					{
						moddedMap.RocksStyle = int.Parse(array2[1]);
					}
					else if (text == "rocklowerslope" || text == "rocklowerbound" || text == "rockslowerbound")
					{
						moddedMap.RocksLowerBound = float.Parse(array2[1]);
					}
					else if (text == "rockupperslope" || text == "rockupperbound" || text == "rocksupperbound")
					{
						moddedMap.RocksUpperBound = float.Parse(array2[1]);
					}
					else if (text == "blurradius")
					{
						moddedMap.BlurRadius = int.Parse(array2[1]);
					}
					else
					{
						Console.WriteLine("Cannot find parameter type: " + text);
					}
				}
			}
			if (moddedMap.UseDefaultFronts)
			{
				moddedMap.SnowfrontCount = moddedMap.Base.SnowfrontCount;
			}
			else
			{
				moddedMap.SnowfrontCount = moddedMap.Fronts.Count;
				Console.WriteLine(string.Concat(new object[]
				{
					"Loaded ",
					moddedMap.SnowfrontCount,
					" custom snowfronts for map ",
					moddedMap.Name
				}));
			}
			if (moddedMap.Trees == -1)
			{
				float num3 = num / 2560000f;
				moddedMap.Trees = (int)(num3 * (float)moddedMap.Size * (float)moddedMap.Size);
			}
			if (moddedMap.Rocks == -1)
			{
				double num4 = (double)ConfigHelper.GetFloat("ModData\\basic.cfg", "defRocks") * (double)moddedMap.Size * (double)moddedMap.Size;
				moddedMap.Rocks = (int)num4;
			}
			result = moddedMap;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Console.WriteLine(ex.StackTrace);
			result = null;
		}
		return result;
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000B233C File Offset: 0x000B053C
	public static Texture2D LoadPNG(string filePath)
	{
		Texture2D texture2D = null;
		if (File.Exists(filePath))
		{
			byte[] data = File.ReadAllBytes(filePath);
			texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
		}
		return texture2D;
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000B236C File Offset: 0x000B056C
	public static List<ModdedMap> GetMaps(string path)
	{
		List<ModdedMap> list = new List<ModdedMap>();
		if (!Directory.Exists(path))
		{
			Console.WriteLine("Unable to find maps directory");
			return new List<ModdedMap>();
		}
		string[] directories = Directory.GetDirectories(path);
		for (int i = 0; i < directories.Length; i++)
		{
			ModdedMap moddedMap = ModdedMap.Get(directories[i]);
			if (moddedMap != null)
			{
				list.Add(moddedMap);
			}
		}
		return list;
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x0001F2F5 File Offset: 0x0001D4F5
	public static ModdedMap Get(string path)
	{
		if (!ModdedMap.cache.ContainsKey(path))
		{
			ModdedMap.cache[path] = ModdedMap.Load(path);
		}
		return ModdedMap.cache[path];
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x0001F32C File Offset: 0x0001D52C
	public void LoadTextures()
	{
		this.HeightMap = ModdedMap.LoadPNG(this.HeightMapPath);
		this.DecorationMaps = ModdedMap.LoadPNG(this.DecorationMapsPath);
	}

	// Token: 0x0400214F RID: 8527
	public float Difficulty = 3f;

	// Token: 0x04002150 RID: 8528
	public float Height = 350f;

	// Token: 0x04002151 RID: 8529
	public string Name = "Unknown";

	// Token: 0x04002152 RID: 8530
	public Sprite Thumb;

	// Token: 0x04002153 RID: 8531
	public int SnowfrontCount;

	// Token: 0x04002154 RID: 8532
	public float ParameterAccessibility = 2f;

	// Token: 0x04002155 RID: 8533
	public float ParameterDiversity = 2f;

	// Token: 0x04002156 RID: 8534
	public float ParameterHomogeneity = 2f;

	// Token: 0x04002157 RID: 8535
	public float ParameterBuildable = 2f;

	// Token: 0x04002158 RID: 8536
	public float ParameterObstacles = 2f;

	// Token: 0x04002159 RID: 8537
	public Texture2D HeightMap;

	// Token: 0x0400215A RID: 8538
	public LevelData Base = Core.Levels[0];

	// Token: 0x0400215B RID: 8539
	public string MapPath;

	// Token: 0x0400215C RID: 8540
	public static ModdedMap selectedMap;

	// Token: 0x0400215D RID: 8541
	public bool UseDefaultFronts = true;

	// Token: 0x0400215E RID: 8542
	public List<Vector3> Fronts;

	// Token: 0x0400215F RID: 8543
	public int Size = 1600;

	// Token: 0x04002160 RID: 8544
	public float LowerBound;

	// Token: 0x04002161 RID: 8545
	public float UpperBound = 1f;

	// Token: 0x04002162 RID: 8546
	public float UpperSlopeBound = 0.5f;

	// Token: 0x04002163 RID: 8547
	public int Trees;

	// Token: 0x04002164 RID: 8548
	public float LowerSlopeBound = 0.1f;

	// Token: 0x04002165 RID: 8549
	public int Rocks = -1;

	// Token: 0x04002166 RID: 8550
	private static Dictionary<string, ModdedMap> cache = new Dictionary<string, ModdedMap>();

	// Token: 0x04002167 RID: 8551
	public bool SmoothMap = true;

	// Token: 0x04002168 RID: 8552
	public Texture2D DecorationMaps;

	// Token: 0x04002169 RID: 8553
	public int RocksStyle = 1;

	// Token: 0x0400216A RID: 8554
	public float RocksUpperBound = 0.9f;

	// Token: 0x0400216B RID: 8555
	public float RocksLowerBound = 0.2f;

	// Token: 0x0400216C RID: 8556
	public int BaseID;

	// Token: 0x0400216D RID: 8557
	public string HeightMapPath;

	// Token: 0x0400216E RID: 8558
	public string DecorationMapsPath;

	// Token: 0x0400216F RID: 8559
	public int BlurRadius = 2;
}
