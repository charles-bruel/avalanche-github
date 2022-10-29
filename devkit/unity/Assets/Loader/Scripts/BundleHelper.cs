using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleHelper
{
    public static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

    public static string AssetBundlePath = "Assets/Out";

    public static AssetBundle LoadIfNotLoaded(string name)
    {
        if (bundles.ContainsKey(name))
        {
            return bundles[name];
        }
        else
        {
            AssetBundle temp = AssetBundle.LoadFromFile(AssetBundlePath + name);
            bundles[name] = temp;
            return temp;
        }
    }
}
