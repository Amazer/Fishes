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
using System.Collections;
using System;
public  static class Logger
{
    public static void Info(object message,params object[] argv)
    {
        Debug.Log(LoggerStringFormat(message, argv));
    }
    public static void Log(object message,params object[] argv)
    {
        Debug.Log(LoggerStringFormat(message, argv));
    }
    public static void Error(object message,params object[] argv)
    {
        Debug.LogError(LoggerStringFormat(message, argv));
    }
    public static void Warning(object message,params object[] argv)
    {
        Debug.LogWarning(LoggerStringFormat(message, argv));
    }

    private static string LoggerStringFormat(object message,params object[] argv)
    {
        string logMsg = "logMsg";
        try
        {
            if(argv==null||argv.Length ==0)
            {
                logMsg = message.ToString();
            }
            else
            {
                logMsg = string.Format(message.ToString(), argv);
            }
        }
        catch(Exception e)
        {
            logMsg = e.Message;
        }
        return logMsg;
    }
}
