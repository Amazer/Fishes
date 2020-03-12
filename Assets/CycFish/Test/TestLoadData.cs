using UnityEngine;
using System.Collections;
public class TestLoadData : MonoBehaviour
{
    public void Start()
    {
        Resources.Load(t_tpl_config.assetsResourcePath + "FishItem");
        Debug.Log(t_tpl_fishitem.FindById(1001));
    }
}
