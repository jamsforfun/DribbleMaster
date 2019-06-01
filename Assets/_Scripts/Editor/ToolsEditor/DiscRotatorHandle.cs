using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DiscRotatorHandle
{
    private static Vector2 startMousePosition, currentMousePosition;
    private static float rotationDist;
    private static Quaternion startRotation;
    private static Vector3 startPositionOnDisc;


    public static Quaternion Do(int controlId, Vector3 position, Quaternion rotation, float size, float radius, Transform objectToRotate)
    {
        Quaternion discRotation = rotation * Quaternion.Euler(90, 0, 0);

        Event e = Event.current;

        switch (e.GetTypeForControl(controlId))
        {
            case EventType.Layout:
                Handles.CircleHandleCap(controlId, position, discRotation, size, EventType.Layout);
                break;
            case EventType.MouseDown:
                //am I closest to the thing ?
                if ((HandleUtility.nearestControl == controlId && e.button == 0 && !e.alt) && GUIUtility.hotControl == 0)
                {
                    Undo.RecordObject(objectToRotate, "rotation One Axis");
                    GUIUtility.hotControl = controlId;
                    currentMousePosition = startMousePosition = e.mousePosition;
                    startRotation = rotation;
                    startPositionOnDisc = HandleUtility.ClosestPointToDisc(position, Vector3.up, size) - position;
                    e.Use();
                    EditorGUIUtility.SetWantsMouseJumping(1);
                }
                break;
            case EventType.MouseDrag:
                if (controlId == GUIUtility.hotControl)
                {
                    currentMousePosition += e.delta;
                    rotationDist = currentMousePosition.x - startMousePosition.x;
                    rotation = Quaternion.AngleAxis(rotationDist * -1, Vector3.up) * startRotation;

                    GUI.changed = true;
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                if (controlId == GUIUtility.hotControl && (e.button == 0 || e.button == 2))
                {
                    GUIUtility.hotControl = 0;  //we no longer using the event
                    e.Use();    //block the event: other script can't use it
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    rotationDist = 0;
                }
                break;
            case EventType.MouseMove:
                if (controlId == HandleUtility.nearestControl)
                    HandleUtility.Repaint();
                break;
            //every time the scene view is redrawn
            case EventType.Repaint:
                Color temp = Color.white;
                //is our control is active ?
                if (controlId == GUIUtility.hotControl)
                {
                    temp = Handles.color;
                    Handles.color = Handles.selectedColor * new Color(1f, 1f, 1f, 0.3f);
                    Handles.DrawSolidArc(position, Vector3.up, startPositionOnDisc, rotationDist * -1, size);
                    Handles.color = Handles.selectedColor;
                }
                //mouseOver effect
                else if (controlId == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
                {
                    temp = Handles.color;
                    Handles.color = Handles.preselectionColor;
                }
                Handles.CircleHandleCap(controlId, position, rotation, size, EventType.Repaint);
                if (controlId == GUIUtility.hotControl || controlId == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
                    Handles.color = temp;
                break;
        }

        return (rotation);
    }
}
