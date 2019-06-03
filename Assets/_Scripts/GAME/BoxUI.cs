using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUI : MonoBehaviour
{
    [FoldoutGroup("UI"), Tooltip(""), SerializeField]
    private GameObject ActionObject;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private BoxManager _boxManager;

    public void ActiveAction(bool isPressingA)
    {
        ActionObject.SetActive(isPressingA);
    }
}
