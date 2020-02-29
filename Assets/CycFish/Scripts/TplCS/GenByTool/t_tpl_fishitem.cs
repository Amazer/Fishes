/**********************************
工具生成，不要改动
***********************************/

using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class t_tpl_fishitem:ScriptableObject
{
    public t_tpl_fishitem_item[] dataArray;
    public static Dictionary<int, t_tpl_fishitem_item> dic;
    private static bool loaded = false;
    public void Awake()
    {
        Debug.Log("t_tpl_fishitem awake!");
        loaded = true;
        dic = new Dictionary<int, t_tpl_fishitem_item>();
        for(int i=0,imax = dataArray.Length;i<imax;++i)
        {
            t_tpl_fishitem_item item = dataArray[i];
            dic.Add(item.id, item);
        }
    }

    public static t_tpl_fishitem_item FindById(int id)
    {
        if(!loaded)
        {
            Debug.LogError("error not loaded!");
        }
        t_tpl_fishitem_item item = null;
        if(dic.TryGetValue(id,out item))
        {
            return item;
        }
        return null;
    }
    
}
