using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FeedFlag
{
    private Transform _tr;
    private bool _feeding = false;
    private FoodAI _tarFood;
    private Action<bool> _callback;
    private float _feedDuration;
    public bool isHungry = true;
    public float hungryTime=0f;
    public float feedDuration
    {
        get
        {
            return _feedDuration;
        }
        set
        {
            _feedDuration = value;
        }
    }


    public bool feeding
    {
        get
        {
            return _feeding;
        }
    }

    public FeedFlag(Transform trans)
    {
        _tr = trans;
    }
    public void StartFeed(FoodAI tarFood, Action<bool> callback)
    {
        _feeding = true;
        _tarFood = tarFood;
        _callback = callback;
    }
    public void Update(float deltaTime)
    {
        if (_feeding)
        {
            _feed(deltaTime);
        }
        if(!isHungry)
        {
            if(Time.timeSinceLevelLoad>hungryTime)
            {
                isHungry = true;
            }
        }
    }
    private void _feed(float deltaTime)
    {
        if (_tarFood == null || !_tarFood.active)
        {
            _feeding = false;
            _tarFood = null;
            _DoCallBack(false);
            return;
        }
        float dis = Vector3.Distance(_tr.position, _tarFood.transform.position);
        if(dis<3) // 吃到食物
        {
            _tarFood.DesFood();
            _feeding = false;
            _tarFood = null;
            isHungry = false;
            hungryTime = Time.timeSinceLevelLoad + _feedDuration;
            _DoCallBack(true);
        }
    }
    private void _DoCallBack(bool getFood)
    {
        if (_callback != null)
        {
            Action<bool> cb = _callback;
            _callback = null;
            cb(getFood);
        }
    }
}
