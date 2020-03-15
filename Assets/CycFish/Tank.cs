using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : SinMono<Tank>
{
    public float width;
    public float height;
    public float depth;

    private float _minWidthPos;
    private float _maxWidthPos;
    private float _minHeightPos;
    private float _maxHeightPos;
    private float _minDepthPos;
    private float _maxDepthPos;

    private Transform _tr;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
        _tr = this.transform;
        _minWidthPos = _tr.position.x - width*0.5f;
        _maxWidthPos = _tr.position.x + width*0.5f;
        _minHeightPos = _tr.position.y - height*0.5f;
        _maxHeightPos = _tr.position.y + height*0.5f;
        _minDepthPos = _tr.position.z - depth*0.5f;
        _maxDepthPos = _tr.position.z + depth*0.5f;
    }
    public float minWidthPos
    {
        get
        {
            return _minWidthPos;
        }
    }
    public float maxWidthPos
    {
        get
        {
            return _maxWidthPos;
        }
    }
    public float minHeightPos
    {
        get
        {
            return _minHeightPos;
        }
    }
    public float maxHeightPos
    {
        get
        {
            return _maxHeightPos;
        }
    }
    public float minDepthPos
    {
        get
        {
            return _minDepthPos;
        }
    }
    public float maxDepthPos
    {
        get
        {
            return _maxDepthPos;
        }
    }
    public float RandomX()
    {
        return Random.Range(_minWidthPos, _maxWidthPos);

    }
    public float RandomY()
    {
        return Random.Range(_minHeightPos, _maxHeightPos);

    }
    public float RandomZ()
    {
        return Random.Range(_minDepthPos, _maxDepthPos);

    }
    public bool InTank(Vector3 pos)
    {
        if (pos.x < _minWidthPos || pos.x > _maxWidthPos)
        {
            return false;
        }
        if (pos.y < _minHeightPos || pos.y > _maxHeightPos)
        {
            return false;
        }
        if (pos.z < _minDepthPos || pos.z > _maxDepthPos)
        {
            return false;
        }
        return true;
    }
    public Vector3 InTankPos(Vector3 pos)
    {
        if (pos.x < _minWidthPos)
        {
            pos.x = _minWidthPos;
        }
        else if (pos.x > _maxWidthPos)
        {
            pos.x = _maxWidthPos;
        }

        if (pos.y < _minHeightPos)
        {
            pos.y = _minHeightPos;
        }
        else if (pos.y > _maxHeightPos) 
        {
            pos.y = _maxHeightPos;
        }
        if (pos.z < _minDepthPos)
        {
            pos.z = _minDepthPos;
        }
        else if (pos.z > _maxDepthPos)
        {
            pos.z = _maxDepthPos;
        }
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (this.enabled)
        {
            if (_tr == null)
            {
                _tr = this.transform;
            }
            Color c = Gizmos.color;
            Gizmos.color = Color.green;
            Matrix4x4 m = Gizmos.matrix;
            Gizmos.matrix = _tr.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(width,height,depth));
            Gizmos.matrix = m;
            Gizmos.color = c;

        }
        
    }
}
