using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class MoveFlag
{
    private Action _callback;
    [SerializeField]
    private Vector3 _dest;
    [SerializeField]
    private Transform _dynamicDest;
    private Transform _tr;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Vector3 _dir;
    [SerializeField]
    private bool _moveToTarget = false;
    [SerializeField]
    private bool _moveToDynamicTarget = false;
    [SerializeField]
    private bool _moving = false;
    public MoveFlag(Transform tr)
    {
        _tr = tr;
    }
    private Vector3 _nowPos
    {
        get
        {
            return _tr.position;
        }
        set
        {
            _tr.position = value;
        }
    }
    public bool movingToTarget
    {
        get
        {
            return _moveToTarget;
        }
    }
    public bool moving
    {
        get
        {
            return _moving;
        }
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void MoveToTarget(Vector3 dest, Action callback)
    {
        _moveToTarget = true;
        _moveToDynamicTarget = false;
        _moving = false;
        _dest = dest;
        _callback = callback;
    }
    public void MoveToDynamicTarget(Transform tr, Action callback)
    {
        _moveToTarget = false;
        _moveToDynamicTarget = true;
        _moving = false;
        _dynamicDest = tr;
        _callback = callback;
    }
    /// <summary>
    /// 如果移动位置超出鱼缸，会callback
    /// </summary>
    /// <param name="moving"></param>
    /// <param name="dir"></param>
    /// <param name="cb"></param>
    public void Move(bool moving, Vector3 dir, Action cb = null)
    {
        _moving = moving;
        _dir = dir;
        _callback = cb;
        if (_moving)
        {
            _moveToTarget = false;
            _moveToDynamicTarget = false;
        }

    }
    public void Update(float deltaTime)
    {
        if (_moveToTarget)
        {
            _MoveToTarget(deltaTime);
        }
        if (_moving)
        {
            Vector3 pos = _nowPos + deltaTime * _dir * _speed;
            if (!Tank.instance.InTank(pos))
            {
                _moving = false;
                _DoCallBack();
            }
            else
            {
                _nowPos = pos;
            }
        }
        if (_moveToDynamicTarget)
        {
            _MoveToDynamicTarget(deltaTime);
        }

    }
    private void _MoveToTarget(float deltaTime)
    {
        if (Mathf.Abs(_speed) < 1e-5)
        {
            _moveToTarget = false;
            return;
        }
        float time = Vector3.Distance(_nowPos, _dest) / _speed;
        if (time > deltaTime) // 需要的时间大于一个deltaTime
        {
            _nowPos = Vector3.Lerp(_nowPos, _dest, deltaTime / time);
        }
        else
        {
            _nowPos = _dest;
            _moveToTarget = false;
            _DoCallBack();
        }
    }
    private void _MoveToDynamicTarget(float deltaTime)
    {
        Vector3 dir = _dynamicDest.position - _nowPos;
        float time = Vector3.Distance(_nowPos, _dynamicDest.position) / _speed;
        if (time > deltaTime) // 需要的时间大于一个deltaTime
        {
            _nowPos = Vector3.Lerp(_nowPos, _dynamicDest.position, deltaTime / time);
        }
        else
        {
//            _nowPos = _dynamicDest.position;
            _moveToDynamicTarget = false;
            _dynamicDest = null;
            _DoCallBack();
        }

    }
    private void _DoCallBack()
    {
        Debug.Log("move over");
        if (_callback != null)
        {
            Action cb = _callback;
            _callback = null;
            cb();
        }
    }
}
