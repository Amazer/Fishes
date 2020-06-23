using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum GameEvent
{
    Feed,
    FoodDestroy,
}

public delegate void EventDelegate(object[] param);
//public class EventMgr: SinClass<EventMgr>  
//{
//    private Dictionary<int, EventDelegate> eventDict;
//    public override void Init()
//    {
//        base.Init();
//        eventDict = new Dictionary<int, EventDelegate>();
//    }
//    public override void UnInit()
//    {
//        base.UnInit();
//    }
//
//    public void TriggerEvent(int eventName,params object[] param)
//    {
//        if(eventDict.ContainsKey(eventName))
//        {
//            eventDict[eventName](param);
//        }
//    }
//    public void AddListener(int eventName,EventDelegate cb)
//    {
//        if(eventDict.ContainsKey(eventName))
//        {
//            eventDict[eventName] += cb;
//        }
//        else
//        {
//            eventDict[eventName] = cb;
//        }
//
//    }
//    public void RemListener(int eventName, EventDelegate cb)
//    {
//        if(eventDict.ContainsKey(eventName))
//        {
//            eventDict[eventName] -= cb;
//        }
//
//    }
//}
public class EventMgr<T> : SinClass<EventMgr<T>>  where T:struct,IFormattable, IConvertible, IComparable
{
    private Dictionary<T, Action<object[]>> eventDict;
    public override void Init()
    {
        base.Init();
        eventDict = new Dictionary<T, Action<object[]>>();
    }
    public override void UnInit()
    {
        base.UnInit();
    }

    public void TriggerEvent(T eventName,params object[] param)
    {
        if(eventDict.ContainsKey(eventName))
        {
            eventDict[eventName](param);
        }
    }
    public void AddListener(T eventName,Action<object[]> cb)
    {
        if(eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] += cb;
        }
        else
        {
            eventDict[eventName] = cb;
        }

    }
    public void RemListener(T eventName, Action<object[]> cb)
    {
        if(eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] -= cb;
        }

    }
}
