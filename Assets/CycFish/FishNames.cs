using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishItem
{
    public string name;
    public float forwardY =0f;
}
public class FishNames : SinClass<FishNames>
{
    public List<string> list;
    public override void Init()
    {
        base.Init();
        list = new List<string>();
        list.Add("amberjack");
        list.Add("amur");
        list.Add("arapaima");
        list.Add("black_spooted_grunt");
        list.Add("emperor_anglefish");
        list.Add("lionfish");
        list.Add("orangespine_unicornfish");
        list.Add("starfish_v1");
        list.Add("yellow_boxfish");
        list.Add("archerfish");
    }
}
