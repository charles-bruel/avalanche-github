using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public class ConfigHelper
{
	// Token: 0x06002CD3 RID: 11475 RVA: 0x0001F084 File Offset: 0x0001D284
	public static ConfigHelper.ConfigFile GetFile(string fileName)
	{
		fileName = fileName.Replace('\\', '/');
		if (ConfigHelper.configs.ContainsKey(fileName))
		{
			return ConfigHelper.configs[fileName];
		}
		return ConfigHelper.InitializeFile(fileName);
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000B02C8 File Offset: 0x000AE4C8
	public static ConfigHelper.ConfigFile InitializeFile(string fileName)
	{
		Console.WriteLine("Loading " + fileName + " for the first time");
		CultureInfo.CurrentCulture = new CultureInfo("en-US");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		Dictionary<string, float> dictionary3 = new Dictionary<string, float>();
		Dictionary<string, Vector3> dictionary4 = new Dictionary<string, Vector3>();
		StreamReader streamReader = new StreamReader(fileName);
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			if (text.Length != 0 && text[0] != '#')
			{
				string[] array = text.Split(new char[]
				{
					' '
				});
				if (array.Length != 3)
				{
					Console.WriteLine(string.Concat(new string[]
					{
						"Invalid number of parameter on line ",
						text,
						" in file ",
						fileName,
						"."
					}));
				}
				else if (array[0] == "string")
				{
					dictionary.Add(array[1], array[2]);
				}
				else
				{
					if (array[0] == "int")
					{
						try
						{
							dictionary2.Add(array[1], int.Parse(array[2]));
							continue;
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
							Console.WriteLine("On " + array[2]);
							dictionary2.Add(array[1], 0);
							continue;
						}
					}
					if (array[0] == "float")
					{
						try
						{
							dictionary3.Add(array[1], float.Parse(array[2]));
							continue;
						}
						catch (Exception ex2)
						{
							Console.WriteLine(ex2.Message);
							Console.WriteLine("On " + array[2]);
							dictionary3.Add(array[1], 0f);
							continue;
						}
					}
					if (array[0] == "vec3")
					{
						try
						{
							string[] array2 = array[2].Split(new char[]
							{
								','
							});
							if (array2.Length != 3)
							{
								Console.WriteLine(string.Concat(new string[]
								{
									"Invalid number of values on line ",
									text,
									" in file ",
									fileName,
									"."
								}));
							}
							else
							{
								array2[0].Trim();
								array2[1].Trim();
								array2[2].Trim();
								dictionary4.Add(array[1], new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2])));
							}
							continue;
						}
						catch (Exception ex3)
						{
							Console.WriteLine(ex3.Message);
							Console.WriteLine("On " + array[2]);
							dictionary4.Add(array[1], default(Vector3));
							continue;
						}
					}
					Console.WriteLine(string.Concat(new string[]
					{
						"Data type ",
						array[0],
						" not found in file ",
						fileName,
						"."
					}));
				}
			}
		}
		ConfigHelper.ConfigFile configFile = new ConfigHelper.ConfigFile(dictionary, dictionary2, dictionary3, dictionary4);
		ConfigHelper.configs.Add(fileName, configFile);
		return configFile;
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x000B05C0 File Offset: 0x000AE7C0
	public static string GetString(string configName, string valName)
	{
		string result;
		try
		{
			result = ConfigHelper.GetFile(configName).GetString(valName);
		}
		catch
		{
			Console.WriteLine("(str)Unable to get config value (or could be bad assignment)" + valName.ToString() + " from " + configName.ToString());
			result = null;
		}
		return result;
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x000B0614 File Offset: 0x000AE814
	public static int GetInt(string configName, string valName)
	{
		int result;
		try
		{
			result = ConfigHelper.GetFile(configName).GetInt(valName);
		}
		catch
		{
			Console.WriteLine("(int)Unable to get config value (or could be bad assignment)" + valName.ToString() + " from " + configName.ToString());
			result = 0;
		}
		return result;
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x000B0668 File Offset: 0x000AE868
	public static float GetFloat(string configName, string valName)
	{
		float result;
		try
		{
			result = ConfigHelper.GetFile(configName).GetFloat(valName);
		}
		catch
		{
			Console.WriteLine("(float)Unable to get config value (or could be bad assignment) " + valName.ToString() + " from " + configName.ToString());
			result = 0f;
		}
		return result;
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x0001F0B1 File Offset: 0x0001D2B1
	public static Vector3 GetVec3(string configName, string valName)
	{
		return ConfigHelper.GetFile(configName).GetVec3(valName);
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x0001F0BF File Offset: 0x0001D2BF
	public static void Reset()
	{
		ConfigHelper.configs = new Dictionary<string, ConfigHelper.ConfigFile>();
	}

	// Token: 0x04002126 RID: 8486
	private static Dictionary<string, ConfigHelper.ConfigFile> configs = new Dictionary<string, ConfigHelper.ConfigFile>();

	// Token: 0x02000915 RID: 2325
	public class ConfigFile
	{
		// Token: 0x06002CDC RID: 11484 RVA: 0x0001F0CB File Offset: 0x0001D2CB
		public ConfigFile()
		{
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000B06C0 File Offset: 0x000AE8C0
		public ConfigFile(Dictionary<string, string> sStrings, Dictionary<string, int> sInts, Dictionary<string, float> sFloats, Dictionary<string, Vector3> sVecs)
		{
			this.sStrings = sStrings;
			this.sInts = sInts;
			this.sFloats = sFloats;
			this.sVecs = sVecs;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x0001F0FF File Offset: 0x0001D2FF
		public string GetString(string valName)
		{
			return this.sStrings[valName];
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x0001F10D File Offset: 0x0001D30D
		public int GetInt(string valName)
		{
			return this.sInts[valName];
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x0001F11B File Offset: 0x0001D31B
		public float GetFloat(string valName)
		{
			return this.sFloats[valName];
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x0001F129 File Offset: 0x0001D329
		public Vector3 GetVec3(string valName)
		{
			return this.sVecs[valName];
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x0001F137 File Offset: 0x0001D337
		public void AddExternalValue(string valName, string value)
		{
			if (this.sStrings.ContainsKey(valName))
			{
				return;
			}
			this.sStrings.Add(valName, value);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x0001F155 File Offset: 0x0001D355
		public void AddExternalValue(string valName, int value)
		{
			if (this.sInts.ContainsKey(valName))
			{
				return;
			}
			this.sInts.Add(valName, value);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x0001F173 File Offset: 0x0001D373
		public void AddExternalValue(string valName, float value)
		{
			if (this.sFloats.ContainsKey(valName))
			{
				return;
			}
			this.sFloats.Add(valName, value);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x0001F191 File Offset: 0x0001D391
		public void AddExternalValue(string valName, Vector3 value)
		{
			if (this.sVecs.ContainsKey(valName))
			{
				return;
			}
			this.sVecs.Add(valName, value);
		}

		// Token: 0x04002127 RID: 8487
		private Dictionary<string, string> sStrings = new Dictionary<string, string>();

		// Token: 0x04002128 RID: 8488
		private Dictionary<string, int> sInts = new Dictionary<string, int>();

		// Token: 0x04002129 RID: 8489
		private Dictionary<string, float> sFloats = new Dictionary<string, float>();

		// Token: 0x0400212A RID: 8490
		private Dictionary<string, Vector3> sVecs = new Dictionary<string, Vector3>();
	}
}
