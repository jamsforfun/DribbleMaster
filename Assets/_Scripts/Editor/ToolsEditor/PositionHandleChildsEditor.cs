using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PositionHandleChilds))]
public class PositionHandleChildsEditor : OdinEditor
{
    private PositionHandleChilds _positionHandle;
    private Tool LastTool = Tool.None;
    private ExtUtilityEditor.HitSceneView hitScene;

    private new void OnEnable()
    {
        _positionHandle = (PositionHandleChilds)target;
        LastTool = Tools.current;
        Tools.current = Tool.None;
    }
    
    private void LoopThoughtMove()
    {
        if (_positionHandle._allChildToMove == null)
            return;

        for (int i = 0; i < _positionHandle._allChildToMove.Count; i++)
        {
            Transform child = _positionHandle._allChildToMove[i];
            if (!child)
                continue;

            if (child.gameObject.activeInHierarchy)
            {
                Undo.RecordObject(child.gameObject.transform, "handle camPoint move");

                if (_positionHandle.LocalSpace)
                {
                    child.position = Handles.PositionHandle(child.position, child.rotation);
                }
                else
                {
                    child.position = Handles.PositionHandle(child.position, Quaternion.identity);
                }
            }
        }
    }

    private void LoopThoughtRotate()
    {
        if (_positionHandle._allChildToRotate == null)
            return;

        for (int i = 0; i < _positionHandle._allChildToRotate.Count; i++)
        {
            Transform child = _positionHandle._allChildToRotate[i];
            if (!child)
                continue;

            if (child.gameObject.activeInHierarchy)
            {
                Undo.RecordObject(child.gameObject.transform, "handle camPoint rotation");
                child.rotation = Handles.RotationHandle(child.rotation, child.position);
            }
        }
    }

    private void HandleChild()
    {
        if (Event.current.alt)
            return;
        if (!_positionHandle.ShowHandler)
            return;

        LoopThoughtMove();
        LoopThoughtRotate();
    }

    /// <summary>
    /// all event of the RaceScene UI 
    /// </summary>
    private void EventInput(SceneView view, Event e)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (e.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                break;
        }

        //clic on gost
        //click on spline: move player
        if (hitScene.objHit != null && e.control && e.alt && e.button == 1 && e.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseDown)
        {
            DeleteChild(hitScene.objHit);
        }
        if (e.control && e.alt && e.button == 0 && e.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseDown)
        {
            CreateChild(hitScene.pointHit, hitScene.normal);
        }
    }

    /// <summary>
    /// try to duplicate one from array
    /// </summary>
    /// <param name="position"></param>
    /// <param name="normal"></param>
    private void CreateChild(Vector3 position, Vector3 normal)
    {
        //defaultParent
        if (_positionHandle._allChildToMove.Count <= 0)
            return;

        Instantiate(_positionHandle._allChildToMove[0], position, ExtQuaternion.DirToQuaternion(normal), _positionHandle.defaultParent);
    }

    private void DeleteChild(GameObject toDelete)
    {
        if (!_positionHandle.canDelete)
            return;
        if (_positionHandle._allChildToMove.Count <= 1)
            return;

        if (_positionHandle._allChildToMove.Contains(toDelete.transform))
        {
            Undo.DestroyObjectImmediate(toDelete);
        }
    }

    private void OnSceneGUI()
    {
        if (_positionHandle.canDelete)
        {
            SceneView view = SceneView.currentDrawingSceneView;
            Event e = Event.current;

            hitScene = ExtUtilityEditor.SetCurrentOverObject(hitScene);

            EventInput(view, e);
        }
        HandleChild();
    }

    private new void OnDisable()
    {
        Tools.current = LastTool;
    }
}
