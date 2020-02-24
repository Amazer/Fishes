using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

/// <summary>
/// 鱼模型规范化处理
/// </summary>
public class FishModelNormalizeEditor : Editor
{
    public const string modelFolder = "Assets/Underwater_animal_pack/Assets_models/animals/";
    public const string animControllerFolder = "Assets/CycFish/AnimControllers/";
    public const string prefabFolder = "Assets/CycFish/Prefabs/fishes/";

    [MenuItem("Tool/生成所有预制件")]
    public static void CreateAllFish()
    {
        foreach (var v in Table_FishItemTool.instance.dataArray)
        {
            CreatePrefab(v.name,v.y);
        }
        Debug.Log("生成完毕");
    }

    [MenuItem("Tool/SetModel")]

    public static void SetModel()
    {
        Object o = Selection.activeObject;
        ConfigModel(o.name);
    }
    public static void ConfigModel(string modelName)
    {
        string path = modelFolder + modelName + ".FBX";// AssetDatabase.GetAssetPath(o);
        string metaPath = path + ".meta";
        FileStream fs = File.Open(metaPath, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string str = sr.ReadLine();
        bool needChange = false;
        while (str != null)
        {
            if (str.Contains("animationType:"))
            {
                str = str.Replace("1", "2");

            }
            if (str.Contains("name:"))
            {
                if (str.Contains("swim") || str.Contains("fastswim"))
                {
                    needChange = true;
                }
            }
            if (needChange)
            {
                if (str.Contains("loopTime:"))
                {
                    str = str.Replace("0", "1");
                    needChange = false;
                }
            }
            sb.AppendLine(str);
            str = sr.ReadLine();
        }
        sr.Close();
        fs.Close();
        FileTool.WriteFile(metaPath, sb.ToString());
        AssetDatabase.Refresh();
        Debug.Log("Normalized model:" + path);
    }

    [MenuItem("Tool/SetAnimatorController")]
    public static void ConfigAnimatorcontroller()
    {
        Object o = Selection.activeObject;
        CreateAnimatorcontroller(o.name);

    }
    public static void CreateAnimatorcontroller(string modelName)
    {
        string path = animControllerFolder + modelName + ".controller";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        AnimatorController.CreateAnimatorControllerAtPath(path);
        AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        AnimatorStateMachine machine = ac.layers[0].stateMachine;

        string modelPath = modelFolder + modelName + ".FBX";
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        if (model.GetComponent<Animator>() == null)
        {
            ConfigModel(modelName);
        }

        Object[] clips = AssetDatabase.LoadAllAssetsAtPath(modelPath);
        int clipNum = 0;
        foreach (var v in clips)
        {
            if (v.GetType() == typeof(AnimationClip) && v.name != "__preview__Take 001")
            {
                AnimatorState state = ac.AddMotion(v as Motion, 0);
                clipNum++;
                if (v.name == "swim")
                {
                    machine.defaultState = state;
                }

            }
        }
        if (clipNum == 0)
        {
            Debug.LogError(modelName + "没有动画clip!!");

        }

        Debug.Log("create animatorController:" + path);
    }

    [MenuItem("Tool/SetFishPrefab")]
    public static void CreateSelectionPrefab()
    {

        string modelName = Selection.activeObject.name;
        CreatePrefab(modelName);
    }
    public static void CreatePrefab(string modelName, int y = 0)
    {
        string controllerPath = animControllerFolder + modelName + ".controller";
        if (!File.Exists(controllerPath))
        {
            CreateAnimatorcontroller(modelName);
        }

        GameObject root = new GameObject(modelName);
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelFolder + modelName + ".FBX");
        model = GameObject.Instantiate(model);
        model.transform.parent = root.transform;
        model.transform.localScale = Vector3.one;
        model.transform.localEulerAngles = new Vector3(0, y, 0);// Vector3.zero;
        model.transform.localPosition = Vector3.zero;

        SkinnedMeshRenderer smr = model.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr == null)
        {

            Debug.Log("smr is null");
        }
        else
        {
            smr.receiveShadows = false;
            smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Material m = smr.sharedMaterial;
            m.shader = Shader.Find("Mobile/Bumped Diffuse");
        }


        DynamicBone db = model.AddComponent<DynamicBone>();
        db.m_Root = model.transform.FindChild("Bone01/Bone02/Bone03");
        db.m_Damping = 0.2f;
        db.m_Elasticity = 0.02f;
        db.m_Stiffness = 0.01f;
        db.m_FreezeAxis = DynamicBone.FreezeAxis.None;

        Animator anim = model.GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.Normal;
        anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        anim.applyRootMotion = true;



        RuntimeAnimatorController animCtr = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(controllerPath);
        anim.runtimeAnimatorController = animCtr;


        string path = prefabFolder + modelName + ".prefab";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        root.AddComponent<CFishAI>();
        PrefabUtility.CreatePrefab(path, root);
        DestroyImmediate(root);
        Debug.Log("create prefab:" + path);
    }
}
