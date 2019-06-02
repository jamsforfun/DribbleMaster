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
            _onCollisionObject.ListRigidBodyBox.Remove(other);
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

    /// <summary>
    /// return the aglomera of the other !
    /// </summary>
    /// <returns></returns>
    private Aglomera IsOtherInAglomera()
    {
        for (int i = 0; i < _onCollisionObject.ListRigidBodyBox.Count; i++)
        {
            if (_onCollisionObject.ListRigidBodyBox[i].BoxManager.AglomeraRef != null)
            {
                return (_onCollisionObject.ListRigidBodyBox[i].BoxManager.AglomeraRef);
            }
        }
        return (null);
    }

    /// <summary>
    /// try to have 2 aglomera stick with each other
    /// </summary>
    private void CollideWithOtherBox()
    {
        if (_onCollisionObject.ListRigidBodyBox.Count == 0)
        {
            return;
        }
        Aglomera other = IsOtherInAglomera();

        if (other == null)
        {
            return;
        }
        Debug.Log("contact between 2 aglomera !");
    }

    
    /// <summary>
    /// remove child collider ! We don't want a child to collider with ourselve
    /// </summary>
    private void CleanColliderInsideUs()
    {
        for (int i = 0; i < _onCollisionObject.ListRigidBodyBox.Count; i++)
        {
            if (_onCollisionObject.ListRigidBodyBox[i].BoxManager.AglomeraRef
                && _onCollisionObject.ListRigidBodyBox[i].BoxManager.AglomeraRef == this)
            {
                _onCollisionObject.ListRigidBodyBox.RemoveAt(i);
                CleanColliderInsideUs();
            }
        }
    }
    

    private void FixedUpdate()
    {
        CleanColliderInsideUs();

        OnPlayerPushOrUnpush();
        CollideWithOtherBox();
    }
}
