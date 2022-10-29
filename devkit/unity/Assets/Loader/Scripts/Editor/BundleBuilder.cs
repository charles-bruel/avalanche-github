using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class BundleBuilder : Editor
{
    [MenuItem("Assets/ Build Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Out", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildAssetBundles("Assets/Out/Win", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildAssetBundles("Assets/Out/Mac", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
    }
}
