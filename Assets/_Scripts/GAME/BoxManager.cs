using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the box mass depending on whatever players are pushing it or not
/// </summary>
public class BoxManager : MonoBehaviour
{
    private const float MASS_WHEN_WE_CAN_PUSH = 1f;
    private const float MASS_WHEN_WE_CANT_PUSH = 1000f;

    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField, ReadOnly]
    private bool IsPushed = false;

    [FoldoutGroup("Render"), Tooltip(""), SerializeField]
    private SpriteRenderer[] _allSpriteRender;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private FrameSizer _frameSizer;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private OnCollisionObject _onCollisionObject;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private Rigidbody2D _rbBox;

    [FoldoutGroup("Debug"), Tooltip(""), SerializeField]
    private Aglomera _aglomeraRef;

    private void OnEnable()
    {
        if (_aglomeraRef)
        {
            _aglomeraRef.AddBox(this);
        }
    }

    /// <summary>
    /// call here when another Box is collider with us
    /// </summary>
    /// <param name="other"></param>
    public void CollideWithOtherBox(BoxManager other)
    {
        Debug.Log("2 box collision");
    }
    /// <summary>
    /// call here when the box leave other box
    /// </summary>
    /// <param name="other"></param>
    public void UnCollideWithOtherBox(BoxManager other)
    {
        Debug.Log("2 box uncollide");
    }

    /// <summary>
    /// each time players push, or unpush the box, we go here, and check for changing
    /// the mass
    /// </summary>
    public void OnPlayerPushOrUnpush()
    {
        //here call the parent if exist
        if (_aglomeraRef != null)
        {

            IsPushed = false;
        }
        else
        {
            //else, we are alone !
            int numberOfPlayer = _countPlayerPushingBox.GetNumberPlayerPushingMe(_onCollisionObject);
            if (_frameSizer.CanPushThis(numberOfPlayer))
            {
                _rbBox.mass = MASS_WHEN_WE_CAN_PUSH;
                IsPushed = true;
            }
            else
            {
                _rbBox.mass = MASS_WHEN_WE_CANT_PUSH;
                IsPushed = false;
            }
        }

        //change 
        ChangeMassCorrectly();
        ChangeColor();
    }

    /// <summary>
    /// change mass of box
    /// </summary>
    private void ChangeMassCorrectly()
    {
        _rbBox.mass = (IsPushed) ? MASS_WHEN_WE_CAN_PUSH : MASS_WHEN_WE_CANT_PUSH;
    }

    /// <summary>
    /// change color of box
    /// </summary>
    private void ChangeColor()
    {
        for (int i = 0; i < _allSpriteRender.Length; i++)
        {
            _allSpriteRender[i].color = (IsPushed) ? Color.green : Color.red;
        }
    }
}
