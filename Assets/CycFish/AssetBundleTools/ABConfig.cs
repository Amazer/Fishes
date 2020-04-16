/*
********************************************** 
**********************************************
AssetBundle相关的配置
*/
using UnityEngine;
using System.Collections;
public class ABConfig 
{
    public static readonly string remoteUrlRoot = "http://192.168.1.169:8888/";
    private static readonly string localABFolder = "assetbundles/";
    private static string _localAbRoot;

    public static string localABRoot
    {
        get
        {
            if(_localAbRoot == null )
            {
                _localAbRoot = string.Format("{0}/{1}", Application.dataPath, localABFolder);
            }
            return _localAbRoot;
        }
    }
}
