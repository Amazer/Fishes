using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
public class BuildAssetBundle
{
    [MenuItem("assetBundle/BuildSelectedAb_SelectOne")]
    public static void BuildAb()
    {
        Object o = Selection.activeObject;
        string outPath = Application.dataPath + "/../../FishesAssetbundles/";
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        string[] depenStr = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(o));

        AssetBundleBuild[] builds = new AssetBundleBuild[depenStr.Length];
        for (int i = 0, imax = depenStr.Length; i < imax; ++i)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(depenStr[i]);
            builds[i].assetBundleName = ABTool.FixedName(asset) + ".ab";
            builds[i].assetNames = new string[] { depenStr[i] };
        }
        BuildPipeline.BuildAssetBundles(outPath, builds,
            BuildAssetBundleOptions.StrictMode
            | BuildAssetBundleOptions.DeterministicAssetBundle
            //            | BuildAssetBundleOptions.ForceRebuildAssetBundle
            , BuildTarget.StandaloneWindows64);
        Debug.Log("打包完成");
    }
    [MenuItem("assetBundle/BuildSelectedAb_SelectMulity")]
    public static void BuildAbMulty()
    {
        string outPath = Application.dataPath + "/../../FishesAssetbundles/";
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        List<string> builtList = new List<string>();
        List<AssetBundleBuild> buildsList = new List<AssetBundleBuild>();
        Object[] objects = Selection.objects;
        for (int i = 0, imax = objects.Length; i < imax; ++i)
        {
            Object o = objects[i];
            string[] depenStr = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(o));

            for (int k = 0, kmax = depenStr.Length; k < kmax; ++k)
            {
                if(builtList.Contains(depenStr[k]))
                {
                    continue;
                }
                builtList.Add(depenStr[k]);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(depenStr[k]);
                AssetBundleBuild abb = new AssetBundleBuild(); 
                abb.assetBundleName = ABTool.FixedName(asset) + ".ab";
                abb.assetNames = new string[] { depenStr[k] };
                buildsList.Add(abb);
            }
        }
        BuildPipeline.BuildAssetBundles(outPath, buildsList.ToArray(),
            BuildAssetBundleOptions.StrictMode
            | BuildAssetBundleOptions.DeterministicAssetBundle
            //            | BuildAssetBundleOptions.ForceRebuildAssetBundle
            , BuildTarget.StandaloneWindows64);
        Debug.Log("打包完成");
    }
    [MenuItem("assetBundle/BuildSelectedAbAllInOne_SelectOne")]
    public static void BuildAbAllInOne()
    {
        Object o = Selection.activeObject;
        string outPath = Application.dataPath + "/../../FishesAssetbundles/";
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        AssetBundleBuild[] builds = new AssetBundleBuild[1];
        string assetPath = AssetDatabase.GetAssetPath(o);
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(o));
        builds[0].assetBundleName = asset.name + ".ab";
        builds[0].assetNames = new string[] { assetPath };
        BuildPipeline.BuildAssetBundles(outPath, builds,
            BuildAssetBundleOptions.StrictMode
            | BuildAssetBundleOptions.DeterministicAssetBundle
            //            | BuildAssetBundleOptions.ForceRebuildAssetBundle
            , BuildTarget.StandaloneWindows64);
        Debug.Log("打包完成");
    }
}
