using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileTool
{
    public static string ReadFile(string path)
    {

        FileStream fs = File.Open(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string content= sr.ReadToEnd();
        sr.Close();
        fs.Close();
        return content;
    }
    public static bool WriteFile(string path,string content)
    {

        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
        sw.Flush();
        sw.Close();
        fs.Close();
        return true;
    }
}
