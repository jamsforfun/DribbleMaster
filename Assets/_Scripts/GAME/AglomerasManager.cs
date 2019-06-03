using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AglomerasManager : MonoBehaviour
{
    [FoldoutGroup("Debug"), Tooltip(""), SerializeField, ReadOnly]
    public List<Aglomera> AllAglomera = new List<Aglomera>();
    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    private CountPlayerPushingBox _countPlayerPushingBox;

    [FoldoutGroup("Object"), Tooltip(""), SerializeField]
    public Transform ParentNormalBox;

    [FoldoutGroup("Prefabs"), Tooltip(""), SerializeField]
    private GameObject _aglomeraPrefabs;

    /// <summary>
    /// add an aglomera to the list (called on the OnEnable of the aglomera)
    /// </summary>
    /// <param name="aglomera"></param>
    public void AddAglomera(Aglomera aglomera)
    {
        Debug.Log("called both time ?");
        if (!AllAglomera.Contains(aglomera))
        {
            AllAglomera.AddIfNotContain(aglomera);
        }
    }

    /// <summary>
    /// called when a box is colliding with another box,
    /// and no one is pressing A
    /// </summary>
    /// <returns></returns>
    public Aglomera CreateAglomera(List<OnCollisionObject> allbox)
    {
        GameObject newAglomeraObject = Instantiate(_aglomeraPrefabs, transform);
        Aglomera newAglomera = newAglomeraObject.GetComponent<Aglomera>();

        newAglomera.Init(this, _countPlayerPushingBox);
        newAglomera.AddBoxToOurAglomera(allbox);

        AddAglomera(newAglomera);

        return (newAglomera);
    }
}
