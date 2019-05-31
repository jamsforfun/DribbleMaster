using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtTransform
{
    /// <summary>
    /// opposite of up
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 down(this Transform t)
    {
        return -t.up;
    }

    /// <summary>
    /// opposite of right
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 left(this Transform t)
    {
        return -t.right;
    }

    /// <summary>
    /// opposite of forward
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 backward(this Transform t)
    {
        return -t.forward;
    }

    /// <summary>
    /// multiply the local scale of the transfrom
    /// </summary>
    /// <param name="t">current transform</param>
    /// <param name="ratio">ratio to multiply</param>
    /// <returns></returns>
    public static Transform MultiplyLocalScale(this Transform t, float ratio)
    {
        t.localScale *= ratio;
        return (t);
    }
    public static Transform MultiplyLocalScale(this Transform t, Vector3 ratio)
    {
        t.localScale = new Vector3(t.localScale.x * ratio.x, t.localScale.y * ratio.y, t.localScale.z * ratio.z);
        return (t);
    }

    /// <summary>
    /// get real pos world of rect transform
    /// </summary>
    public static Rect GetWorldRect(RectTransform rt, Vector2 scale)
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }

    /// <summary>
    /// reset pos of a rect transform
    /// use: someRect.ResetPos();
    /// </summary>
    public static RectTransform ResetPos(this RectTransform transform)
    {
        transform.localPosition = Vector3.zero;
        return (transform);
    }

    /// <summary>
    /// reset scale of a rectTransform
    /// use: someRect.ResetScale();
    /// </summary>
    public static RectTransform ResetScale(this RectTransform transform)
    {
        transform.localScale = Vector3.one;
        transform.offsetMin = new Vector2(0, 0);
        transform.offsetMax = new Vector2(0, 0);
        return (transform);
    }

    /// <summary>
    /// Remove every childs in a transform
    /// use: someTransform.ClearChild();
    /// someTransform.ClearChild(true); //can destroy immediatly in editor
    /// </summary>
    public static Transform ClearChild(this Transform transform, bool immediate = false)
	{
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        else if (immediate)
        {
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        return (transform);
	}

    /// <summary>
    /// return a list of all child in a transform (not recursive)
    /// use: List<Transform>() newList = someTransform.GetAllChild();
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static List<Transform> GetAllChild(this Transform transform)
    {
        List<Transform> allChild = new List<Transform>();
        foreach (Transform child in transform)
        {
            allChild.Add(child);
        }
        return (allChild);
    }

    /// <summary>
    /// active all child of this transform
    /// use: someTransform.ActiveAllChild(true);
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="active"></param>
    /// <param name="exeption">if we want a transform to be inefacted</param>
    /// <returns></returns>
    public static Transform ActiveAllChild(this Transform transform, bool active, Transform exeption = null)
    {
        foreach (Transform child in transform)
        {
            if (exeption && child == exeption)
            {
                child.gameObject.SetActive(!active);
            }
            else
            {
                child.gameObject.SetActive(active);
            }
        }
        return (transform);
    }

    /// <summary>
    /// reset a transform
    /// use: someTransform.ResetTransform(true);
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="local">local or global reset ?</param>
    public static void ResetTransform(this Transform trans, bool local)
    {
        if (local)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
        }
        else
        {
            trans.position = Vector3.zero;
            trans.rotation = Quaternion.identity;
        }
        trans.localScale = Vector3.one;
    }

    #region Set XYZ

    /// <summary>
    /// change only X of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void SetX(this Transform transform, float x, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }
    }
    /// <summary>
    /// change only X and Y of transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void SetXY(this Transform transform, float x, float y, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }
        else
        {
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }
    }
    /// <summary>
    /// change only XZ of transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public static void SetXZ(this Transform transform, float x, float z, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(x, transform.position.y, z);
        }
        else
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        }
    }
    /// <summary>
    /// change only Y of transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="y"></param>
    public static void SetY(this Transform transform, float y, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }
    }
    /// <summary>
    /// change only YZ of transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void SetYZ(this Transform transform, float y, float z, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(transform.position.x, y, z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, z);
        }
    }
    /// <summary>
    /// change only Z of transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="z"></param>
    public static void SetZ(this Transform transform, float z, bool global = true)
    {
        if (global)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }
    }

    #endregion

    /// <summary>
    /// Sets the position of a transform's children to zero.
    /// </summary>
    /// <param name="transform">Parent transform.</param>
    /// <param name="recursive">Also reset ancestor positions</param>
    public static void ResetChildPositions(this Transform transform, bool recursive = false)
    {
        foreach (Transform child in transform)
        {
            child.position = Vector3.zero;

            if (recursive)
            {
                child.ResetChildPositions(recursive);
            }
        }
    }

    /// <summary>
    /// Makes the given game objects children of the transform.
    /// use: someTransform.AddChildren(arrayOfChild);
    /// </summary>
    /// <param name="transform">Parent transform.</param>
    /// <param name="children">Game objects to make children.</param>
    public static void AddChildren(this Transform transform, GameObject[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }
    public static void AddChildren(this Transform transform, Component[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }
}
