using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), OnValueChanged("Init"), SerializeField]
    public int IdPlayer = 0;
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private Color _color;
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private float _forceSpeed = 5f;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private SpriteRenderer _mainSprite;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private SpriteRenderer[] eyes;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private Rigidbody2D _rigidBody;

    [FoldoutGroup("Object"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private PlayerInput _playerInput;

    private void OnEnable()
    {
        Init();
    }

    [Button]
    public void Init()
    {
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].color = _color;
        }
    }

    private void Move()
    {
        if (!_playerInput.IsMoving())
        {
            //_rigidBody.drag = 10f;
            return;
        }
        //_rigidBody.drag = 0f;
        _rigidBody.MovePosition(_rigidBody.position + _playerInput.Move * _forceSpeed);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }
}
