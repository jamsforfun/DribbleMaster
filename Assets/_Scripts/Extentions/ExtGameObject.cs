using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class ExtGameObject
{
    /// <summary>
    /// Find a GameObject even if it's disabled.
    /// </summary>
    /// <param name="name">The name.</param>
    public static GameObject FindWithDisabled(this GameObject go, string name)
    {
        var temp = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        var obj = new GameObject();
        foreach (GameObject o in temp)
        {
            if (o.name == name)
            {
                obj = o;
            }
        }
        return obj;
    }


    #region is parent child
    public static bool IsParentOf(this GameObject parent, GameObject possibleChild)
    {
        if (parent == null || possibleChild == null) return false;
        return possibleChild.transform.IsChildOf(parent.transform);
    }

    public static bool IsParentOf(this Transform parent, GameObject possibleChild)
    {
        if (parent == null || possibleChild == null) return false;
        return possibleChild.transform.IsChildOf(parent);
    }

    public static bool IsParentOf(this GameObject parent, Transform possibleChild)
    {
        if (parent == null || possibleChild == null) return false;
        return possibleChild.IsChildOf(parent.transform);
    }

    public static bool IsParentOf(this Transform parent, Transform possibleChild)
    {
        if (parent == null || possibleChild == null) return false;
        /*
         * Since implementation of this, Unity has since added 'IsChildOf' that is far superior in efficiency
         * 
        while (possibleChild != null)
        {
            if (parent == possibleChild.parent) return true;
            possibleChild = possibleChild.parent;
        }
        return false;
        */

        return possibleChild.IsChildOf(parent);
    }

    public static bool IsChildOf(this GameObject go, Transform potentialParent)
    {
        return (go.transform.IsChildOf(potentialParent));
    }
    public static bool IsChildOf(this GameObject go, GameObject potentialParent)
    {
        return (go.transform.IsChildOf(potentialParent.transform));        
    }
    public static bool IsChildOf(this Transform go, GameObject potentialParent)
    {
        return (go.IsChildOf(potentialParent.transform));
    }
    #endregion

    /// <summary>
    /// create a gameObject, with a set of components
    /// ExtGameObject.CreateGameObject("game object name", Transform parent, Vector3.zero, Quaternion.identity, Vector3 Vector.One, Component [] components)
    /// set null at components if no component to add
    /// return the created gameObject
    /// </summary>
    public static GameObject CreateLocalGameObject(string name,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        Component [] components)
    {
        GameObject newObject = new GameObject(name);
        //newObject.SetActive(true);
        newObject.transform.SetParent(parent);
        newObject.transform.SetAsLastSibling();
        newObject.transform.localPosition = localPosition;
        newObject.transform.localRotation = localRotation;
        newObject.transform.localScale = localScale;

        if (components != null)
        {
            for (int i = 0; i < components.Length; i++)
            {
                newObject.AddComponent(components[i]);
            }
        }
        return (newObject);
    }

    public static GameObject CreateObjectFromPrefab(GameObject prefabs, Vector3 position, Quaternion rotation, Transform parent)
    {
        return (GameObject.Instantiate(prefabs, position, rotation, parent));
    }

    /// <summary>
    /// hide all renderer
    /// </summary>
    /// <param name="toHide">apply this to a transform, or a gameObject</param>
    /// <param name="hide">hide (or not)</param>
    public static void HideAllRenderer(this Transform toHide, bool hide = true)
    {
        HideAllRenderer(toHide.gameObject, hide);
    }
    public static void HideAllRenderer(this GameObject toHide, bool hide = true)
    {
        Renderer[] allrenderer = toHide.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < allrenderer.Length; i++)
        {
            allrenderer[i].enabled = !hide;
        }
    }
    public static void HideAllComponentOfType<T>(this GameObject toHide, bool hide = true)
        where T : Behaviour
    {
        T[] allrenderer = toHide.GetComponentsInChildren<T>();

        for (int i = 0; i < allrenderer.Length; i++)
        {
            allrenderer[i].enabled = !hide;
        }
    }

    /// <summary>
    /// Returns true if the GO is null or inactive
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static bool IsNullOrInactive(this GameObject go)
    {
        return ((go == null) || (!go.activeSelf));
    }

    /// <summary>
    /// Returns true if the GO is not null and is active
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static bool IsActive(this GameObject go)
    {
        return ((go != null) && (go.activeSelf));
    }


    /// <summary>
    /// change le layer de TOUT les enfants
    /// </summary>
    //use: myButton.gameObject.SetLayerRecursively(LayerMask.NameToLayer(“UI”));
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetLayerRecursively(layer);
        }            
    }
    /// <summary>
    /// change le layer de TOUT les enfants
    /// </summary>
    //use: myButton.gameObject.SetLayerRecursively(LayerMask.NameToLayer(“UI”));
    public static void SetStaticRecursively(this GameObject gameObject, bool isStatic)
    {
        gameObject.isStatic = isStatic;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetStaticRecursively(isStatic);
        }
    }

    /// <summary>
    /// activate recursivly the Colliders
    /// use: gameObject.SetCollisionRecursively(false);
    /// </summary>
    public static void SetCollisionRecursively(this GameObject gameObject, bool tf)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = tf;
        }            
    }

    /// <summary>
    /// but what if you want the collision mask to be based on the weapon’s layer?
    /// It’d be nice to set some weapons to “Team1” and others to “Team2”,
    /// perhaps, and also to ensure your code doesn’t break if you change
    /// the collision matrix in the project’s Physics Settings
    /// 
    /// USE:
    /// if(Physics.Raycast(startPosition, direction, out hitInfo, distance,
    ///                          weapon.gameObject.GetCollisionMask()) )
    ///{
    ///    // Handle a hit
    ///}
    ///
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static int GetCollisionMask(this GameObject gameObject, int layer = -1)
    {
        if (layer == -1)
            layer = gameObject.layer;

        int mask = 0;
        for (int i = 0; i < 32; i++)
            mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;

        return mask;
    }

    /// <summary>
    /// is the object's layer in the specified layermask
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
    {
        return ((mask.value & (1 << gameObject.layer)) > 0);
    }

    /// <summary>
    /// Is a gameObject grounded or not ?
    /// </summary>
    /// <param name="target">object to test for grounded</param>
    /// <param name="dirUp">normal up of the object</param>
    /// <param name="distToGround">dist to test</param>
    /// <param name="layerMask">layermask</param>
    /// <param name="queryTriggerInteraction"></param>
    /// <param name="marginDistToGround">aditionnal margin to the distance</param>
    /// <returns></returns>
    public static bool IsGrounded(GameObject target, Vector3 dirUp, float distToGround, int layerMask, QueryTriggerInteraction queryTriggerInteraction, float marginDistToGround = 0.1f)
    {
        return Physics.Raycast(target.transform.position, -dirUp, distToGround + marginDistToGround, layerMask, queryTriggerInteraction);
    }
}
