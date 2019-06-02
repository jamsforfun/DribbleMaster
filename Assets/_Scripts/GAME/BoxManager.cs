using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the box mass depending on whatever players are pushing it or not
/// </summary>
public class BoxManager : MonoBehaviour
{
    public const float MASS_WHEN_WE_CAN_PUSH = 1f;
    public const float MASS_WHEN_WE_CANT_PUSH = 1000f;

    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField, ReadOnly]
    private bool IsPushed = false;
    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField, ReadOnly]
    private bool IsPressingA = false;

    [FoldoutGroup("Render"), Tooltip(""), SerializeField]
    private SpriteRenderer[] _allSpriteRender;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private AglomerasManager _aglomeraManager;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private BoxUI _boxUI;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private FrameSizer _frameSizer;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private OnCollisionObject _onCollisionObject;
    public OnCollisionObject GetThisCollisionObject() => _onCollisionObject;
    [FoldoutGroup("Object"), Tooltip("")]
    public Rigidbody2D RbBox;

    [FoldoutGroup("Debug"), Tooltip("")]
    public Aglomera AglomeraRef;
    [FoldoutGroup("Debug"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> AllPlayerColliding = new List<OnCollisionObject>();
    [FoldoutGroup("Debug"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> AllPlayerInside = new List<OnCollisionObject>();


    private void OnEnable()
    {
        if (AglomeraRef)
        {
            AglomeraRef.AddBox(this);
        }
    }

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
    /// if the box who are colliding with us is pressing A...
    /// </summary>
    /// <returns></returns>
    private bool IsOtherBoxIsPressingA()
    {
        for (int i = 0; i < _onCollisionObject.ListRigidBodyBox.Count; i++)
        {
            if (_onCollisionObject.ListRigidBodyBox[i].BoxManager.IsPressingA)
            {
                return (true);
            }
        }
        return (false);
    }

    /// <summary>
    /// call here when another Box is collider with us
    /// </summary>
    /// <param name="other"></param>
    public void CollideWithOtherBox()
    {
        //no collision with box
        if (_onCollisionObject.ListRigidBodyBox.Count == 0)
        {
            return;
        }

        Aglomera other = IsOtherInAglomera();

        //collision with box
        if (AglomeraRef == null && other == null)
        {
            if (IsPressingA || IsOtherBoxIsPressingA())
            {
                //here do nothing ! ye pressing action !
                return;
            }

            Debug.Log("create an aglometa !");
            AglomeraRef = _aglomeraManager.CreateAglomera(_onCollisionObject.ListRigidBodyBox);
        }
        else
        {
            

            if (AglomeraRef == null)
            {
                //here we are NOT in an aglorema, but other does

                if (IsPressingA)
                {
                    //here do nothing ! ye pressing action !
                    return;
                }

                AglomeraRef = other;
                AglomeraRef.AddThisBoxToOurAglomera(_onCollisionObject);
            }
            else
            {
                //here we are inside the aglomera, try to add all other box in collision

                ExtLog.LogList(_onCollisionObject.ListRigidBodyBox);
                AglomeraRef.AddBoxToOurAglomera(_onCollisionObject.ListRigidBodyBox);
            }
        }
    }
    /// <summary>
    /// call here when the box leave other box
    /// </summary>
    /// <param name="other"></param>
    public void UnCollideWithOtherBox(BoxManager other)
    {
        //Debug.Log("2 box uncollide");
    }

    /// <summary>
    /// force to be pushed
    /// </summary>
    public void ForceSetLight()
    {
        RbBox.mass = MASS_WHEN_WE_CAN_PUSH;
        IsPushed = true;
    }

    /// <summary>
    /// each time players push, or unpush the box, we go here, and check for changing
    /// the mass
    /// </summary>
    public void OnPlayerPushOrUnpush()
    {
        AllPlayerColliding.Clear();

        //here call the parent if exist
        if (AglomeraRef != null)
        {
            //AglomeraRef.ThisBoxIsPushed(this);
            /*
            int numberOfPlayer = _countPlayerPushingBox.GetNumberPlayerPushingMe(_onCollisionObject);
            if (_frameSizer.CanPushThis(numberOfPlayer))
            {
                AglomeraRef.PushAllInsideAglomera();
            }
            else
            {
                RbBox.mass = MASS_WHEN_WE_CANT_PUSH;
                IsPushed = false;
            }
            */
        }
        else
        {
            //else, we are alone !
            int numberOfPlayer = _countPlayerPushingBox.GetNumberPlayerPushingMe(_onCollisionObject);
            if (_frameSizer.CanPushThis(numberOfPlayer))
            {
                RbBox.mass = MASS_WHEN_WE_CAN_PUSH;
                IsPushed = true;
            }
            else
            {
                RbBox.mass = MASS_WHEN_WE_CANT_PUSH;
                IsPushed = false;
            }

            //change 
            ChangeMassCorrectly();
            ChangeColor();
        }
    }

    /// <summary>
    /// change mass of box
    /// </summary>
    private void ChangeMassCorrectly()
    {
        RbBox.mass = (IsPushed) ? MASS_WHEN_WE_CAN_PUSH : MASS_WHEN_WE_CANT_PUSH;
    }

    /// <summary>
    /// change color of box
    /// </summary>
    private void ChangeColor()
    {
        for (int i = 0; i < _allSpriteRender.Length; i++)
        {
            _allSpriteRender[i].color = (IsPushed) ? Color.green : Color.white;
        }
    }

    public bool IsObjectInsideBox(Vector3 positionObject)
    {
        return (_frameSizer.IsObjectInsideBox(positionObject));
    }

    /// <summary>
    /// player pressing A: do not lock to a Aglomera
    /// </summary>
    private void PressingA()
    {
        IsPressingA = false;
        for (int i = 0; i < AllPlayerInside.Count; i++)
        {
            if (AllPlayerInside[i].PlayerController.IsPressingAction())
            {
                IsPressingA = true;
                break;
            }
        }
        _boxUI.ActiveAction(IsPressingA);
    }

    public void CustomFixedUpdate()
    {
        PressingA();

        OnPlayerPushOrUnpush();
        CollideWithOtherBox();

        if (AglomeraRef != null)
        {
            this.enabled = false;
            IsPressingA = false;
        }
    }
}
