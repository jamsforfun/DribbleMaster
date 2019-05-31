using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ExtComponent
{
    #region GetInterfaces
    /// <summary>
    /// Returns the interface on this game object
    /// </summary>
    public static T GetInterface<T>(this GameObject go) where T : class
    {

        if (!typeof(T).IsInterface)
        {
            Debug.LogError(typeof(T).ToString() + " is not an interface");
            return null;
        }

        return go.GetComponents<Component>().OfType<T>().FirstOrDefault();
    }
    /// <summary>
    /// Returns the first matching interface on this game object's children
    /// </summary>
    public static T GetInterfaceInChildren<T>(this GameObject go) where T : class
    {

        if (!typeof(T).IsInterface)
        {
            Debug.LogError(typeof(T).ToString() + " is not an interface");
            return null;
        }

        return go.GetComponentsInChildren<Component>().OfType<T>().FirstOrDefault();
    }
    /// <summary>
    /// Returns all interfaces on this game object matching this type
    /// </summary>
    public static IEnumerable<T> GetInterfaces<T>(this GameObject go) where T : class
    {

        if (!typeof(T).IsInterface)
        {
            Debug.LogError(typeof(T).ToString() + " is not an interface");
            return Enumerable.Empty<T>();
        }

        return go.GetComponents<Component>().OfType<T>();
    }
    /// <summary>
    /// Returns all matching interfaces on this game object's children
    /// </summary>
    public static IEnumerable<T> GetInterfacesInChildren<T>(this GameObject go) where T : class
    {
        if (!typeof(T).IsInterface)
        {
            Debug.LogError(typeof(T).ToString() + " is not an interface");
            return Enumerable.Empty<T>();
        }

        return go.GetComponentsInChildren<Component>(true).OfType<T>();
    }
    #endregion

    #region Add Copy or Delete

    /// <summary>
    /// Gets or add a component. Usage example:
    /// use: BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        T result = component.GetComponent<T>();
        if (result == null)
        {
            result = component.gameObject.AddComponent<T>();
        }
        return result;
    }
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T result = obj.GetComponent<T>();
        if (result == null)
        {
            result = obj.AddComponent<T>();
        }
        return result;
    }

    /// <summary>
    /// add a component to an object
    /// </summary>
    public static T AddComponent<T>(this Component go, T toAdd) where T : Component
    {
        return go.gameObject.AddComponent<T>().GetCopyOf(toAdd) as T;
    }
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    /// <summary>
    /// copy a component into another gameObject
    /// use: 
    /// BoxCollider box = gameObject1.GetOrAddComponent<BoxCollider>();
    /// ExtComponent.CopyComponent(box, gameObject2);
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    /// <summary>
    /// copy an component into the one selected
    /// use:
    /// BoxCollider box = gameObject1.GetOrAddComponent<BoxCollider>();
    /// gameObject2.GetCopyOf(box);
    /// </summary>
    /// <param name="other">the component to copy</param>
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }
    public static T GetCopyOf<T>(this GameObject comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    /// <summary>
	/// Adds all the components found on a resource prefab.
	/// </summary>
	/// <param name='inst'>
	/// Instance of game object to add the components to
	/// </param>
	/// <param name='path'>
	/// Path of prefab relative to ANY resource folder in the assets directory
	/// </param>
	/// 
	public static void AddComponentsFromResource(this GameObject inst, string path)
    {
        var go = Resources.Load(path) as GameObject;

        foreach (var src in go.GetComponents<Component>())
        {
            var dst = inst.AddComponent(src.GetType()) as Behaviour;
            dst.enabled = false;
            //ComponentUtil.Copy(dst, src);
            dst.enabled = true;
        }
    }

    /// <summary>
    /// Destroy component if present
    /// </summary>
    /// <returns>return true if object found and destroyed</returns>
    public static bool DestroyComponent<T>(this Component child, bool immediate = false) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result != null)
        {
            if (immediate)
            {
                GameObject.DestroyImmediate(child.gameObject.GetComponent<T>());
            }
            else
            {
                GameObject.Destroy(child.gameObject.GetComponent<T>());
            }
            return (true);
        }
        return false;
    }

    /// <summary>
    /// destroy all component inside that object
    /// use: gameObject.DestroyAllComponentInside<Renderer>();
    /// use: transform.DestroyAllComponentInside<Renderer>();
    /// use: transform.DestroyAllComponentInside<Renderer>(true, false);
    /// </summary>
    /// <param name="imediate">type of destruction: imediate, or normal</param>
    /// /// <param name="includeInactive">destroy also in inactive object</param>
    public static void DestroyAllComponentInside<T>(this Component child, bool immediate = false, bool includeInactive = false) where T : Component
    {
        DestroyAllComponentInside<T>(child.gameObject);
    }
    public static void DestroyAllComponentInside<T>(this GameObject child, bool immediate = false, bool includeInactive = false) where T : Component
    {
        T[] allComponent = child.GetComponentsInChildren<T>(includeInactive);

        for (int i = 0; i < allComponent.Length; i++)
        {
            if (immediate)
            {
                GameObject.DestroyImmediate(allComponent[i]);
            }
            else
            {
                GameObject.Destroy(allComponent[i]);
            }
        }
    }
    public static void DisableAllComponentInside<T>(this Component child, bool includeInactive = false) where T : Behaviour
    {
        DisableAllComponentInside<T>(child.gameObject);
    }
    public static void DisableAllComponentInside<T>(this GameObject child, bool includeInactive = false) where T : Behaviour
    {
        T[] allComponent = child.GetComponentsInChildren<T>(includeInactive);

        for (int i = 0; i < allComponent.Length; i++)
        {
            allComponent[i].enabled = false;
        }
    }

    #endregion

    #region Get Childrens
    /// <summary>
    /// Get a component of type T on any of the children recursivly
    /// use:
    /// Player player = gameObject.GetExtComponentInChildrens<Player>();
    /// Player player = gameObject.GetExtComponentInChildrens<Player>(4, true);
    /// Player[] player = gameObject.GetExtComponentInChildrens<Player>();
    /// Player[] player = gameObject.GetExtComponentInChildrens<Player>(4, true);
    /// </summary>
    public static T GetExtComponentInChildrens<T>(this Component component, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentInChildrens<T>(component.gameObject, depth, startWithOurSelf));
    }
    public static T GetExtComponentInChildrens<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        if (startWithOurSelf)
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                return (null);
            return (t.gameObject.GetExtComponentInChildrens<T>(depth - 1, true));
        }
        return (null);
    }
    public static T[] GetExtComponentsInChildrens<T>(this Component component, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentsInChildrens<T>(component.gameObject, depth, startWithOurSelf));
    }
    public static T[] GetExtComponentsInChildrens<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        List<T> results = new List<T>();
        if (startWithOurSelf)
        {
            T[] result = gameObject.GetComponents<T>();
            for (int i = 0; i < result.Length; i++)
            {
                results.Add(result[i]);
            }
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                break;
            results.AddRange(t.gameObject.GetExtComponentsInChildrens<T>(depth - 1, true));
        }

        return results.ToArray();
    }

    public static T GetExtComponentInChildrensWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentInChildrensWithTag<T>(component.gameObject, tag, depth, startWithOurSelf));
    }
    public static T GetExtComponentInChildrensWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        if (startWithOurSelf && gameObject.CompareTag(tag))
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                return (null);
            return (t.gameObject.GetExtComponentInChildrensWithTag<T>(tag, depth - 1, true));
        }
        return (null);
    }
    public static T[] GetExtComponentsInChildrensWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentsInChildrensWithTag<T>(component.gameObject, tag, depth, startWithOurSelf));
    }
    public static T[] GetExtComponentsInChildrensWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        List<T> results = new List<T>();
        if (startWithOurSelf && gameObject.CompareTag(tag))
        {
            T[] result = gameObject.GetComponents<T>();
            for (int i = 0; i < result.Length; i++)
            {
                results.Add(result[i]);
            }
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                break;
            results.AddRange(t.gameObject.GetExtComponentsInChildrensWithTag<T>(tag, depth - 1, true));
        }

        return results.ToArray();
    }

    public static T GetExtComponentInChildrensWithLayer<T>(this Component component, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentInChildrensWithLayer<T>(component.gameObject, layer, depth, startWithOurSelf));
    }
    public static T GetExtComponentInChildrensWithLayer<T>(this GameObject gameObject, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        if (startWithOurSelf && gameObject.layer == layer)
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                return (null);
            return (t.gameObject.GetExtComponentInChildrensWithLayer<T>(layer, depth - 1, true));
        }
        return (null);
    }
    public static T[] GetExtComponentsInChildrensWithLayer<T>(this Component component, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentsInChildrensWithLayer<T>(component.gameObject, layer, depth, startWithOurSelf));
    }
    public static T[] GetExtComponentsInChildrensWithLayer<T>(this GameObject gameObject, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        List<T> results = new List<T>();
        if (startWithOurSelf && gameObject.layer == layer)
        {
            T[] result = gameObject.GetComponents<T>();
            for (int i = 0; i < result.Length; i++)
            {
                results.Add(result[i]);
            }
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                break;
            results.AddRange(t.gameObject.GetExtComponentsInChildrensWithLayer<T>(layer, depth - 1, true));
        }

        return results.ToArray();
    }

    public static T GetExtComponentInChildrensWithTagAndLayer<T>(this Component component, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentInChildrensWithTagAndLayer<T>(component.gameObject, tag, layer, depth, startWithOurSelf));
    }
    public static T GetExtComponentInChildrensWithTagAndLayer<T>(this GameObject gameObject, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        if (startWithOurSelf && gameObject.CompareTag(tag) && gameObject.layer == layer)
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                return (null);
            return (t.gameObject.GetExtComponentInChildrensWithTagAndLayer<T>(tag, layer, depth - 1, true));
        }
        return (null);
    }
    public static T[] GetExtComponentsInChildrensWithTagAndLayer<T>(this Component component, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        return (GetExtComponentsInChildrensWithTagAndLayer<T>(component.gameObject, tag, layer, depth, startWithOurSelf));
    }
    public static T[] GetExtComponentsInChildrensWithTagAndLayer<T>(this GameObject gameObject, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false)
        where T : Component
    {
        List<T> results = new List<T>();
        if (startWithOurSelf && gameObject.CompareTag(tag) && gameObject.layer == layer)
        {
            T[] result = gameObject.GetComponents<T>();
            for (int i = 0; i < result.Length; i++)
            {
                results.Add(result[i]);
            }
        }

        foreach (Transform t in gameObject.transform)
        {
            if (depth - 1 <= 0)
                break;
            results.AddRange(t.gameObject.GetExtComponentsInChildrensWithTagAndLayer<T>(tag, layer, depth - 1, true));
        }

        return results.ToArray();
    }

    #endregion

    #region Has component

    /// <summary>
    /// Checks whether a component's game object has a component of type T attached
    /// use:
    /// gameObject.HadComponent<Renderer>();
    /// gameObject.HadComponent<Renderer>(3, true);
    /// gameObject.HadComponent<Renderer>("tag");
    /// gameObject.HadComponent<Renderer>(LayerMask.NameToLayer("layer"));
    /// gameObject.HadComponent<Renderer>(LayerMask.NameToLayer("layer"), "tag");
    /// gameObject.HadComponent<Renderer>(LayerMask.NameToLayer("layer"), "tag", 5, true);
    /// </summary>
    /// <param name="component">Component.</param>
    /// <param name="depth">1: first depth, 2: second depth</param>
    /// <param name="startWithOurself">do we test ourself or not ?</param>
    /// <returns>True when component is attached.</returns>
    public static bool HasComponent<T>(this Component component) where T : Component
    {
        return component.GetComponent<T>() != null;
    }
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static bool HasComponentInParents<T>(this Component component, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInParents<T>(component, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInParents<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInParents<T>(gameObject, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInParentsWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInParentsWithTag<T>(component, tag, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInParentsWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInParentsWithTag<T>(gameObject, tag, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInParentsWithLayer<T>(this Component component, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentsInParentsWithLayer<T>(component, layer, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInParentsWithTag<T>(this GameObject gameObject, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentsInParentsWithLayer<T>(gameObject, layer, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInParentsWithTagAndLayer<T>(this Component component, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentsInParentsWithTagAndLayer<T>(component, tag, layer, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInParentsWithTagAndLayer<T>(this GameObject gameObject, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentsInParentsWithTagAndLayer<T>(gameObject, tag, layer, depth, startWithOurSelf) != null;
    }


    public static bool HasComponentInChildrens<T>(this Component component, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrens<T>(component, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInChildrens<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrens<T>(gameObject, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInChildrensWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithTag<T>(component, tag, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInChildrensWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithTag<T>(gameObject, tag, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInChildrensWithLayer<T>(this Component component, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithLayer<T>(component, layer, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInChildrensWithLayer<T>(this GameObject gameObject, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithLayer<T>(gameObject, layer, depth, startWithOurSelf) != null;
    }

    public static bool HasComponentInChildrensWithTagAndLayer<T>(this Component component, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithTagAndLayer<T>(component, tag, layer, depth, startWithOurSelf) != null;
    }
    public static bool HasComponentInChildrensWithTagAndLayer<T>(this GameObject gameObject, string tag, LayerMask layer, int depth = 99, bool startWithOurSelf = false) where T : Component
    {
        return ExtComponent.GetExtComponentInChildrensWithTagAndLayer<T>(gameObject, tag, layer, depth, startWithOurSelf) != null;
    }

    #endregion

    #region Get Parent
    /// <summary>
    /// Get a component of type T on any of the parents
    /// use:
    /// Player player = gameObject.GetComponentInParents<Player>();
    /// Player player = gameObject.GetComponentInParents<Player>(4, true);
    /// Player[] player = gameObject.GetComponentsInParents<Player>();
    /// Player[] player = gameObject.GetComponentsInParents<Player>(4, true);
    /// 
    /// Player player = gameObject.GetExtComponentInParentsWithTag<Player>("tagOfGameObject");
    /// Player[] player = gameObject.GetExtComponentInParentsWithTag<Player>("tagOfGameObject");
    /// 
    /// Player player = gameObject.GetExtComponentInParentsWithLayer<Player>(LayerMask.NameToLayer("Player"));
    /// Player[] player = gameObject.GetExtComponentInParentsWithLayer<Player>(LayerMask.NameToLayer("Player"));
    /// 
    /// Player player = gameObject.GetExtComponentInParentsWithTagAndLayer<Player>(LayerMask.NameToLayer("Player"), "tagOfGameObject");
    /// Player[] player = gameObject.GetExtComponentInParentsWithTagAndLayer<Player>(LayerMask.NameToLayer("Player"), "tagOfGameObject");
    /// </summary>
    /// <param name="component">Component.</param>
    /// <param name="depth">1: first depth, 2: second depth</param>
    /// <param name="startWithOurself">do we test ourself or not ?</param>
    /// <returns>True when component is attached.</returns>
    public static T GetExtComponentInParents<T>(this Component component, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        return (GetExtComponentInParents<T>(component.gameObject, depth, startWithOurself));
    }
    public static T GetExtComponentInParents<T>(this GameObject gameObject, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        if (startWithOurself)
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
                return result;
            currentDepth++;
            if (currentDepth >= depth)
                return (null);
        }

        return null;
    }
    public static T[] GetExtComponentsInParents<T>(this Component component, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        return (GetExtComponentsInParents<T>(component.gameObject, depth, startWithOurself));
    }
    public static T[] GetExtComponentsInParents<T>(this GameObject gameObject, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        List<T> results = new List<T>();

        if (startWithOurself)
        {
            T[] allComponentOfThisGameObject = gameObject.GetComponents<T>();
            for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
            {
                results.Add(allComponentOfThisGameObject[i]);
            }
        }

        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            T[] allComponentOfThisGameObject = t.GetComponents<T>();
            for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
            {
                results.Add(allComponentOfThisGameObject[i]);
            }

            currentDepth++;
            if (currentDepth >= depth)
                break;
        }

        return results.ToArray();
    }

    public static T GetExtComponentInParentsWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        return (GetExtComponentInParentsWithTag<T>(component.gameObject, tag, depth, startWithOurself));
    }
    public static T GetExtComponentInParentsWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        if (startWithOurself && gameObject.CompareTag(tag))
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.CompareTag(tag))
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    return result;
            }
            currentDepth++;
            if (currentDepth >= depth)
                return (null);
        }
        return null;
    }
    public static T[] GetExtComponentsInParentsWithTag<T>(this Component component, string tag, int depth = 99, bool startWithOurself = false)
    where T : Component
    {
        return (GetExtComponentsInParentsWithTag<T>(component.gameObject, tag, depth, startWithOurself));
    }
    public static T[] GetExtComponentsInParentsWithTag<T>(this GameObject gameObject, string tag, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        List<T> results = new List<T>();

        if (startWithOurself && gameObject.CompareTag(tag))
        {
            T[] allComponentOfThisGameObject = gameObject.GetComponents<T>();
            for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
            {
                results.Add(allComponentOfThisGameObject[i]);
            }
        }
        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.CompareTag(tag))
            {
                T[] allComponentOfThisGameObject = t.GetComponents<T>();
                for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
                {
                    results.Add(allComponentOfThisGameObject[i]);
                }
            }
            currentDepth++;
            if (currentDepth >= depth)
                break;
        }

        return results.ToArray();
    }

    public static T GetExtComponentInParentsWithLayer<T>(this Component component, int layer, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        return (GetExtComponentInParentsWithLayer<T>(component.gameObject, layer, depth, startWithOurself));
    }
    public static T GetExtComponentInParentsWithLayer<T>(this GameObject gameObject, int layer, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        if (startWithOurself && gameObject.layer == layer)
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.gameObject.layer == layer)
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    return result;
            }
            currentDepth++;
            if (currentDepth >= depth)
                return (null);
        }
        return null;
    }
    public static T[] GetExtComponentsInParentsWithLayer<T>(this Component component, LayerMask layer, int depth = 99, bool startWithOurself = false)
    where T : Component
    {
        return (GetExtComponentsInParentsWithLayer<T>(component.gameObject, layer, depth, startWithOurself));
    }
    public static T[] GetExtComponentsInParentsWithLayer<T>(this GameObject gameObject, LayerMask layer, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        List<T> results = new List<T>();

        if (startWithOurself && gameObject.layer == layer)
        {
            T[] allComponentOfThisGameObject = gameObject.GetComponents<T>();
            for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
            {
                results.Add(allComponentOfThisGameObject[i]);
            }
        }
        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.gameObject.layer == layer)
            {
                T[] allComponentOfThisGameObject = t.GetComponents<T>();
                for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
                {
                    results.Add(allComponentOfThisGameObject[i]);
                }
            }
            currentDepth++;
            if (currentDepth >= depth)
                break;
        }

        return results.ToArray();
    }

    public static T GetExtComponentInParentsWithTagAndLayer<T>(this Component component, int layer, string tag, int depth = 99, bool startWithOurself = false)
where T : Component
    {
        return (GetExtComponentInParentsWithTagAndLayer<T>(component.gameObject, layer, tag, depth, startWithOurself));
    }
    public static T GetExtComponentInParentsWithTagAndLayer<T>(this GameObject gameObject, int layer, string tag, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        if (startWithOurself && gameObject.layer == layer && gameObject.CompareTag(tag))
        {
            T result = gameObject.GetComponent<T>();
            if (result != null)
                return (result);
        }

        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.gameObject.layer == layer && t.CompareTag(tag))
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    return result;
            }
            currentDepth++;
            if (currentDepth >= depth)
                return (null);
        }
        return null;
    }
    public static T[] GetExtComponentsInParentsWithTagAndLayer<T>(this Component component, string tag, LayerMask layer, int depth = 99, bool startWithOurself = false)
    where T : Component
    {
        return (GetExtComponentsInParentsWithTagAndLayer<T>(component.gameObject, tag, layer, depth, startWithOurself));
    }
    public static T[] GetExtComponentsInParentsWithTagAndLayer<T>(this GameObject gameObject, string tag, LayerMask layer, int depth = 99, bool startWithOurself = false)
        where T : Component
    {
        List<T> results = new List<T>();

        if (startWithOurself && gameObject.layer == layer && gameObject.CompareTag(tag))
        {
            T[] allComponentOfThisGameObject = gameObject.GetComponents<T>();
            for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
            {
                results.Add(allComponentOfThisGameObject[i]);
            }
        }
        Transform firstParent = gameObject.transform.parent;

        int currentDepth = 0;
        for (Transform t = firstParent; t != null; t = t.parent)
        {
            if (t.gameObject.layer == layer && t.CompareTag(tag))
            {
                T[] allComponentOfThisGameObject = t.GetComponents<T>();
                for (int i = 0; i < allComponentOfThisGameObject.Length; i++)
                {
                    results.Add(allComponentOfThisGameObject[i]);
                }
            }
            currentDepth++;
            if (currentDepth >= depth)
                break;
        }

        return results.ToArray();
    }

    #endregion
}
