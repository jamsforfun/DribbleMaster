using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtDictionnary
{
    public static void Query<K, V>(this Dictionary<K, V> self, K key, Action<V> f) where V : UnityEngine.Object
    {
        var v = default(V);
        if (self.TryGetValue(key, out v))
        {
            if ((UnityEngine.Object)v != (UnityEngine.Object)null)
            {
                f(v);
            }
        }
    }

    public static void QueryObject<K, V>(this Dictionary<K, V> self, K key, Action<V> f)
    {
        var v = default(V);
        if (self.TryGetValue(key, out v))
        {
            if (v != null)
            {
                f(v);
            }
        }
    }

    public static IEnumerable<V> ActiveValues<K, V>(this Dictionary<K, V> self) where V : UnityEngine.Component
    {
        return self.Values.Actives();
    }

    public static IEnumerable<V> ActiveObjectValues<K, V>(this Dictionary<K, V> self) where V : UnityEngine.Object
    {
        return self.Values.ActiveObjects();
    }

    public static IEnumerable<T> AsEnumerable<T>(this T o)
    {
        yield return o;
    }

    public static IEnumerable<T> CombineFirst<T>(this IEnumerable<T> enumerable, T element)
    {
        yield return element;
        foreach (var e in enumerable)
        {
            yield return e;
        }
    }

    public static IEnumerable<T> CombineLast<T>(this IEnumerable<T> enumerable, T element)
    {
        foreach (var e in enumerable)
        {
            yield return e;
        }

        yield return element;
    }

    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> f)
    {
        foreach (var e in enumerable)
        {
            f(e);
        }
    }

    public static void EachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> f)
    {
        EachWithIndex(enumerable, 0, f);
    }

    public static void EachWithIndex<T>(this IEnumerable<T> enumerable, int start, Action<T, int> f)
    {
        var list = new List<T>(enumerable);
        var l = list.Count;
        for (var i = 0; i < l; ++i)
        {
            f(list[i], start + i);
        }
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
    {
        return self.OrderBy(e => e, new RandomComparer<T>());
    }

    class RandomComparer<T> : IComparer<T>
    {
        public int Compare(T a, T b)
        {
            return 1 - 2 * UnityEngine.Random.Range(0, 1);
        }
    }

    public static T Sample<T>(this IEnumerable<T> self)
    {
        return self.ElementAt(UnityEngine.Random.Range(0, self.Count()));
    }

    public static bool IsEmpty<T>(this IEnumerable<T> self)
    {
        return !self.Any();
    }

    public static IEnumerable<T> ActiveObjects<T>(this IEnumerable<T> self) where T : UnityEngine.Object
    {
        return self.Where(e => (UnityEngine.Object)e != (UnityEngine.Object)null);
    }

    public static IEnumerable<T> Actives<T>(this IEnumerable<T> self) where T : UnityEngine.Component
    {
        return self.Where(e => (UnityEngine.Object)e != (UnityEngine.Object)null && e.gameObject != null);
    }
}
