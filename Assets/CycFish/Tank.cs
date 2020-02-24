using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank :SinMono<Tank>
{
    public Vector2 width;
    public Vector2 height;
    public Vector2 depth;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
        width.x = this.transform.position.x - this.transform.localScale.x * 0.5f;
        width.y = this.transform.position.x + this.transform.localScale.x * 0.5f;
        height.x = this.transform.position.y - this.transform.localScale.y * 0.5f;
        height.y = this.transform.position.y + this.transform.localScale.y * 0.5f;
        depth.x = this.transform.position.z - this.transform.localScale.z * 0.5f;
        depth.y = this.transform.position.z + this.transform.localScale.z * 0.5f;
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
}
