﻿using UnityEngine;
using System.Collections;
[System.Serializable]
public class RotateFlag
{
    private Transform _tr;
    private bool _rotating = false;
    private bool _rotatToTarget = false;

    private float _rotaTime;
    private float _rotaSpeed;
    [SerializeField]
    private Vector3 _tarDir;
    [SerializeField]
    private Quaternion _tarQuat;
    [SerializeField]
    private Transform _tarTrans;

    private float _time;

    public Vector3 curDir
    {
        get
        {
            return _tr.localEulerAngles;
        }
        private set
        {

            _tr.localEulerAngles = value;
        }
    }
    public RotateFlag(Transform tr)
    {
        _tr = tr;
    }
    public void SetRotate(float rotaTime, float rotaSpeed, Vector3 tarDir)
    {
        _rotating = true;
        _rotatToTarget = false;
        _time = 0;
        _rotaTime = rotaTime;
        _rotaSpeed = rotaSpeed;
        _tarDir = tarDir;
        _tarTrans = null;
        _tarQuat = Quaternion.LookRotation(tarDir);
    }
    public void SetRotateToTarget(float rotaTime, float rotaSpeed, Transform tarTran)
    {
        _rotating = false;
        _rotatToTarget = true;
        _time = 0;
        _rotaTime = rotaTime;
        _rotaSpeed = rotaSpeed;
        _tarTrans = tarTran;
        _tarQuat = Quaternion.LookRotation(_tarTrans.position-_tr.position);

    }
    public void Update(float deltaTime)
    {
        if (_rotating)
        {
            _rotate(deltaTime);
        }
        else if(_rotatToTarget)
        {
            _rotateToTarget(deltaTime);
        }

    }
    private void _rotate(float deltaTime)
    {
        if(Mathf.Abs(_rotaSpeed)<1e-5)
        {
            _rotating = false;
            return;
        }
        if(_tr.localRotation != _tarQuat)
        {
            _tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, _rotaSpeed * deltaTime);
        }
        else
        {
            _rotating = false;
        }
    }


    private void _rotateToTarget(float deltaTime)
    {
        _tarQuat = Quaternion.LookRotation(_tarTrans.position-_tr.position);
        if(_tr.localRotation != _tarQuat)
        {
            _tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, _rotaSpeed * deltaTime);
        }
    }

}
