using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Borodar.RainbowHierarchy;

public class UtilityEditor : ScriptableObject
{
    public static T GetScript<T>()
    {
        object obj = UnityEngine.Object.FindObjectOfType(typeof(T));

        if (obj != null)
        {
            return ((T)obj);
            //gameManager = (GameManager)obj;
            //gameManager.indexSaveEditorTmp = gameManager.saveManager.GetMainData().GetLastMapSelectedIndex();
        }

        return (default(T));
    }
    public static T[] GetScripts<T>()
    {
        object[] obj = UnityEngine.Object.FindObjectsOfType(typeof(T));
        T[] objType = new T[obj.Length];


        if (obj != null)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                objType[i] = (T)obj[i];
            }
        }

        return (objType);
    }

    public static void ViewportPanZoomIn(float zoom = 5f)
    {
        //Debug.Log(SceneView.lastActiveSceneView.size);
        if (SceneView.lastActiveSceneView.size > zoom)
            SceneView.lastActiveSceneView.size = zoom;
        SceneView.lastActiveSceneView.Repaint();
    }

    public static void FocusOnSelection(GameObject objToFocus, float zoom = 5f)
    {
        SceneView.lastActiveSceneView.LookAt(objToFocus.transform.position);
        if (zoom != -1)
            ViewportPanZoomIn(zoom);
    }

    public static bool PingAndSelect(GameObject obj)
    {
        if (obj == null)
            return (false);

        EditorGUIUtility.PingObject(obj);
        Selection.activeObject = obj;
        return (true);
    }

    /// <summary>
    /// change the visual of the editor config for this object
    /// </summary>
    /// <param name="selectedObject"></param>
    /// <param name="iconType"></param>
    /// <param name="_IsIconRecursive"></param>
    /// <param name="coreBackground"></param>
    /// <param name="_IsBackgroundRecursive"></param>
    public static void AddCustomEditorToObject(GameObject selectedObject, bool create = true,
        HierarchyIcon iconType = HierarchyIcon.None,
        bool _IsIconRecursive = false,
        Borodar.RainbowCore.CoreBackground coreBackground = Borodar.RainbowCore.CoreBackground.None,
        bool _IsBackgroundRecursive = false)
    {
        GameObject hierarchy = GameObject.Find("RainbowHierarchyConf");
        HierarchySceneConfig hierarchySceneConfig = hierarchy.GetComponent<HierarchySceneConfig>();
        if (hierarchySceneConfig)
        {
            HierarchyItem newItem = hierarchySceneConfig.GetItem(selectedObject);
            if (newItem == null)
            {
                if (!create)
                    return;

                newItem = new HierarchyItem(HierarchyItem.KeyType.Object, selectedObject, selectedObject.name)
                {
                    IconType = HierarchyIcon.None,
                    IsIconRecursive = false,
                    BackgroundType = Borodar.RainbowCore.CoreBackground.ClrIndigo,
                    IsBackgroundRecursive = false,
                };
                hierarchySceneConfig.AddItem(newItem);
            }
            else
            {
                if (!create)
                {
                    hierarchySceneConfig.RemoveAll(selectedObject, HierarchyItem.KeyType.Object);
                }
                else
                {
                    hierarchySceneConfig.RemoveAll(selectedObject, HierarchyItem.KeyType.Object);
                    newItem = new HierarchyItem(HierarchyItem.KeyType.Object, selectedObject, selectedObject.name)
                    {
                        IconType = HierarchyIcon.None,
                        IsIconRecursive = false,
                        BackgroundType = Borodar.RainbowCore.CoreBackground.ClrIndigo,
                        IsBackgroundRecursive = false,
                    };
                    hierarchySceneConfig.AddItem(newItem);
                }
            }
        }
    }
}