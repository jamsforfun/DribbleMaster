using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HighResScreenShots : MonoBehaviour
{
    [FoldoutGroup("GamePlay")]
    public int ratioX = 216;
    [FoldoutGroup("GamePlay")]
    public int ratioY = 384;
    [FoldoutGroup("GamePlay")]
    public string pathImageFromAsset = "ScreenShots/";

    public string GetPath()
    {
        return string.Format("{0}/Resources/{1}", Application.dataPath, pathImageFromAsset);
    }

    [Button]
    public string TakeHiResShot(Camera cam)
    {
        return (TakeHiResShot(cam, ratioX, ratioY));
    }

    public string TakeHiResShot(Camera cam, int resWidth, int resHeight)
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        DestroyImmediate(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string path = GetPath();
        string filename = ExtFile.ScreenShotName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(path + filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", (path + filename) ));

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        //SceneView.RepaintAll();
        return (filename);
    }
}