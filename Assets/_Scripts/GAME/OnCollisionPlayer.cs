using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlayer : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
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
