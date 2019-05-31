using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHandleChilds : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public bool ShowHandler = true;
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public bool LocalSpace = true;

    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public List<Transform> _allChildToMove = new List<Transform>();
    
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public List<Transform> _allChildToRotate = new List<Transform>();

    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public bool canDelete = false;
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public Transform defaultParent;

    /// <summary>
    /// clear fields
    /// </summary>
    public void ResetAll()
    {
        _allChildToMove.Clear();
        _allChildToRotate.Clear();
        Vector3 vec = Vector3.zero;
    }

    public void Init(Transform [] allChildMove, Transform[] allChildRotate)
    {
        _allChildToMove = ExtList.ToList(allChildMove);
        _allChildToRotate = ExtList.ToList(allChildRotate);
    }
    public void Init(List<Transform> allChildMove, List<Transform> allChildRotate)
    {
        _allChildToMove = allChildMove;
        _allChildToRotate = allChildRotate;
    }
}
