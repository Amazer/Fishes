using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SinMono<CameraMgr>
{
    public Camera cam;
    protected override void Init()
    {
        cam = this.GetComponent<Camera>();
        base.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(r, out hitInfo, 100f))
            {
                CFishAI ai = hitInfo.transform.GetComponent<CFishAI>();
                ai.Escape();
            }
            else
            {
                // 喂食
                float randomZ = Tank.instance.RandomZ() + Tank.instance.transform.position.z - cam.transform.position.z;
                Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,randomZ));
                FoodMgr.instance.GenFood(worldPos);

            }
        }
    }
}
