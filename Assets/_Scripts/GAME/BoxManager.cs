using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private FrameSizer _frameSizer;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private OnCollisionObject _onCollisionObject;

    public void OnChangePlayers()
    {
        //here call the parent if exist

        //else, we are alone !

        int numberOfPlayer = _countPlayerPushingBox.GetNumberPlayerPushingMe(_onCollisionObject);
        if (_frameSizer.CanPushThis(numberOfPlayer))
        {
            Debug.Log("we can be pushedd !");
        }
        else
        {
            Debug.Log("no push...");
        }
    }
}
