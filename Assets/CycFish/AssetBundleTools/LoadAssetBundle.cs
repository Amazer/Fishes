using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LoadAssetBundle : MonoBehaviour
{
    public string downloadUrl = "http://192.168.1.169:8888/";
    [System.NonSerialized]
    public string assetFolder;
    public string assetBundleName;
    [System.NonSerialized]
    public string abManifestName = "FishesAssetbundles";
    public string extension = ".ab";
    public AssetBundleManifest manifest;
    public Dictionary<string, AssetBundle> abList = new Dictionary<string, AssetBundle>();
    [ContextMenu("GC")]
    public void GC()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
    private void Awake()
    {
        assetFolder = Application.dataPath + "/../../FishesAssetbundles/";
    }
    IEnumerator Start()
    {
        yield return StartCoroutine(LoadManifest());
        yield return StartCoroutine(LoadOneAssetBundle(assetBundleName));
        AssetBundle test = abList[assetBundleName];
        Object res = test.LoadAsset(assetBundleName);
        test.Unload(false);
        GameObject go = GameObject.Instantiate(res) as GameObject;
        yield return StartCoroutine(LoadOneAssetBundle("TestTextureRefGO"));
        AssetBundle testRef = abList["TestTextureRefGO"];
        Object resRef = testRef.LoadAsset("TestTextureRefGO");
//        testRef.Unload(false);
        GameObject goRef = GameObject.Instantiate(resRef) as GameObject;
    }

    IEnumerator LoadManifest()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetFolder + abManifestName);
        yield return request;
        if (request.assetBundle != null)
        {
            manifest = request.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            request.assetBundle.Unload(false);
            Debug.Log("manifest load ok:" + (manifest != null));
        }
        else
        {
            Debug.LogError("LoadManifest failed!");
        }
        request = null;
    }
    IEnumerator LoadOneAssetBundle(string name)
    {
        string[] depens = manifest.GetAllDependencies(name + extension);
        for (int i = 0, imax = depens.Length; i < imax; ++i)
        {
            Debug.Log("depens:" + depens[i]);
            if(abList.ContainsKey(depens[i]))
            {
                continue;
            }
            yield return StartCoroutine(LoadDepensBundle(depens[i]));
        }
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetFolder + name + extension);
        yield return request;
        abList.Add(name, request.assetBundle);
        Debug.Log("load AB Over " + name);

    }
    IEnumerator LoadDepensBundle(string name)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetFolder + name);
        yield return request;
        Debug.Log("load depen AB Over " + name);
        abList.Add(ABTool.NameWithoutExtension(name), request.assetBundle);
    }
    private void OnApplicationQuit()
    {
        if(manifest!=null)
        {
            DestroyImmediate(manifest);
        }
        manifest = null;
    }
}
