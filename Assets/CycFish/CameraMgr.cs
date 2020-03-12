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
            Debug.Log("mouse 0");
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(r, out hitInfo, 100f))
            {
                CFishAI ai = hitInfo.transform.GetComponent<CFishAI>();
                //                ai.Escape();
                ai.Escape();
            }
        }
        
    }
}
