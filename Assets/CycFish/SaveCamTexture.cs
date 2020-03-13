using UnityEngine;
using System;
using System.IO;
public class SaveCamTexture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public void Start()
    {
        if(cam==null)
        {
            cam = this.GetComponent<Camera>();
        }
    }
    private void Update()
    {
        if(cam==null)
        { return; }
        if(Input.GetKeyDown(KeyCode.F4))
        {
            _SaveCamTexture();
        }
    }
    private void _SaveCamTexture()
    {
        rt = cam.targetTexture;
        if(rt!=null)
        {
            _SaveRenderTexture(rt);
            rt = null;
        }
        else
        {
            GameObject camGo = new GameObject("camGO");
            Camera tmpCam = camGo.AddComponent<Camera>();
            tmpCam.CopyFrom(cam);
//            rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rt =RenderTexture.GetTemporary(Screen.width,Screen.height,16, RenderTextureFormat.ARGB32);
            tmpCam.targetTexture = rt;
            tmpCam.Render();
            _SaveRenderTexture(rt);
            Destroy(camGo);
//            rt.Release();
            RenderTexture.ReleaseTemporary(rt);
            rt = null;
//            Destroy(rt);
        }

    }
    private void _SaveRenderTexture(RenderTexture rt)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        png.Apply();
        RenderTexture.active = active;
        byte[] bytes = png.EncodeToPNG();
        string path = string.Format("Assets/../rt_{0}_{1}_{2}.png", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        FileStream fs = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fs);
        writer.Write(bytes);
        writer.Flush();
        writer.Close();
        fs.Close();
        Destroy(png);
        png = null;
        Debug.Log("保存成功！"+path);
    }
}

