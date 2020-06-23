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
using System.Collections.Generic;
using System;
public class EventAction
{
    private Dictionary<int, EventDelegate> evtDic;
    private bool addToMgr = false;
    public EventAction()
    {
        evtDic = new Dictionary<int, EventDelegate>();
    }
    public void AddEventListener<T>(T evtKey, EventDelegate cb) where T : struct,IFormattable, IConvertible, IComparable
    {
        int key = Convert.ToInt32(evtKey);
        if (!evtDic.ContainsKey(key))
        {
            evtDic[key] = cb;
            Logger.Info("注册事件：{0},{1}", evtKey, cb.Method.Name);
        }
        else
        {
            Delegate dele = evtDic[key];
            Delegate[] deles = dele.GetInvocationList();
            bool registed = false;
            for (int i = 0, imax = deles.Length; i < imax; ++i)
            {
                if (deles[i].Equals(cb))
                {
                    registed = true;
                    break;

                }
            }
            if (registed)
            {

                Logger.Error("已经注册过此事件了：{0},{1}", evtKey, cb.Method.Name);
            }
            else
            {
                Delegate.Combine(dele, cb);
                Logger.Info("注册事件：{0},{1}", evtKey, cb.Method.Name);
            }
        }
    }

    public void AddToEventMgr()
    {
        if (!addToMgr)
        {
            addToMgr = true;
            foreach (var v in evtDic)
            {
//                EventMgr.instance.AddListener(v.Key, v.Value);
            }
        }
        else
        {
            Logger.Error("已经添加到EventMgr了");
        }

    }
    public void RemoveFromEventMgr()
    {
        if (addToMgr)
        {
            addToMgr = false;
            foreach (var v in evtDic)
            {
//                EventMgr.instance.RemListener(v.Key, v.Value);
            }
        }
        else
        {
            Logger.Error("已经从EventMgr移除了");
        }
    }
}
public class AutoEvent 
{
    private Dictionary<Type, EventAction> evtDic;
    public AutoEvent()
    {
        evtDic = new Dictionary<Type, EventAction>();
    }

    public void AddEventListener<T>(T evtKey, EventDelegate cb) where T : struct,IFormattable, IConvertible, IComparable
    {
        Type t = evtKey.GetType();
        if (!evtDic.ContainsKey(t))
        {
            evtDic[t] = new EventAction();
        }
        EventAction act = evtDic[t];
        act.AddEventListener(evtKey, cb);
    }
    public void AddEventToMgr()
    {
        foreach (var v in evtDic.Values)
        {
            v.AddToEventMgr();
        }
    }
    public void RemoveFromMgr()
    {
        foreach (var v in evtDic.Values)
        {
            v.AddToEventMgr();
        }
    }
}
