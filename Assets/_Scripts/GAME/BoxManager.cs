using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private OnCollisionObject _onCollisionObject;

    public void OnChangePlayers()
    {

    }
}
