using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// ObjectsPooler Description
/// </summary>
public class ObjectsPooler : SingletonMono<ObjectsPooler>
{
    protected ObjectsPooler() { } // guarantee this will be always a singleton only - can't use the constructor!

    #region Attributes
    [System.Serializable]
    public class Pool
    {
        public GameData.PoolTag tag;
        public GameObject prefab;
        public int size;
        public bool shouldExpand = false;
    }

    [FoldoutGroup("GamePlay"), Tooltip("new pool"), SerializeField]
    private List<Pool> pools = new List<Pool>();

    private Dictionary<GameData.PoolTag, List<GameObject>> poolDictionary;

    #endregion

    #region Initialization

    private void Awake()
    {
        InitPool();
    }

    /// <summary>
    /// initialise la pool
    /// </summary>
    private void InitPool()
    {
        poolDictionary = new Dictionary<GameData.PoolTag, List<GameObject>>();

        for (int j = 0; j < pools.Count; j++)
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < pools[j].size; i++)
            {
                GameObject obj = Instantiate(pools[j].prefab, transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolDictionary.Add(pools[j].tag, objectPool);
        }
    }

    #endregion

    #region Core
    /// <summary>
    /// ici désactive tout les éléments de la pool qui sont actuellement activé...
    /// Appeler une fonction spécial ??
    /// </summary>
    public void desactiveEveryOneForTransition()
    {
        foreach (KeyValuePair<GameData.PoolTag, List<GameObject>> attachStat in poolDictionary)
        {
            List<GameObject> objFromTag = attachStat.Value;
            for (int j = 0; j < objFromTag.Count; j++)
            {
                //l'objet en question
                GameObject obj = objFromTag[j];
                if (!obj)
                    continue;

                //est-ce que l'objet est dans la pool ? (le transform), Si non, le mettre
                if (obj.transform.parent.GetInstanceID() != transform.GetInstanceID())
                {
                    Debug.Log("ici set le transform parent de: " + obj.name);
                    obj.transform.SetParent(transform);
                }
                if (obj.activeSelf)
                {
                    IPooledObject pooledObject = objFromTag[j].GetComponent<IPooledObject>();

                    if (pooledObject != null)
                    {
                        pooledObject.OnDesactivePool();
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// access object from pool
    /// </summary>
    public GameObject SpawnFromPool(GameData.PoolTag tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("pool with tag: " + tag + "doesn't exist");
            return (null);
        }

        List<GameObject> objFromTag = poolDictionary[tag];

        for (int i = 0; i < objFromTag.Count; i++)
        {
            if (objFromTag[i] && !objFromTag[i].activeSelf)
            {
                //ici on récupère un objet de la pool !

                objFromTag[i].SetActive(true);
                objFromTag[i].transform.position = position;
                objFromTag[i].transform.rotation = rotation;
                objFromTag[i].transform.SetParent(parent);

                IPooledObject pooledObject = objFromTag[i].GetComponent<IPooledObject>();

                if (pooledObject != null)
                {
                    pooledObject.OnObjectSpawn();
                }

                return (objFromTag[i]);
            }
        }

        Debug.Log("ici on a raté ! tout les objets de la pools sont complet !!");
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].tag == tag)
            {
                if (pools[i].shouldExpand)
                {
                    GameObject obj = Instantiate(pools[i].prefab, transform);
                    //obj.SetActive(false);
                    objFromTag.Add(obj);


                    obj.SetActive(true);
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.transform.SetParent(parent);

                    IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

                    if (pooledObject != null)
                    {
                        pooledObject.OnObjectSpawn();
                    }

                    return (obj);


                }
                else
                {
                    Debug.LogError("pas d'expantion, error pour le tag: " + tag);

                    break;
                }
            }
        }


        return (null);
    }
    #endregion
}
