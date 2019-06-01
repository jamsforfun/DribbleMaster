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
    private List<Rigidbody2D> _listRigidBody = new List<Rigidbody2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetExtComponentInParents<Rigidbody2D>(99, true);

        if (rb && !_listRigidBody.Contains(rb))
        {
            _listRigidBody.Add(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetExtComponentInParents<Rigidbody2D>(99, true);

        if (rb && _listRigidBody.Contains(rb))
        {
            _listRigidBody.Remove(rb);
        }
    }
}
