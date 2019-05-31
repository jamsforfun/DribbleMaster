using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ExtPrefabs
{
#if UNITY_EDITOR
    /// <summary>
    /// save the MeshFilter of the selected object in Assets/Resources/Procedural/
    /// </summary>
    [MenuItem("PERSO/Procedural/Save Selected Mesh")]
    public static void SaveSelectedMesh()
    {
        GameObject activeOne = Selection.activeGameObject;
        if (activeOne == null)
            return;
        SaveSelectedMeshObj(activeOne);
    }

    public static Mesh SaveSelectedMeshObj(GameObject activeOne)
    {
        if (activeOne == null)
            return (null);

        MeshFilter meshRoad = activeOne.GetComponent<MeshFilter>();
        if (meshRoad == null)
        {
            return (null);
        }

        Mesh tempMesh = (Mesh)UnityEngine.Object.Instantiate(meshRoad.sharedMesh);

        string path = "Assets/Resources/Procedural/" + SaveAsset("savedMesh");
        Debug.Log(path);
        AssetDatabase.CreateAsset(tempMesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return (tempMesh);
    }
#endif

#if UNITY_EDITOR
    /// <summary>
    /// delete the mesh asset in the gameObject if it exist
    /// </summary>
    /// <param name="obj"></param>
    public static void DeleteSelectedMesh(GameObject obj)
    {
        MeshFilter meshRoad = obj.GetComponent<MeshFilter>();
        if (!meshRoad)
        {
            return;
        }
        Mesh tempMesh = meshRoad.sharedMesh;
        AssetDatabase.DeleteAsset("Assets/Resources/Procedural/" + meshRoad.sharedMesh.name);
    }

    private static string SaveAsset(string nameMesh, string extention = "asset")
    {
        return string.Format("{0}_{1}.{2}",
                            nameMesh,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                             extention);
    }
#endif

#if UNITY_EDITOR
    /// <param name="objectToPrefabs"></param>
    /// <param name="localPath"></param>
    /// <param name="name"></param>
    public static void CreateNewPrefabs(GameObject objectToPrefabs, string _localPathAndName)
    {
        //Check if the Prefab and/or name already exists at the path
        if (AssetDatabase.LoadAssetAtPath(_localPathAndName, typeof(GameObject)))
        {
            //Create dialog to ask if User is sure they want to overwrite existing Prefab
            CreateNewLD(objectToPrefabs, _localPathAndName);
        }
        //If the name doesn't exist, create the new Prefab
        else
        {
            Debug.Log(objectToPrefabs.name + " is not a Prefab, will convert");
            CreateNewLD(objectToPrefabs, _localPathAndName);
        }
    }

    public static void ChangeNamePrefab(string _localPathAndName, string newName)
    {
        if (AssetDatabase.LoadAssetAtPath(_localPathAndName, typeof(GameObject)))
        {
            Debug.Log("change: " + _localPathAndName + " to " + newName);
            AssetDatabase.RenameAsset(_localPathAndName, newName);

        }
        else
        {
            Debug.Log("can't find " + _localPathAndName);
        }
        AssetDatabase.Refresh();
    }

    public static void DeletePrefab(string _localPathAndName)
    {
        if (AssetDatabase.LoadAssetAtPath(_localPathAndName, typeof(GameObject)))
        {
            AssetDatabase.MoveAssetToTrash(_localPathAndName);
        }
    }

    private static void CreateNewLD(GameObject obj, string localPath)
    {
        //Create a new Prefab at the path given
        UnityEngine.Object prefab = PrefabUtility.SaveAsPrefabAsset(obj, localPath);
        //PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    public static GameObject GetPrefabsFromLocalPath(string _localPath, string name)
    {
        string localPath = _localPath + name + ".prefab";
        GameObject obj = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) as GameObject;
        return (obj);
    }
#endif

}
