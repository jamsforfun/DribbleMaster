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
    }

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    public TypeObject TypeRigidBodyMe = TypeObject.PLAYER;

    [FoldoutGroup("Object"), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBodyMe", TypeObject.BOX), SerializeField, FormerlySerializedAs("_boxManager")]
    private BoxManager BoxManager;
    [FoldoutGroup("Object"), ShowIf("TypeRigidBodyMe", TypeObject.PLAYER), Tooltip(""), FormerlySerializedAs("_playerController")]
    public PlayerController PlayerController;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField, ReadOnly]
    public List<OnCollisionObject> ListRigidBody = new List<OnCollisionObject>();

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
            ListRigidBody.Add(collisionObject);
            collisionObject.ListRigidBody.AddIfNotContain(this);
        }

        if (collisionObject == null)
        {
            return;
        }

        //if we are a Box, and other collision is a player
        if (TypeRigidBodyMe == TypeObject.BOX && collisionObject.TypeRigidBodyMe == TypeObject.PLAYER)
        {
            BoxManager.OnPlayerPushOrUnpush();
        }
        //else if we are a box, and other is a box too...
        else if (TypeRigidBodyMe == TypeObject.BOX && collisionObject.TypeRigidBodyMe == TypeObject.BOX)
        {
            BoxManager.CollideWithOtherBox(collisionObject.BoxManager);
        }
    }

    
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionObject collisionObject = collision.collider.GetExtComponentInParents<OnCollisionObject>(99, true);

        if (collisionObject && ListRigidBody.Contains(collisionObject))
        {
            ListRigidBody.Remove(collisionObject);

            
            //if we are a Box, and other collision is a player
            if (TypeRigidBodyMe == TypeObject.BOX && collisionObject.TypeRigidBodyMe == TypeObject.PLAYER)
            {
                BoxManager.OnPlayerPushOrUnpush();
            }
            //else if we are a box, and other is a box too...
            else if (TypeRigidBodyMe == TypeObject.BOX && collisionObject.TypeRigidBodyMe == TypeObject.BOX)
            {
                BoxManager.UnCollideWithOtherBox(collisionObject.BoxManager);
            }
            
        }
    }
    
}
