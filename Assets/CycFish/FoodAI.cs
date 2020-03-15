using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAI : MonoBehaviour
{
    private Transform tr;
    public float downSpeed = 10;
    public bool active = true;
    private void Awake()
    {
        tr = this.transform;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }
        downSpeed = FoodMgr.instance.downSpeed;
        float posY = tr.position.y - downSpeed * Time.deltaTime;
        tr.position = new Vector3(tr.position.x, posY, tr.position.z);
        if (posY < Tank.instance.minHeightPos)
        {
            active = false;
            FoodMgr.instance.DesFood(this);
            Destroy(this.gameObject);
        }
    }
    public void DesFood()
    {
        active = false;
        FoodMgr.instance.DesFood(this);
        Destroy(this.gameObject);
    }
}
