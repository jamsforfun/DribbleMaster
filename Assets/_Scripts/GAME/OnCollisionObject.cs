﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OnCollisionObject : MonoBehaviour
{
    public enum TypeObject
    {
        NONE,
        BOX,
        PLAYER,
        AGLOMERA,
    }

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    public TypeObject TypeRigidBodyMe = TypeObject.PLAYER;

    [FoldoutGroup("Object"), HideIf("TypeRigidBodyMe", TypeObject.AGLOMERA), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBodyMe", TypeObject.BOX), SerializeField, FormerlySerializedAs("_boxManager")]
    public BoxManager BoxManager;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBodyMe", TypeObject.PLAYER), Tooltip(""), FormerlySerializedAs("_playerController")]
    public PlayerController PlayerController;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBodyMe", TypeObject.AGLOMERA), Tooltip(""), FormerlySerializedAs("_playerController")]
    public Aglomera Aglomera;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> ListRigidBody = new List<OnCollisionObject>();
    [FoldoutGroup("Object"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> ListRigidBodyPlayer = new List<OnCollisionObject>();
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    public List<OnCollisionObject> ListRigidBodyBox = new List<OnCollisionObject>();

    private void OnEnable()
    {
        if (TypeRigidBodyMe == TypeObject.BOX)
        {
            _countPlayerPushingBox.AddBox(this);
        }
        else if (TypeRigidBodyMe == TypeObject.PLAYER)
        {
            _countPlayerPushingBox.AddPlayer(this);
        }
    }

    /// <summary>
    /// return true if we are colliding with that specific object
    /// </summary>
    /// <param name="collisionObject"></param>
    /// <returns></returns>
    public bool DoWeAreCollidingWithThat(OnCollisionObject collisionObject)
    {
        return (ListRigidBody.Contains(collisionObject));
    }

    /// <summary>
    /// if we collide with other OnCollisionObject
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);


        if (collisionObject && !ListRigidBody.Contains(collisionObject))
        {

            //don't add if we are an Aglomera, and this object is a child of ourself !
            if (collisionObject.TypeRigidBodyMe == TypeObject.AGLOMERA)
            {
                if (collisionObject.Aglomera && Aglomera && collisionObject.Aglomera.GetInstanceID() == Aglomera.GetInstanceID())
                {
                    return;
                }
            }

            ListRigidBody.Add(collisionObject);
            collisionObject.ListRigidBody.AddIfNotContain(this);
        }

        if (collisionObject == null)
        {
            return;
        }

        //if other is a player
        if (collisionObject.TypeRigidBodyMe == TypeObject.PLAYER)
        {
            ListRigidBodyPlayer.AddIfNotContain(collisionObject);
        }
        //if other is a box
        else if (collisionObject.TypeRigidBodyMe == TypeObject.BOX)
        {
            ListRigidBodyBox.AddIfNotContain(collisionObject);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && ListRigidBody.Contains(collisionObject))
        {
            ListRigidBody.Remove(collisionObject);
            
            //if other is a player
            if (collisionObject.TypeRigidBodyMe == TypeObject.PLAYER)
            {
                ListRigidBodyPlayer.Remove(collisionObject);
            }
            //if other is a box
            else if (collisionObject.TypeRigidBodyMe == TypeObject.BOX)
            {
                ListRigidBodyBox.Remove(collisionObject);
            }

        }
    }
    
}
