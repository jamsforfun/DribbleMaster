using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct DataPrefabsInfo
{
    public string ObjToLoadName;
    public string LocalPathPrefabs;
    public GameObject GameObjectToSave;
    public string PreviousGameObjectLoaded;

    public DataPrefabsInfo(string mainToLoadName, string localPathPrefabs)
    {
        ObjToLoadName = mainToLoadName;
        LocalPathPrefabs = localPathPrefabs;
        GameObjectToSave = null;
        PreviousGameObjectLoaded = "";
    }
}

[System.Serializable]
public class MainData : PersistantData
{
    protected string fileName = "";
    public MainData(string name)
    {
        fileName = name;
        SetDefault();
    }

    [SerializeField]
    public int lastMapSelected = 0;
    [SerializeField]
    private List<string> mapCreated = new List<string>();
    public int MapCreatedSize() => mapCreated.Count;

    public void DeleteThisMap(int index)
    {
        mapCreated.RemoveAt(index);
        if (lastMapSelected == index)
            lastMapSelected = 0;
    }

    public string GetMapNameByIndex(int index)
    {
        if (mapCreated.Count == 0 || index < 0 || index >= mapCreated.Count)
            return (null);
        return (mapCreated[index]);
    }
    public void SetMapSelected(int index)
    {
        if (mapCreated.Count == 0 || index < 0 || index > mapCreated.Count)
            return;

        lastMapSelected = index;
    }

    public string GetLastMapSelectedName()
    {
        if (mapCreated.Count == 0 || lastMapSelected < 0 || lastMapSelected > mapCreated.Count)
            return (null);
        return (mapCreated[lastMapSelected]);
    }
    public int GetLastMapSelectedIndex()
    {
        if (mapCreated.Count == 0 || lastMapSelected < 0 || lastMapSelected > mapCreated.Count)
            return (-1);
        return (lastMapSelected);
    }

    /// <summary>
    /// is this map (with extention) is created ?
    /// </summary>
    /// <param name="map"></param>
    public bool CreateMap(string map)
    {
        if (mapCreated.Contains(map))
            return (false);
        mapCreated.Add(map);
        return (true);
    }
    public void DeleteMap(string map)
    {
        mapCreated.Remove(map);
        mapCreated.Clear();
    }

    /// <summary>
    /// reset toute les valeurs à celle d'origine pour le jeu
    /// </summary>
    public void SetDefault()
    {
        lastMapSelected = 0;
        mapCreated.Clear();
    }

    public override string GetFilePath()
    {
        return (fileName);
    }
}

[System.Serializable]
public class MapData : PersistantData
{
    protected string fileName = "";
    public MapData(string name)
    {
        fileName = name;
    }
    public void ChangeName(string newName)
    {
        fileName = newName;
    }

    [Space(30)]
    [Header("Path Prefabs")]
    public string LocalPathMainPlayerDatasToLoad;
    public string LocalPathLdToLoad;
    public string LocalPathObjectInRoad;
    [Space(10)]
    [Header("Name Prefabs")]
    public string MainPlayerDatasToLoad = "Main_Player_Datas_default";
    public string LDroadToLaod = "LD_To_Load_default";
    public string ObjectInRoadToLoad = "Object_In_Road_default";

    [Space(30)]


    public float lastPlayerPercentInMap = 0;
    public int vuforiaCurrentlySelected = 0;
    
    /// <summary>
    /// reset toute les valeurs à celle d'origine pour le jeu
    /// </summary>
    public void SetDefault()
    {
        
    }

    public override string GetFilePath ()
	{
		return (fileName);
	}
}