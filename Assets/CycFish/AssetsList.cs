using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsList : MonoBehaviour {
    public List<GameObject> list;
    public bool Add(GameObject go)
    {
        if(list ==null)
        {
            list = new List<GameObject>();
        }
        if(list.Contains(go))
        {
            return false;
        }
        list.Add(go);
        return true;
    }

    public bool Clear()
    {
        if(list == null)
        {
            list = new List<GameObject>();
        }
        list.Clear();
        return true;
    }
}
