using UnityEngine;
using UnityEditor;

/// <summary>
/// scene view calculation
/// </summary>
public class SceneViewCameraFunction : ScriptableObject
{
    /// <summary>
    /// focus on object and zoom
    /// </summary>
    /// <param name="objToFocus"></param>
    /// <param name="zoom"></param>
    public static void FocusOnSelection(GameObject objToFocus, float zoom = 5f)
    {
        SceneView.lastActiveSceneView.LookAt(objToFocus.transform.position);
        if (zoom != -1)
        {
            SceneViewCameraFunction.ViewportPanZoomIn(zoom);
        }
    }

    /// <summary>
    /// Set the zoom of the camera
    /// </summary>
    public static void ViewportPanZoomIn(float zoom = 5f)
    {
        if (SceneView.lastActiveSceneView.size > zoom)
        {
            SceneView.lastActiveSceneView.size = zoom;
            //SceneView.lastActiveSceneView.pivot = ;
        }
        
        SceneView.lastActiveSceneView.Repaint();
    }
}