using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class BundleBuilder : Editor
{
    [MenuItem("Assets/ Build Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Bundles/Mac", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneOSX);
        BuildPipeline.BuildAssetBundles("Assets/Bundles", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildAssetBundles("Assets/Bundles/Linux", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneLinux);
    }
}
