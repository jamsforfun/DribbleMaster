using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), OnValueChanged("Init"), SerializeField]
    public PlayerSettings PlayerSettings;
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private float _forceSpeed = 10000f;
    [FoldoutGroup("GamePLay"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private float _turnRate = 30f;

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
            eyes[i].color = PlayerSettings.Color;
        }
    }

    private void Move()
    {
        if (!_playerInput.IsMoving())
        {
            return;
        }
        //_rigidBody.drag = 0f;
        _rigidBody.velocity = _playerInput.Move * _forceSpeed * Time.fixedDeltaTime;
    }

    private void Rotate()
    {
        if (!_playerInput.IsMoving())
        {
            return;
        }
        _mainSprite.transform.rotation = ExtQuaternion.DirObject2d(_mainSprite.transform.rotation, _playerInput.Move, _turnRate, ExtQuaternion.TurnType.Z);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
}
