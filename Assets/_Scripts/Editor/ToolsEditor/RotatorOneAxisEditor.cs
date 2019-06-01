using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotatorOneAxis))]
public class RotatorOneAxisEditor : OdinEditor
{
    RotatorOneAxis rotatorOneAxis;
    int discRotateControlId;

    private new void OnEnable()
    {
        rotatorOneAxis = (RotatorOneAxis)target;
    }

    void OnSceneGUI()
    {
        if (discRotateControlId == -1)
        {
            discRotateControlId = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
        }
        rotatorOneAxis.objToRotate.rotation = DiscRotatorHandle.Do(discRotateControlId, rotatorOneAxis.objToRotate.position, rotatorOneAxis.objToRotate.rotation, 4f, 0f, rotatorOneAxis.objToRotate);
    }
}
