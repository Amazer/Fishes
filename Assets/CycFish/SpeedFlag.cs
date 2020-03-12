using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class SpeedFlag 
{
    private Transform _tr;
    [SerializeField]
    private float _curSpeed;
    [SerializeField]
    private float _curTime;
    [SerializeField]
    private float _totalTime; // 总时长
    [SerializeField]
    private float _accTime; // 加速运动时间
    [SerializeField]
    private float _decTime; // 减速运动时间
    [SerializeField]
    private float _avgTime; // 匀速运动时间
    [SerializeField]
    private bool _varSpeeding = false;
    private Action _callback;


    // 加速阶段占的总时长
    [SerializeField]
    private float _speedUpProgress = 0.3f;
    // 减速阶段占的总时长
    [SerializeField]
    private float _speedDownProgress = 0.3f;

    // 加速度
    [SerializeField]
    private float _accSpeed = 0;
    // 减速度
    [SerializeField]
    private float _decSpeed = 0;

    [SerializeField]
    private float _maxSpeed = 0;
    [SerializeField]
    private float _minSpeed = 0f;

    public bool varSpeeding
    {
        get
        {
            return _varSpeeding;
        }
    }

    public float curSpeed
    {
        get
        {
            return _curSpeed;
        }
    }
    public SpeedFlag(Transform tr)
    {
        _tr = tr;
    }
    public void SetVarSpeedConfig(float speedUpProgress,float speedDownProfress)
    {
        _speedUpProgress = speedUpProgress;
        _speedDownProgress = speedDownProfress;
    }
    public void SetVarMinSpeed(float minSpeed)
    {
        _minSpeed = minSpeed;
    }
    public void StartVarSpeed(float distance, float totalTime,Action callback)
    {
        _varSpeeding = true;
        _callback = callback;
        _totalTime = totalTime;
        _accTime = totalTime * _speedUpProgress;
        _decTime = totalTime * _speedDownProgress;
        _avgTime = _totalTime - _accTime - _decTime;
        _maxSpeed = (2f * distance-_curSpeed*_accTime-_minSpeed*_decTime) / (2f * totalTime - _accTime-_decTime);
        _accSpeed = (_maxSpeed - _curSpeed) / _accTime;
        _decSpeed = (_maxSpeed - _minSpeed) / _decTime;

    }
    public void Update(float deltaTime)
    {
        if(_varSpeeding)
        {
            _varSpeed(deltaTime);
        }

    }
    private void _varSpeed(float deltaTime)
    {
        _curTime = _curTime + deltaTime; 
        if(_accTime>0)
        {
            if(_accTime>deltaTime)
            {
                _curSpeed += deltaTime * _accSpeed;
                _accTime -= deltaTime;
            }
            else 
            {
                _curSpeed += _accTime * _accSpeed;
                deltaTime -= _accTime;
                _accTime = 0;
                _varSpeed(deltaTime);
            }
        }
        else if(_avgTime>0)
        {
            if(_avgTime>deltaTime)
            {
                _avgTime -= deltaTime;
            }
            else 
            {
                deltaTime -= _avgTime;
                _avgTime = 0;
                _varSpeed(deltaTime);
            }
        }
        else if(_decTime>0)
        {
            if(_decTime>deltaTime)
            {
                _curSpeed -= deltaTime * _decSpeed;
                _decTime -= deltaTime;
            }
            else 
            {
                _curSpeed -= _decTime * _decSpeed;
                deltaTime -= _decTime;
                _decTime = 0;
                _varSpeeding = false;
                _DoCallback();
            }
        }
    }
    private void _DoCallback()
    {
        if(_callback!=null)
        {
            Action cb = _callback;
            _callback = null;
            cb();
        }
    }
}
