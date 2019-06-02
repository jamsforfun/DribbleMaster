using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aglomera : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField, ReadOnly]
    private bool IsPushed = false;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private AglomerasManager _aglomeraManager;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private List<BoxManager> _allBoxManager;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private Rigidbody2D _agloRigid;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private OnCollisionObject _onCollisionObject;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;

    public void Init(AglomerasManager aglomeraManager, CountPlayerPushingBox countPlayerPushingBox)
    {
        _aglomeraManager = aglomeraManager;
        _countPlayerPushingBox = countPlayerPushingBox;
    }

    private void OnEnable()
    {
        if (_aglomeraManager)
        {
            _aglomeraManager.AddAglomera(this);
        }
    }

    /// <summary>
    /// add box (called from BoxManager OnEnable if they have an aglomera);
    /// </summary>
    public void AddBox(BoxManager box)
    {
        _allBoxManager.AddIfNotContain(box);
    }

    public void RemoveBox(BoxManager box)
    {
        _allBoxManager.Remove(box);
    }

    public void AddThisBoxToOurAglomera(OnCollisionObject other)
    {
        if (!_allBoxManager.Contains(other.BoxManager))
        {
            //here add a new box to our aglomera !
            other.BoxManager.AglomeraRef = this;
            _allBoxManager.AddIfNotContain(other.BoxManager);
            other.BoxManager.transform.SetParent(transform);
            other.enabled = false;

            Destroy(other.BoxManager.RbBox);
        }
    }

    public void AddBoxToOurAglomera(List<OnCollisionObject> allbox)
    {
        for (int i = 0; i < allbox.Count; i++)
        {
            AddThisBoxToOurAglomera(allbox[i]);
        }
    }

    private void OnPlayerPushOrUnpush()
    {
        /*
        int numberOfPlayer = _countPlayerPushingBox.GetNumberPlayerPushingMe(_onCollisionObject);
        for (int i = 0; i < _allBoxManager.Count; i++)
        {

        }
        */
    }

    private void FixedUpdate()
    {
        OnPlayerPushOrUnpush();
    }
}
