using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.IO;
using System.Data;

public class FishItemTool
{ /// <summary>
  /// 读取表数据，生成对应的数组
  /// </summary>
  /// <param name="filePath">excel文件全路径</param>
  /// <returns>Item数组</returns>
    public static Table_FishItem[] CreateItemArrayWithExcel(string filePath)
    {
        //获得表数据
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

        //根据excel的定义，第二行开始才是数据
        Table_FishItem[] array = new Table_FishItem[rowNum - 1];
        for (int i = 1; i < rowNum; i++)
        {
            Table_FishItem item = new Table_FishItem();
            //解析每列的数据
            if (string.IsNullOrEmpty(collect[i][0].ToString()))
            {
                continue;
            }
            item.id = int.Parse(collect[i][0].ToString());
            item.name = collect[i][1].ToString();
            item.y = int.Parse(collect[i][2].ToString());
            array[i - 1] = item;
        }
        return array;
    }

    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        string tmpFile = Application.dataPath + "/../excel_tmp.xlsx";// "C:/Users/eric/Downloads/excel_tmp.xlsx";
        File.Copy(filePath, tmpFile);

        FileStream stream = File.Open(tmpFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        // 读取后缀.xlsx
//        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        // 读取后缀 .xls
        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        DataSet result = excelReader.AsDataSet();
        stream.Close();
        excelReader.Close();
        File.Delete(tmpFile);

        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }


}
