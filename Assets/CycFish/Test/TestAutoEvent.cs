/*
********************************************** 
 *Copyright(C) 2018 by #COMPANY# 
 *
 *模块名:           #SCRIPTFULLNAME# 
 *创建者:           #AUTHOR# 
 *创建日期:         #DATE# 
 *修改者列表:
 *描述:    
**********************************************
*/
using UnityEngine;
using System.Collections.Generic;
using System;
public enum TestEvent
{
    event1,
    event2
}
public enum TestEvent2
{
    event1,
    event2
}
public class TestAutoEvent : MonoBehaviour
{
    public AutoEvent autoEvt;
    private void Awake()
    {
        autoEvt = new AutoEvent();
        autoEvt.AddEventListener<TestEvent>(TestEvent.event1, OnEvent1);
        autoEvt.AddEventListener<TestEvent>(TestEvent.event2, OnEvent2);
        autoEvt.AddEventListener<TestEvent2>(TestEvent2.event1, OnEvent1_1);
        autoEvt.AddEventListener<TestEvent2>(TestEvent2.event2, OnEvent1_2);
    }
    private void Start()
    {
    }


    private void OnEnable()
    {
        autoEvt.AddEventToMgr();
    }
    private void OnDisable()
    {
        autoEvt.RemoveFromMgr();
    }
    private void OnEvent1(object[] param)
    {
        Logger.Info("OnEvent 1 called");
    }
    private void OnEvent2(object[] param)
    {
        Logger.Info("OnEvent 2 called");
    }
    private void OnEvent1_1(object[] param)
    {
        Logger.Info("OnEvent 1 called");
    }
    private void OnEvent1_2(object[] param)
    {
        Logger.Info("OnEvent 2 called");
    }

}
