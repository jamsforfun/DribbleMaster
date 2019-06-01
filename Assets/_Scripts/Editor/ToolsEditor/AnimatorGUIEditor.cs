using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(AnimatorGUI))]
public class AnimatorGUIEditor : OdinEditor
{
    AnimatorGUI animatorGUI;

    private void OnSceneGUI()
    {
        animatorGUI = (AnimatorGUI)target;

        Handles.color = Color.green;
        if (Handles.Button(animatorGUI.transform.position + Vector3.up * 1 + Vector3.right * 0.3f, Quaternion.identity, 0.2f, 0.2f, Handles.SphereHandleCap))
        {
            ExtReflexion.SetPlayButton();
        }
    }
}
