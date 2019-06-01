using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(FOVVisualize))]
public class FOVVisualizeEditor : OdinEditor
{
    FOVVisualize fOVVisualize;
    ArcHandle arcHandle;

    private new void OnEnable()
    {
        fOVVisualize = (FOVVisualize)target;
        arcHandle = new ArcHandle();
        arcHandle.SetColorWithRadiusHandle(Color.red, 0.2f);
    }

    void OnSceneGUI()
    {
        Matrix4x4 handleMatrix = Matrix4x4.TRS(fOVVisualize.transform.position, fOVVisualize.transform.rotation, Vector3.one);
        arcHandle.angle = fOVVisualize.angle;
        arcHandle.radius = fOVVisualize.radius;

        Handles.color = fOVVisualize.colorFov;
        using (new Handles.DrawingScope(handleMatrix))
        {
            Handles.DrawLine(Vector3.zero, Vector3.up * arcHandle.radius);
            arcHandle.DrawHandle();
        }
        fOVVisualize.radius = arcHandle.radius;
        fOVVisualize.angle = arcHandle.angle;
    }
}
