using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimFlag
{
    private Transform _tr;
    private Animator _anim;

    [SerializeField]
    private float _speed;

    public AnimFlag(Transform tr)
    {
        _tr = tr;
        _anim = _tr.GetComponentInChildren<Animator>();
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;

    }
    public void Update(float deltaTime)
    {
        if (!_anim)
        {
            return;
        }
        _UpdateAnimSpeed();

    }

    private void _UpdateAnimSpeed()
    {
        if (_speed > 1.5f)
        {
            _speed = 1.5f;
        }
        else if (_speed < 0.2f)
        {
            _speed = 0.2f;
        }
        _anim.speed = _speed;
    }
}
