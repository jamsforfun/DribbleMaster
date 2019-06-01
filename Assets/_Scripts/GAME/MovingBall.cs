using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBall : MonoBehaviour
{
    [FoldoutGroup("GamePLay"), Tooltip(""), SerializeField]
    private float _forceSpeed = 10000f;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private Rigidbody2D _rigidBody;

    [FoldoutGroup("Debug"), Tooltip(""), SerializeField]
    private float _coolDownBounce = 0.1f;

    private Vector2 lastDirection;
    private FrequencyCoolDown coolDown = new FrequencyCoolDown();

    private void Start()
    {
        SetRotation(ExtRandom.GetRandomVectorNormalized());
    }

    /// <summary>
    /// set a normalized direction
    /// </summary>
    /// <param name="direction"></param>
    private void SetRotation(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        lastDirection = direction.normalized;
        _rigidBody.velocity = lastDirection * _forceSpeed * Time.fixedDeltaTime;
    }

    private void Move()
    {
        //_rigidBody.drag = 0f;
        _rigidBody.velocity = _rigidBody.velocity.normalized * _forceSpeed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        Move();
        if (coolDown.IsReady())
        {

        }
    }
}
