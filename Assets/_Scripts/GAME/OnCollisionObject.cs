using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionObject : MonoBehaviour
{
    public enum TypeObject
    {
        NONE,
        BOX,
        PLAYER,
    }

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    public TypeObject TypeRigidBody = TypeObject.PLAYER;

    [FoldoutGroup("Object"), ShowIf("TypeRigidBody", TypeObject.BOX), SerializeField]
    private BoxManager _boxManager;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBody", TypeObject.PLAYER), Tooltip(""), SerializeField]
    private PlayerController _playerController;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField, ReadOnly]
    private List<OnCollisionObject> _listRigidBody = new List<OnCollisionObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && !_listRigidBody.Contains(collisionObject))
        {
            _listRigidBody.Add(collisionObject);

            if (TypeRigidBody == TypeObject.BOX)
            {
                //_boxManager
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && _listRigidBody.Contains(collisionObject))
        {
            _listRigidBody.Remove(collisionObject);
        }
    }
}
