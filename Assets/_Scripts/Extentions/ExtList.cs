using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtList
{
    /// <summary>
    /// is this list contain a string (nice for enum test)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="listEnum"></param>
    /// <returns></returns>
    public static bool ListContain<T>(this List<T> listEnum, string tag)
    {
        for (int i = 0; i < listEnum.Count; i++)
        {
            if (String.Equals(listEnum[i].ToString(), tag))
                return (true);
        }
        return (false);
    }

    /// <summary>
    /// add the item in the lsit if it's not already in the lsit
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="listEnum">list</param>
    /// <param name="item">item to add</param>
    /// <returns>return false if already in the list, true if we add successfuly in the list</returns>
    public static bool AddIfNotContain<T>(this List<T> listEnum, T item)
    {
        if (item == null)
        {
            return (false);
        }
        if (listEnum.Contains(item))
        {
            return (false);
        }
        listEnum.Add(item);
        return (true);
    }

    /// <summary>
    /// true return if in the list, there is a word that is substring of the fileName
    /// </summary>
    /// <param name="toTransform"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static int ContainSubStringInList(List<string> toTransform, string fileName)
    {
        for (int i = 0; i < toTransform.Count; i++)
        {
            if (fileName.Contains(toTransform[i]))
                return (i);
        }
        return (-1);
    }

    

    /// <summary>
    /// return a list of all child of this transform (depth 1 only)
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<Transform> GetListFromChilds(this Transform parent)
    {
        List<Transform> allChild = new List<Transform>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            allChild.Add(parent.transform.GetChild(i));
        }
        return (allChild);
    }

    /// <summary>
    /// move an item in a list, from oldIndex to newIndex
    /// </summary>
    public static List<T> Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        if (list == null)
            return (null);
        if (list.Count == 0)
            return (list);
        if (oldIndex >= list.Count || oldIndex < 0)
            return (list);
        if (newIndex >= list.Count || newIndex < 0)
            return (list);

        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
        return (list);
    }

    /// <summary>
    /// bubble sort optimize algorythme
    /// </summary>
    public static List<float> BubbleSort(this List<float> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            bool sorted = true;
            for (int j = 0; j <= i - 1; j++)
            {
                if (list[j + 1] < list[j])
                {
                    list.Move(j + 1, j);
                    sorted = false;
                }
            }
            if (sorted)
                break;
        }
        return (list);
    }

    /// <summary>
    /// transform an array into a list
    /// </summary>
    public static List<T> ToList<T>(T[] array)
    {
        if (array == null)
            return (null);

        List<T> newList = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            newList.Add(array[i]);
        }
        return (newList);
    }

    /// <summary>
    /// Shuffle the list in place using the Fisher-Yates method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        list.Sort((a, b) => 1 - 2 * UnityEngine.Random.Range(0, 1));
    }

    /// <summary>
    /// Return a random item from the list.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Removes a random item from the list, returning that item.
    /// Sampling without replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    /// <summary>
    /// Returns true if the array is null or empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this T[] data)
    {
        return ((data == null) || (data.Length == 0));
    }

    /// <summary>
    /// Returns true if the list is null or empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this List<T> data)
    {
        return ((data == null) || (data.Count == 0));
    }

    /// <summary>
    /// Returns true if the dictionary is null or empty
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> data)
    {
        return ((data == null) || (data.Count == 0));
    }

    /// <summary>
    /// deques an item, or returns null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="q"></param>
    /// <returns></returns>
    public static T DequeueOrNull<T>(this Queue<T> q)
    {
        try
        {
            return (q.Count > 0) ? q.Dequeue() : default(T);
        }

        catch (Exception)
        {
            return default(T);
        }
    }

    /// <summary>
    /// Add in a list some new items
    /// </summary>
    public static void AddRange<T>(this IList<T> list, IList<T> items, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < items.Count && i < endIndex; i++)
        {
            list.Add(items[i]);
        }
    }
}
