using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class SinMono<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static bool _destroyed;
    public static T instance
    {
        get
        {
            return SinMono<T>.GetInstance();
        }
    }

    public static T GetInstance()
    {
        if(SinMono<T>._instance == null && !SinMono<T>._destroyed)
        {
            Type typeFromeHandle = typeof(T);
            SinMono<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeFromeHandle));
            if(SinMono<T>._instance == null)
            {
//                object[] customAttributes = typeFromeHandle.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
//                if(customAttributes.Length >0 && !((AutoSingletonAttribute)customAttributes[0]).bAutoCreate)
//                {
//                    return (T)((object)null);
//                }
                GameObject gameObject = new GameObject(typeof(T).Name);
                SinMono<T>._instance = gameObject.AddComponent<T>();
                
            }
        }
        return SinMono<T>._instance;
    }

    public static void DestroyInstance()
    {
        if(SinMono<T>._instance != null)
        {
            UnityEngine.Object.Destroy(SinMono<T>._instance.gameObject);
        }
        SinMono<T>._destroyed = true;
        SinMono<T>._instance = (T)((object)null);
    }

	protected virtual void Awake()
	{
		if (SinMono<T>._instance != null && SinMono<T>._instance.gameObject != base.gameObject)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				Object.DestroyImmediate(base.gameObject);
			}
		}
		else if (SinMono<T>._instance == null)
		{
			SinMono<T>._instance = base.GetComponent<T>();
		}
		Object.DontDestroyOnLoad(base.gameObject);
		this.Init();
	}

	protected virtual void OnDestroy()
	{
		if (SinMono<T>._instance != null && SinMono<T>._instance.gameObject == base.gameObject)
		{
			SinMono<T>._instance = (T)((object)null);
		}
	}

	public static bool HasInstance()
	{
		return SinMono<T>._instance != null;
	}

	protected virtual void Init()
	{
	}
}
