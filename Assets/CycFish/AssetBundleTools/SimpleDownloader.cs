using UnityEngine;
using System.Collections;
using System.IO;
public class SimpleDownloader : MonoBehaviour 
{
    public string abPath;
    public void Start () 
    {
        if(!Directory.Exists(ABConfig.localABRoot))
        {
            Directory.CreateDirectory(ABConfig.localABRoot);
        }
        StartCoroutine(StartDownload(abPath));
    }
    IEnumerator StartDownload(string path)
    {
        WWW w = new WWW(ABConfig.remoteUrlRoot+path);
        yield return w;
        if(w.error!=null)
        {
            Debug.LogError(w.error);
        }
        else
        {

            DownloadOK(w.bytes,path);
        }
        w.Dispose();
    }
    private void DownloadOK(byte[] bytes,string path)
    {
        FileStream fs = new FileStream(ABConfig.localABRoot+path,FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();
        fs.Close();
        Debug.Log("Write ab over:" + path);
    }
}
