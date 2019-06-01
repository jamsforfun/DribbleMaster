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

    [FoldoutGroup("Object"), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBody", TypeObject.BOX), SerializeField]
    private BoxManager _boxManager;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBody", TypeObject.PLAYER), Tooltip(""), SerializeField]
    private PlayerController _playerController;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> ListRigidBody = new List<OnCollisionObject>();

    private void OnEnable()
    {
        if (TypeRigidBody == TypeObject.BOX)
        {
            _countPlayerPushingBox.AddBox(this);
        }
        else if (TypeRigidBody == TypeObject.PLAYER)
        {
            _countPlayerPushingBox.AddPlayer(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && !ListRigidBody.Contains(collisionObject))
        {
            ListRigidBody.Add(collisionObject);

            if (TypeRigidBody == TypeObject.BOX)
            {
                _boxManager.OnChangePlayers();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && ListRigidBody.Contains(collisionObject))
        {
            ListRigidBody.Remove(collisionObject);

            if (TypeRigidBody == TypeObject.BOX)
            {
                _boxManager.OnChangePlayers();
            }
        }
    }
}
