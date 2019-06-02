using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtLog
{
    
    /// <summary>
    /// move an item in a list, from oldIndex to newIndex
    /// </summary>
    public static void LogList<T> (List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }
}
