using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtColor
{
    /// <summary>
    /// return a color from a string
    /// </summary>
    public static Color ColorHTML(string color)
    {
        Color newCol;
        ColorUtility.TryParseHtmlString(color, out newCol);
        return (newCol);
    }

    /// <summary>
    /// return a color, based on r, g, b, a variable from 0 to 255
    /// use: Color newColor = ExtColor.Color255(25, 255, 0, 1);
    /// </summary>
    /// <param name="r">from 0 to 255</param>
    /// <param name="g">from 0 to 255</param>
    /// <param name="b">from 0 to 255</param>
    /// <param name="a">from 0 to 1</param>
    /// <returns></returns>
    public static Color Color255(float r, float g, float b, float a)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a);
    }

    /// <summary>
    /// only change the alpha of a color
    /// GUI.color = desiredColor.WithAlpha(0.5f);
    /// </summary>
    /// <param name="a">from 0 to 1</param>
    public static Color ChangeAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    /// <summary>
    /// Direct speedup of <seealso cref="Color.Lerp"/>
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Color Lerp(Color c1, Color c2, float value)
    {
        if (value > 1.0f)
            return c2;
        if (value < 0.0f)
            return c1;
        return new Color(c1.r + (c2.r - c1.r) * value,
                         c1.g + (c2.g - c1.g) * value,
                         c1.b + (c2.b - c1.b) * value,
                         c1.a + (c2.a - c1.a) * value);
    }

    /// <summary>
    /// get a random color
    /// </summary>
    /// <returns></returns>
    public static Color GetRandomColor()
    {
        return (ExtRandom.GetRandomColor());
    }

    /// <summary>
    /// get a random color, with alpha 1
    /// </summary>
    /// <returns></returns>
    public static Color GetRandomColorSeed(System.Random randomSeed)
    {
        return (ExtRandom.GetRandomColorSeed(randomSeed));
    }

    public static Color Color(this int n)
    {
        return n.Color32();
    }

    public static Color32 Color32(this int n)
    {
        return new Color32((byte)((n >> 16) & 0xff), (byte)((n >> 8) & 0xff), (byte)((n >> 0) & 0xff), 0xff);
    }

    public static Color R(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color G(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color B(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color A(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    public static Color R(this Color c, Func<float, float> f)
    {
        c.r = f(c.r);
        return c;
    }

    public static Color G(this Color c, Func<float, float> f)
    {
        c.g = f(c.g);
        return c;
    }

    public static Color B(this Color c, Func<float, float> f)
    {
        c.b = f(c.b);
        return c;
    }

    public static Color A(this Color c, Func<float, float> f)
    {
        c.a = f(c.a);
        return c;
    }
}
