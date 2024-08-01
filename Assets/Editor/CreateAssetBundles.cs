using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Create Asset Bundles/Create Scene Bundles")]
    private static void CreateSceneBundles()
    {
        BuildAssetBundles("Scene");
    }

    [MenuItem("Assets/Create Asset Bundles/Create Prefab Bundles")]
    private static void CreatePrefabBundles()
    {
        BuildAssetBundles("Prefab");
    }

    [MenuItem("Assets/Create Asset Bundles/Create Audio Bundles")]
    private static void CreateAudioBundles()
    {
        BuildAssetBundles("Audio");
    }

    [MenuItem("Assets/Create Asset Bundles/Create Texture Bundles")]
    private static void CreateTextureBundles()
    {
        BuildAssetBundles("Texture");
    }

    [MenuItem("Assets/Create Asset Bundles/Create Other Bundles")]
    private static void CreateOtherBundles()
    {
        BuildAssetBundles("Other");
    }

    private static void BuildAssetBundles(string subfolderName)
    {
        var assetBundleDirectoryPath = Path.Combine(Application.dataPath, "..", "AssetBundle", subfolderName);

        try
        {
            // Create the directory if it doesn't exist
            if (!Directory.Exists(assetBundleDirectoryPath))
            {
                Debug.Log($"AssetBundle subfolder '{subfolderName}' doesn't exist! \n ** Creating New Subfolder **");
                Directory.CreateDirectory(assetBundleDirectoryPath);
                AssetDatabase.Refresh(); // Refresh the AssetDatabase to recognize the new directory
            }
            else
            {
                Debug.Log($"AssetBundle subfolder '{subfolderName}' exists");
            }

            // Build asset bundles
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            Debug.Log("Asset bundles built successfully and stored in subfolder: " + subfolderName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to build asset bundles: " + e.Message);
        }
    }
}
