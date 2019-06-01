using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aglomera : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private List<BoxManager> _allBoxManager;

    /// <summary>
    /// add box at start
    /// </summary>
    public void AddBox(BoxManager box)
    {
        _allBoxManager.AddIfNotContain(box);
    }

    public void RemoveBox(BoxManager box)
    {
        _allBoxManager.Remove(box);
    }
}
