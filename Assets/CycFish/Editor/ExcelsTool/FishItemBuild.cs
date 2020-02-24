using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;

public class FishItemBuild : Editor
{

    [MenuItem("Table/CreateFishItem")]
    public static void CreateItemAsset()
    {
        Table_FishItemTool manager = ScriptableObject.CreateInstance<Table_FishItemTool>();
        //赋值
        manager.dataArray = FishItemTool.CreateItemArrayWithExcel(TableConfig.excelsFolderPath + "FishItem.xlsx");

        //确保文件夹存在
        if (!Directory.Exists(TableConfig.assetPath))
        {
            Directory.CreateDirectory(TableConfig.assetPath);
        }

        //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
        string assetPath = string.Format("{0}{1}.asset", TableConfig.assetPath, "FishItem");
        //生成一个Asset文件
        AssetDatabase.CreateAsset(manager, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Fish Item 创建完成");

    }
}
