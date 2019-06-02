using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class has the ref of all players / boxs
/// </summary>
public class CountPlayerPushingBox : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private List<OnCollisionObject> _player;
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private List<OnCollisionObject> _box;

    /// <summary>
    /// add player at start
    /// </summary>
    public void AddPlayer(OnCollisionObject player)
    {
        _player.AddIfNotContain(player);
    }

    /// <summary>
    /// add box at start
    /// </summary>
    public void AddBox(OnCollisionObject box)
    {
        _box.AddIfNotContain(box);
    }

    /// <summary>
    /// count how many player are pushing this box
    /// </summary>
    /// <param name="toTest"></param>
    /// <returns></returns>
    public int GetNumberPlayerPushingMe(OnCollisionObject toTest)
    {
        BoxManager box = toTest.BoxManager;



        box.AllPlayerColliding.Clear();

        for (int i = 0; i < _player.Count; i++)
        {
            if (_player[i].DoWeAreCollidingWithThat(toTest))
            {
                box.AllPlayerColliding.AddIfNotContain(_player[i]);
            }
        }
        TryToAddOther(box);


        return (box.AllPlayerColliding.Count);
    }

    /// <summary>
    /// Try to add the one pushing to other player...
    /// </summary>
    private void TryToAddOther(BoxManager box)
    {
        //here do another pass...
        for (int i = 0; i < box.AllPlayerColliding.Count; i++)
        {
            for (int j = 0; j < box.AllPlayerColliding[i].ListRigidBody.Count; j++)
            {
                if (box.AllPlayerColliding[i].ListRigidBody[j].TypeRigidBodyMe == OnCollisionObject.TypeObject.PLAYER
                    && !box.AllPlayerColliding.Contains(box.AllPlayerColliding[i].ListRigidBody[j]))
                {
                    box.AllPlayerColliding.AddIfNotContain(box.AllPlayerColliding[i].ListRigidBody[j]);
                    TryToAddOther(box);
                }
            }
        }
    }

    /// <summary>
    /// fill the list of player & box: for knowing in wich box we are as player,
    /// and in each box, wich player are in where
    /// </summary>
    private void SetInWichBoxIsEachPlayer()
    {
        //first clear all list
        for (int i = 0; i < _box.Count; i++)
        {
            _box[i].BoxManager.AllPlayerInside.Clear();
        }
        for (int i = 0; i < _player.Count; i++)
        {
            _player[i].PlayerController.AllBoxInside.Clear();
        }

        //then for each player...
        for (int i = 0; i < _player.Count; i++)
        {
            SetInWichBoxIsThisPlayer(_player[i]);
        }
    }

    private void SetInWichBoxIsThisPlayer(OnCollisionObject player)
    {
        for (int i = 0; i < _box.Count; i++)
        {
            if (_box[i].BoxManager.IsObjectInsideBox(player.PlayerController.RigidBody.transform.position))
            {
                _box[i].BoxManager.AllPlayerInside.AddIfNotContain(player);
                player.PlayerController.AllBoxInside.AddIfNotContain(_box[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        SetInWichBoxIsEachPlayer(); //set all player in all boxs

        for (int i = 0; i < _player.Count; i++)
        {
            _player[i].PlayerController.CustomFixedUpdate();
        }

        for (int i = 0; i < _box.Count; i++)
        {
            if (_box[i].BoxManager.enabled)
            {
                _box[i].BoxManager.CustomFixedUpdate();
            }
        }
    }
}
