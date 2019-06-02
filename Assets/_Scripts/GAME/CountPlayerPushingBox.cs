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
    private List<PlayerController> allPlayerPushingIt = new List<PlayerController>();

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
                allPlayerPushingIt.AddIfNotContain(_player[i].PlayerController);
            }
        }

        //here do another pass...

        return (allPlayerPushingIt.Count);
    }
}
