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
    public Rigidbody2D RigidBody;

    [FoldoutGroup("Object"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    private PlayerInput _playerInput;
    [FoldoutGroup("Debug"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> AllBoxInside = new List<OnCollisionObject>();


    private Vector2 _lastMove;

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

    /// <summary>
    /// is the player holding action ?
    /// </summary>
    /// <returns></returns>
    public bool IsPressingAction()
    {
        return (_playerInput.Action);
    }

    private void Move()
    {
        if (!_playerInput.IsMoving())
        {
            return;
        }
        //_rigidBody.drag = 0f;
        RigidBody.velocity = _playerInput.Move.normalized * _forceSpeed * Time.fixedDeltaTime;
    }

    private void Rotate()
    {
        if (!_playerInput.IsMoving())
        {
            _mainSprite.transform.rotation = ExtQuaternion.DirObject2d(_mainSprite.transform.rotation,_lastMove, _turnRate, ExtQuaternion.TurnType.Z, false, false, true);
            return;
        }
        _mainSprite.transform.rotation = ExtQuaternion.DirObject2d(_mainSprite.transform.rotation, _playerInput.Move, _turnRate, ExtQuaternion.TurnType.Z, false, false, true);
        _lastMove = _playerInput.Move;
    }

    // Update is called once per frame
    public void CustomFixedUpdate()
    {
        Move();
        Rotate();
    }
}
