using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class BundleHelper
{
	// Token: 0x06002CE8 RID: 11496 RVA: 0x000B071C File Offset: 0x000AE91C
	public static AssetBundle LoadIfNotLoaded(string name)
	{
		name = name.Replace('\\', '/');
		if (BundleHelper.bundles.ContainsKey(name))
		{
			return BundleHelper.bundles[name];
		}
		Console.WriteLine("Loading bundle " + name + " for the first time.");
		AssetBundle assetBundle = AssetBundle.LoadFromFile(name);
		BundleHelper.bundles[name] = assetBundle;
		return assetBundle;
	}

	// Token: 0x0400212B RID: 8491
	public static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

	// Token: 0x0400212C RID: 8492
	public static string AssetBundlePath = "ModData/Bundles/";
}
