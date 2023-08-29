using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundleCreator
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        string assetBundleDirectoryPath = "Assets/StreamingAssets";
        try
        {
            if (!System.IO.Directory.Exists(assetBundleDirectoryPath))
                System.IO.Directory.CreateDirectory(assetBundleDirectoryPath);
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + ex.Message);
            throw ex;
        }
    }

}
