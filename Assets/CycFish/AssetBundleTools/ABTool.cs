using UnityEngine;
using System.Collections;
public static class ABTool 
{
    public static string FixedName(Object o)
    {
        if(o is Material)
        {
            return o.name + "_mat";
        }
        else if(o is Texture)
        {
            return o.name + "_tex";
        }
        return o.name;
    }
    public static string NameWithoutExtension(string name)
    {
        return System.IO.Path.GetFileName(name);
    }
}
