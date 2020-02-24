using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Table_FishItemTool : ScriptableObject
{
    static Table_FishItemTool _instance=null;
    public static Table_FishItemTool instance
    {
        get
        {
            if(_instance ==null)
            {
                _instance = Resources.Load<Table_FishItemTool>(TableConfig.assetResourcePath+"FishItem");
            }
            return _instance;
        }
    }
    public Table_FishItem[] dataArray;
}
