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

    [FoldoutGroup("Debug"), Tooltip(""), SerializeField]
    private List<OnCollisionObject> allPlayerPushingIt = new List<OnCollisionObject>();

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
        allPlayerPushingIt.Clear();

        for (int i = 0; i < _player.Count; i++)
        {
            if (_player[i].DoWeAreCollidingWithThat(toTest))
            {
                allPlayerPushingIt.AddIfNotContain(_player[i]);
            }
        }
        TryToAddOther();


        return (allPlayerPushingIt.Count);
    }

    /// <summary>
    /// Try to add the one pushing to other player...
    /// </summary>
    private void TryToAddOther()
    {
        //here do another pass...
        for (int i = 0; i < allPlayerPushingIt.Count; i++)
        {
            for (int j = 0; j < allPlayerPushingIt[i].ListRigidBody.Count; j++)
            {
                if (allPlayerPushingIt[i].ListRigidBody[j].TypeRigidBodyMe == OnCollisionObject.TypeObject.PLAYER
                    && !allPlayerPushingIt.Contains(allPlayerPushingIt[i].ListRigidBody[j]))
                {
                    allPlayerPushingIt.AddIfNotContain(allPlayerPushingIt[i].ListRigidBody[j]);
                    TryToAddOther();
                }
            }
        }
    }
}
