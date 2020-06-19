using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
public class FileItem
{
    public int id;
    public string fileRelPath;
    public int state;
}
/// <summary>
/// 同步分支工具
/// </summary>
public class SynSyncResEditor : EditorWindow
{
    public static string TargetRootPath = @"";
    private static string tarAssetsPath = "";
    private static string localAssetsPath;

    private static string tarProjectPath;
    private static string localProjectPath;

    private static string exportPath = "";
    private static string exprotProjectPath = "";

    // 记录目标路径保存
    private const string save_tar_path_key = "sync_branches_tar_path";
    // 记录导出文件的文件夹
    private const string save_export_path_key ="save_export_path";

    private string infoTxt = "";

    private List<FileItem> filesList;
    private List<FileItem> removeList;
    private List<FileItem> exportFilesList;

    private int functionType = 0;
    private string[] funcStrs = { "同步当前版本", "同步选中版本" };

    Vector2 scrollPostion = Vector2.zero;

    private void Awake()
    {
        filesList = new List<FileItem>();
        removeList = new List<FileItem>();
        exportFilesList = new List<FileItem>();
    }
    [MenuItem("Tools/同步分支")]
    public static void Init()
    {
        localAssetsPath = Application.dataPath;
        localProjectPath = GetProjectPath(localAssetsPath);
        tarAssetsPath = PlayerPrefs.GetString(save_tar_path_key);
        if(tarAssetsPath==string.Empty)
        {
            tarAssetsPath = "";
        }
        else
        {
            tarProjectPath = GetProjectPath(tarAssetsPath);
        }
        exportPath = PlayerPrefs.GetString(save_export_path_key);
        if(exportPath==string.Empty)
        {
            exportPath = "";
        }
        SynSyncResEditor win = EditorWindow.GetWindow<SynSyncResEditor>();
        win.titleContent = new GUIContent("同步资源");
        win.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();



        EditorGUILayout.EndVertical();
        
    }

    private void DrawSrcPath()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("当前路径", GUILayout.Width(100));
        EditorGUI.BeginDisabledGroup(true);
        string assetsDir = Application.dataPath;
        EditorGUILayout.TextField(assetsDir);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
    }

    private void DrawTarPath()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("目标路径", GUILayout.Width(100));
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(tarAssetsPath);
        EditorGUI.EndDisabledGroup();
        if(GUILayout.Button("打开位置",GUILayout.Width(100)))
        {
            string path = EditorUtility.OpenFolderPanel("选择分支Assets文件夹", "", "");
            if(!string.IsNullOrEmpty(path))
            {
//                if(GetAssetsPath(ref path))
//                {
//
//                }
            }
        }
        GUILayout.EndHorizontal();

    }

    private static string GetProjectPath(string path )
    {
        int index = path.IndexOf("Assets");
        if(index<=0)
        {
            string tmpPath = path + "/Assets/";
            if(Directory.Exists(tmpPath))
            {
                return path;
            }
            return "";
        }
        path = path.Substring(0, index);
        return path;
    }

//    private 
}
