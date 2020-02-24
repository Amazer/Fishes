using System;

public class SinClass<T> where T : class,new()
{
    private static T s_instance;
    public static T instance
    {
        get
        {
            if(SinClass<T>.s_instance == null)
            {
                SinClass<T>.CreateInstacne();

            }
            return SinClass<T>.s_instance;
        }
    }

    protected SinClass()
    {

    }
    public static void CreateInstacne()
    {
        if(SinClass<T>.s_instance == null)
        {
            SinClass<T>.s_instance = Activator.CreateInstance<T>();
            (SinClass<T>.s_instance as SinClass<T>).Init();
        }
    }
    public static void DestroyInstance()
    {
        if(SinClass<T>.s_instance != null)
        {
            (SinClass<T>.s_instance as SinClass<T>).UnInit();
            SinClass<T>.s_instance = (T)((object)null);
        }
    }

    public static T GetInstance()
    {
        if(SinClass<T>.s_instance == null)
        {
            SinClass<T>.CreateInstacne();
        }
        return SinClass<T>.s_instance;
    }

    public static bool HasInstance()
    {
        return SinClass<T>.s_instance != null;
    }

    public virtual void Init()
    {
    }
    public  virtual void UnInit()
    {

    }

}
