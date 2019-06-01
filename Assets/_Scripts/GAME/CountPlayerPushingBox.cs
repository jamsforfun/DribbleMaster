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

    public int GetNumberPlayerPushingMe(OnCollisionObject toTest)
    {
        return (0);
    }
}
