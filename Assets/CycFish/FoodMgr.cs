using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMgr : SinMono<FoodMgr>
{
    public GameObject foodGo;
    public float downSpeed = 5f;
    public List<FoodAI> foodList;
    protected override void Init()
    {
        base.Init();
        foodList = new List<FoodAI>();
    }
    public void GenFood(Vector3 pos)
    {
        GameObject go = GameObject.Instantiate(foodGo);
        go.transform.position = pos;
        foodList.Add(go.GetComponent<FoodAI>());
        EventMgr<GameEvent>.instance.TriggerEvent(GameEvent.Feed);

    }
    public void DesFood(FoodAI ai)
    {
        foodList.Remove(ai);
        EventMgr<GameEvent>.instance.TriggerEvent(GameEvent.FoodDestroy,ai);
    }
    public void Nearest(Vector3 pos, out FoodAI ai)
    {
        float dis = 9999f;
        ai = null;
        for (int i = 0, imax = foodList.Count; i < imax; ++i)
        {
            FoodAI f = foodList[i];
            if (f != null && f.active)
            {
                float d = Vector3.Distance(pos, f.transform.position);
                if(d<dis)
                {
                    ai = f;
                }
            }
        }
    }
}
