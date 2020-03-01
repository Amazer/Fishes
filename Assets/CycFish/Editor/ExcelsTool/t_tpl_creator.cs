using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.IO;
using System.Data;
using UnityEngine;
using System.Collections;
using System.Text;
using Excel;
using System.Data;
using System.Reflection;
using System;
/// <summary>
/// 生成表格数据的cs文件
/// 格式为：class_name = tableName.toLower
/// t_tpl_class_name_item: 数据item
/// t_tpl_class_name: 数据items集合
/// </summary>
public class t_tpl_creator
{
    public static bool create_tpl_item_cs(string tableName,DataTable dataTable)
    {
        string class_name =string.Format("t_tpl_{0}_item", tableName.ToLower());
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("/**********************************");
        sb.AppendLine("         工具生成，不要改动");
        sb.AppendLine("***********************************/");
        sb.AppendLine("");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("");
        sb.AppendLine("[System.Serializable]");
        sb.AppendLine("public class "+class_name+":t_tpl_data");
        sb.AppendLine("{");
        sb.Append(create_variates(dataTable));
        sb.AppendLine("}");

        if(!Directory.Exists(t_tpl_config.dataCSPath))
        {
            Directory.CreateDirectory(t_tpl_config.dataCSPath);
        }
        FileStream fs = File.Open(t_tpl_config.dataCSPath + class_name+".cs", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(sb);
        sw.Flush();
        sw.Close();
        fs.Close();
        Debug.Log(string.Format("{0} 生成完毕!", class_name));
        return true;
    }
    private static string create_variates(DataTable dataTable)
    {
        DataRowCollection collect = dataTable.Rows;
        StringBuilder sb = new StringBuilder();
        int columnNum = dataTable.Columns.Count;
        for (int i = 0; i < columnNum; ++i)
        {
            string typeStr = collect[1][i].ToString();
            string varNameStr = collect[2][i].ToString();
            sb.AppendLine("\tpublic " + get_type(typeStr) + " " + varNameStr + ";");
        }
        return sb.ToString();
    }
    private static string get_type(string typeStr)
    {
        if (typeStr.Contains("array"))
        {
            int subIndex = typeStr.LastIndexOf("_");
            string subStr = typeStr.Substring(subIndex + 1);
            return "List<" + subStr + ">";
        }
        else
        {
            return typeStr;
        }
    }
    public static bool create_tpl_cs(string tableName)
    {
        string class_name = string.Format("t_tpl_{0}", tableName.ToLower());
        string class_item_name = string.Format("t_tpl_{0}_item", tableName.ToLower());
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine("/**********************************");
//        sb.AppendLine("        工具生成，不要改动");
//        sb.AppendLine("***********************************/");
//        sb.AppendLine("");
//        sb.AppendLine("using System;");
//        sb.AppendLine("using System.Collections.Generic;");
//        sb.AppendLine("using UnityEngine;");
//        sb.AppendLine("");
//        sb.AppendLine("[System.Serializable]");
//        sb.AppendLine("public class " + class_name + ":ScriptableObject");
//        sb.AppendLine("{");
//        sb.AppendLine("\tpublic " + class_item_name + "[] dataArray;");
//        sb.AppendLine("\tpublic static Dictionary<int," + class_item_name + "> dic;");
//        sb.AppendLine("\tprivate static bool loaded = false;");
//        sb.AppendLine("\tpublic void Awake()");
//        sb.AppendLine("\t{");
//        sb.AppendLine("\t\tloaded = true;");
//        sb.AppendLine("\t\tdic = new Dictionary<int," + class_item_name + ">();");
//        sb.AppendLine("\t\tfor(int i=0,imax=dataArray.Length;i<imax;++i)");
//        sb.AppendLine("\t\t\t"+class_item_name+" item = dataArray[i];");
//        sb.AppendLine("\t\t\t dic.Add(item.id,item);");
//        sb.AppendLine("}");
//
        if(!Directory.Exists(t_tpl_config.dataCSPath))
        {
            Directory.CreateDirectory(t_tpl_config.dataCSPath);
        }
        FileStream templateFs = File.Open(t_tpl_config.templatePath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(templateFs);
        string csText = reader.ReadToEnd();
        reader.Close();
        templateFs.Close();

        csText = csText.Replace("#class_name#", class_name);
        csText = csText.Replace("#class_item_name#", class_item_name);

        FileStream fs = File.Open(t_tpl_config.dataCSPath + class_name+".cs", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(csText);
        sw.Flush();
        sw.Close();
        fs.Close();
        Debug.Log(string.Format("{0} 生成完毕!", class_name));
        return true;
    }

    public static T[] create_item_array_frome_excel<T>(DataTable dataTable)
    {
        List<T> list = new List<T>();
        int rowNum = dataTable.Rows.Count;
        for (int i = 3; i < rowNum; ++i)
        {
            DataRow rowData = dataTable.Rows[i];
            T o = Activator.CreateInstance<T>();
            Type t = o.GetType();
            FieldInfo[] fields = t.GetFields();
            for (int k = 0, kmax = fields.Length; k < kmax; ++k)
            {
//                Debug.Log("fild name:" + fields[k].Name);
//                Debug.Log("rowData:" + rowData[k]);
                FieldInfo f = fields[k];
                fields[k].SetValue(o, get_by_type(f.FieldType.ToString(),rowData[k]));
            }
            list.Add(o);
        }
        return list.ToArray();
    }
    public static object get_by_type(string typestr, object valO)
    {
        if (typestr == typeof(Int32).ToString())
        {
            return int.Parse(valO.ToString());
        }
        else if (typestr == typeof(float).ToString())
        {
            return float.Parse(valO.ToString());
        }
        else if(typestr == typeof(List<int>).ToString())
        {
            string valStr = valO.ToString();
            string[] itemStr = valStr.Split(',');
            List<int> list = new List<int>(itemStr.Length);
            for(int i=0,imax=itemStr.Length;i<imax;++i)
            {
                list.Add(int.Parse(itemStr[i]));
            }
            return list;
        }
        else if(typestr == typeof(List<float>).ToString())
        {
            string valStr = valO.ToString();
            string[] itemStr = valStr.Split(',');
            List<float> list = new List<float>(itemStr.Length);
            for(int i=0,imax=itemStr.Length;i<imax;++i)
            {
                list.Add(float.Parse(itemStr[i]));
            }
            return list;
        }
        else if(typestr == typeof(List<string>).ToString())
        {

            string valStr = valO.ToString();
            string[] itemStr = valStr.Split(',');
            List<string> list = new List<string>(itemStr);
            return list;
        }
        return valO;
    }
}
