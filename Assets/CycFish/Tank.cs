using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank :SinMono<Tank>
{
    public Vector2 width;
    public Vector2 height;
    public Vector2 depth;
    public Transform tankCube;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
        width.x = -this.tankCube.localScale.x*0.5f;
        width.y = this.tankCube.localScale.x*0.5f;
        height.x = -this.tankCube.localScale.y * 0.5f;
        height.y = this.tankCube.localScale.y * 0.5f;;
        depth.x = -this.tankCube.localScale.z * 0.5f;
        depth.y = this.tankCube.localScale.z * 0.5f;
    }
    public float RandomX()
    {
        return Random.Range(width.x, width.y);

    }
    public float RandomY()
    {
        return Random.Range(height.x, height.y);

    }
    public float RandomZ()
    {
        return Random.Range(depth.x, depth.y);

    }
    public bool InTank(Vector3 pos)
    {
        if(pos.x<width.x||pos.x>width.y)
        {
            return false;
        }
        if(pos.y<height.x||pos.y>height.y)
        {
            return false;
        }
        if(pos.z<depth.x||pos.z>depth.y)
        {
            return false;
        }
        return true;
    }
    public Vector3 InTankPos(Vector3 pos)
    {
        if(pos.x<width.x)
        {
            pos.x = width.x;
        }
        else if(pos.x>width.y)
        {
            pos.x = width.y;
        }

        if(pos.y<height.x)
        {
            pos.y = height.x;
        }
        else if(pos.y>height.y)
        {
            pos.y = height.y;
        }
        if(pos.z<depth.x)
        {
            pos.z = depth.x;
        }
        else if(pos.z>depth.y)
        {
            pos.z = depth.y;
        }
        return pos;
    }
}
