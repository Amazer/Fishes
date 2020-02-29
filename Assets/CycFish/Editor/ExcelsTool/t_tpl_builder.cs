using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using System.Data;
using System.IO;
using Excel;
public class t_tpl_builder : Editor
{
    [MenuItem("Excel/创建表格CS")]
    public static void CreateExcelCS()
    {
        FileTool.DelAll(t_tpl_config.dataCSPath);
        string[] files = FileTool.GetAllFilePath(t_tpl_config.excelPath);
        foreach (var v in files)
        {
            FileAttributes attr = File.GetAttributes(v);
            int res = ((int)attr & (int)FileAttributes.Hidden);
            if (res == 0) // 没有 hidden标志，可以读取
            {
                DataTable dataTable = ReadExcel(v);
                CreateTableCS(Path.GetFileNameWithoutExtension(v), dataTable);
                AssetDatabase.Refresh();
                //                build_table_assets(Path.GetFileNameWithoutExtension(v));
            }
        }
    }
    [MenuItem("Excel/创建表格Assets")]
    public static void CreateExcelAssets()
    {
        FileTool.DelAll(t_tpl_config.dataAssetPath);
        string[] files = FileTool.GetAllFilePath(t_tpl_config.excelPath);
        foreach (var v in files)
        {
            FileAttributes attr = File.GetAttributes(v);
            int res = ((int)attr & (int)FileAttributes.Hidden);
            if (res == 0) // 没有 hidden标志，可以读取
            {
                //                DataTable dataTable = ReadExcel(v);
                //                CreateTableCS(Path.GetFileNameWithoutExtension(v), dataTable);
                //                AssetDatabase.Refresh();
                build_table_assets(Path.GetFileNameWithoutExtension(v));
            }
        }
    }
    [MenuItem("Excel/Test")]
    public static void Test()
    {

        string class_name = "test";
        Assembly csAss = Assembly.Load("Assembly-CSharp");
        Type t = csAss.GetType("t_tpl_" + class_name);
        Debug.Log("t is null:" + (t == null));
        object o = Activator.CreateInstance(t);
    }
    public static DataTable ReadExcel(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        string tmpFile = Application.dataPath + "/../excel_tmp"+extension;// "C:/Users/eric/Downloads/excel_tmp.xlsx";
        if(File.Exists(tmpFile))
        {
            File.Delete(tmpFile);
        }
        File.Copy(filePath, tmpFile);

        FileStream stream = File.Open(tmpFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = null;
        if(extension == ".xlsx")
        {
        // 读取后缀.xlsx
         excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        }
        else if (extension == ".xls")
        {
        // 读取后缀 .xls
            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        }
        DataSet result = excelReader.AsDataSet();
        stream.Close();
        excelReader.Close();
        File.Delete(tmpFile);
        //Tables[0] 下标0表示excel文件中第一张表的数据
        //        columnNum = result.Tables[0].Columns.Count;
        //        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0];
    }
    private static void CreateTableCS(string tableName, DataTable result)
    {
        //        int columnNum = result.Tables[0].Columns.Count;
        //        int rowNum = result.Tables[0].Rows.Count;
        // 第一行是中文注释
        // 第二行是数据类型
        // 第三行是变量名称
        // 从第四行开始是数据
        t_tpl_creator.create_tpl_cs(tableName, result);
        t_tpl_creator.create_tpl_mgr_cs(tableName);
    }
    public static void build_table_assets(string tableName)
    {

        Assembly csAss = Assembly.Load("Assembly-CSharp");
        string class_name = string.Format("t_tpl_{0}", tableName.ToLower());
        Type t = csAss.GetType(class_name);
        ScriptableObject assetData = ScriptableObject.CreateInstance(class_name);

        string item_class_name = string.Format("t_tpl_{0}_item", tableName.ToLower());
        Type item_t = csAss.GetType(item_class_name);
        Type createType = typeof(t_tpl_creator);
        MethodInfo method = createType.GetMethod("create_item_array_frome_excel", BindingFlags.Static | BindingFlags.Public);
        method = method.MakeGenericMethod(item_t);
        object[] para = new object[1];
        string tablePath = t_tpl_config.excelPath + tableName + ".xlsx";
        if(!File.Exists(tablePath))
        {
            tablePath = t_tpl_config.excelPath + tableName + ".xls";
        }
        DataTable dataTable = ReadExcel(tablePath);
        para[0] = dataTable;
//        mgr.dataArray = (t_tpl_test_item[])method.Invoke(null, para);
        object dataArray = method.Invoke(null, para);

        FieldInfo field = t.GetField("dataArray", BindingFlags.Instance | BindingFlags.Public);
        field.SetValue(assetData, dataArray);
        string dataAssetPath = string.Format("{0}{1}.asset", t_tpl_config.dataAssetPath, tableName);
        //        //确保文件夹存在
        if (!Directory.Exists(t_tpl_config.dataAssetPath))
        {
            Directory.CreateDirectory(t_tpl_config.dataAssetPath);
        }
        //生成一个Asset文件
        AssetDatabase.CreateAsset(assetData, dataAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(tableName + "  dataAsset 创建完成！");
    }
}
