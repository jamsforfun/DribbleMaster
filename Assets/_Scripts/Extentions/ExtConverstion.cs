using System;
using UnityEngine;

/// <summary>
/// Extensions for converting one data type to another
/// </summary>
public static class ExtConverstion
{
    #region IntTo

    /// <summary>
    /// Converts an int to float
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float ToFloat(this int value)
    {
        return (float) value;
    }

    /// <summary>
    /// Converts an int to decimal
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal ToDecimal(this int value)
    {
        return (decimal) value;
    }

    /// <summary>
    /// Converts an int to a char
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static char ToChar(this int value)
    {
        return Convert.ToChar(value);
    }

    // IntTo

    #endregion

    #region StringTo

    #region ToInt

    /// <summary>
    /// Converts a string to an int
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static int ToInt(this string value, int defaultValue = 0)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        int rVal;
        return int.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToInt

    #endregion

    #region ToIntNull

    /// <summary>
    /// Converts a string to a nullable int
    /// </summary>
    /// <param name="value">value to convert</param>
    public static int? ToIntNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        int rVal;
        return int.TryParse(value, out rVal) ? rVal : new int?();
    }

    // ToIntNull

    #endregion

    #region ToLong

    /// <summary>
    /// Converts a string to an long
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static long ToLong(this string value, long defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        long rVal;
        return long.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToLong

    #endregion

    #region ToLongNull

    /// <summary>
    /// Converts a string to a nullable long
    /// </summary>
    /// <param name="value">value to convert</param>
    public static long? ToLongNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        long rVal;
        return long.TryParse(value, out rVal) ? rVal : new long?();
    }

    // ToLongNull

    #endregion

    #region ToDecimal

    /// <summary>
    /// Converts a string to a decimal
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static decimal ToDecimal(this string value, decimal defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        decimal rVal;
        return decimal.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToDecimal

    #endregion

    #region ToDouble

    /// <summary>
    /// Converts a string to a decimal
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static double ToDouble(this string value, double defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        double rVal;
        return double.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToDouble

    #endregion

    #region ToDecimalNull

    /// <summary>
    /// Converts a string to a nullable decimal
    /// </summary>
    /// <param name="value">value to convert</param>
    public static decimal? ToDecimalNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        decimal rVal;
        return decimal.TryParse(value, out rVal) ? rVal : new decimal?();
    }

    // ToDecimalNull

    #endregion

    #region ToFloat

    /// <summary>
    /// Converts a string to a float
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static float ToFloat(this string value, float defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        float rVal;
        return float.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToFloat

    #endregion

    #region ToFloatNull

    /// <summary>
    /// Converts a string to a nullable float
    /// </summary>
    /// <param name="value">value to convert</param>
    public static float? ToFloatNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        float rVal;
        return float.TryParse(value, out rVal) ? rVal : new float?();
    }

    // ToFloatNull

    #endregion

    #region ToBool

    /// <summary>
    /// Converts a string to a bool
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static bool ToBool(this string value, bool defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        bool rVal;
        return bool.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToBool

    #endregion

    #region ToBoolNull

    /// <summary>
    /// Converts a string to a nullable bool
    /// </summary>
    /// <param name="value">value to convert</param>
    public static bool? ToBoolNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        bool rVal;
        return bool.TryParse(value, out rVal) ? rVal : new bool?();
    }

    // ToBoolNull

    #endregion

    #region ToDateTime

    /// <summary>
    /// Converts a string to a datetime
    /// </summary>
    /// <param name="value">value to convert</param>
    /// <param name="defaultValue">default value if could not convert</param>
    public static DateTime ToDateTime(this string value, DateTime defaultValue)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        // convert
        DateTime rVal;
        return DateTime.TryParse(value, out rVal) ? rVal : defaultValue;
    }

    // ToDateTime

    #endregion

    #region ToDateTimeNull

    /// <summary>
    /// Converts a string to a nullable date
    /// </summary>
    /// <param name="value">the value to convert</param>
    public static DateTime? ToDateTimeNull(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        DateTime tempDate;
        return (DateTime.TryParse(value, out tempDate)) ? tempDate : new DateTime?();
    }

    // ToDateTimeNull

    #endregion

    #region ToGuid

    /// <summary>
    /// Converts a string to a nullable Guid
    /// </summary>
    /// <param name="gString"></param>
    public static Guid? ToGuid(this string gString)
    {
        try
        {
            return new Guid(gString);
        }

        catch (Exception)
        {
            return null;
        }
    }

    // ToGuid

    #endregion

    #region ToDoubleNullable

    /// <summary>
    /// Returns a nullable double
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double? ToDoubleNullable(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        double rVal;
        return double.TryParse(value, out rVal) ? rVal : new double?();
    }

    // ToDoubleNullable

    #endregion

    #region ToDecimalNullable

    /// <summary>
    /// Returns a nullable decimal
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal? ToDecimalNullable(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        decimal rVal;
        return decimal.TryParse(value, out rVal) ? rVal : new decimal?();
    }

    // ToDecimalNullable

    #endregion

    #region ToIntNullable

    /// <summary>
    /// Returns a nullable int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int? ToIntNullable(this string value)
    {
        // exit if null
        if (string.IsNullOrEmpty(value))
            return null;

        // convert
        int rVal;
        return int.TryParse(value, out rVal) ? rVal : new int?();
    }

    // ToIntNullable

    #endregion

    // StringTo

    #endregion

    #region ByteTo

    /// <summary>
    /// Converts a byte to an int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt(this byte value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    /// Converts a byte to a long
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long ToLong(this byte value)
    {
        return Convert.ToInt64(value);
    }

    /// <summary>
    /// Converts a byte to a double
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double ToDouble(this byte value)
    {
        return Convert.ToDouble(value);
    }

    // ByteTo

    #endregion

    #region BytesTo

    /// <summary>
    /// Converts a byte array to a base64 string
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ToBase64(this byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    /// <summary>
    /// Converts a bas64 string to a byte array
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] FromBase64(this string data)
    {
        return Convert.FromBase64String(data);
    }

    // BytesTo

    #endregion

    #region GenericConversions

    // Credit: http://www.smokingcode.com/2008/05/extension-methods.html

    /*
        string a = myInt.ToOrDefault<string>();
        //note type inference
        DateTime d = myString.ToOrOther(DateTime.MAX_VALUE);
        double d;
        //note type inference
        bool didItGiveDefault = myString.ToOrDefault(d);
        string s = myDateTime.ToOrNull<string>();    
     */

    #region To

    /// <summary>
    /// Converts to a new type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T To <T>(this IConvertible obj)
    {
        return (T) Convert.ChangeType(obj, typeof(T));
    }

    // To

    #endregion

    #region ToOrDefault

    /// <summary>
    /// Converts to the new type, or returns default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T ToOrDefault <T>(this IConvertible obj)
    {
        try
        {
            return To<T>(obj);
        }
        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
                return default(T);

            // everything else gets rethrown
            throw;
        }
    }

    /// <summary>
    /// Returns true if the conversion was successful
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="newObj"></param>
    /// <returns></returns>
    public static bool ToOrDefault <T>(this IConvertible obj, out T newObj)
    {
        try
        {
            newObj = To<T>(obj);
            return true;
        }

        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
            {
                newObj = default(T);
                return false;
            }

            // everything else gets rethrown
            throw;
        }
    }

    // ToOrDefault

    #endregion

    #region ToOrOther

    /// <summary>
    /// Converts to obj. If fails to convert, then converts to other type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static T ToOrOther <T>(this IConvertible obj, T other)
    {
        try
        {
            return To<T>(obj);
        }
        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
                return other;

            // everything else gets rethrown
            throw;
        }
    }

    /// <summary>
    /// Converts to obj. If fails to convert, then converts to other type. Returns true if conversion was successful
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="newObj"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool ToOrOther <T>(this IConvertible obj, out T newObj, T other)
    {
        try
        {
            newObj = To<T>(obj);
            return true;
        }
        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
            {
                newObj = other;
                return false;
            }

            // everything else gets rethrown
            throw;
        }
    }

    // ToOrOther

    #endregion

    #region ToOrNull

    /// <summary>
    /// Converts to a new object, or returns null if couldn't convert
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T ToOrNull<T> (this IConvertible obj) where T : class
    {
        try
        {
            return To<T>(obj);
        }
        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
                return null;

            // everything else gets rethrown
            throw;
        }
    }

    /// <summary>
    /// Converts to a new object, or to null if couldn't convert. Returns true if conversion was successful
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="newObj"></param>
    /// <returns></returns>
    public static bool ToOrNull<T>(this IConvertible obj, out T newObj) where T : class
    {
        try
        {
            newObj = To<T>(obj);
            return true;
        }
        catch (Exception excep)
        {
            // handle conversion exceptions by returning default
            if (excep is InvalidCastException || excep is ArgumentNullException)
            {
                newObj = null;
                return false;
            }

            // everything else gets rethrown
            throw;
        }
    }

    // ToOrNull
    #endregion

    // GenericConversions

    #endregion

    #region Color

    public static int ToInt(Color color)
    {
        return (Mathf.RoundToInt(color.a * 255) << 24) +
               (Mathf.RoundToInt(color.r * 255) << 16) +
               (Mathf.RoundToInt(color.g * 255) << 8) +
               Mathf.RoundToInt(color.b * 255);
    }

    public static Color ToColor(int value)
    {
        var a = (float)(value >> 24 & 0xFF) / 255f;
        var r = (float)(value >> 16 & 0xFF) / 255f;
        var g = (float)(value >> 8 & 0xFF) / 255f;
        var b = (float)(value & 0xFF) / 255f;
        return new Color(r, g, b, a);
    }

    public static Color ToColor(string value)
    {
        return ToColor(ToInt(value));
    }

    public static Color ToColor(Color32 value)
    {
        return new Color((float)value.r / 255f,
                         (float)value.g / 255f,
                         (float)value.b / 255f,
                         (float)value.a / 255f);
    }

    public static Color ToColor(Vector3 value)
    {

        return new Color((float)value.x,
                         (float)value.y,
                         (float)value.z);
    }

    public static Color ToColor(Vector4 value)
    {
        return new Color((float)value.x,
                         (float)value.y,
                         (float)value.z,
                         (float)value.w);
    }

    public static int ToInt(Color32 color)
    {
        return (color.a << 24) +
               (color.r << 16) +
               (color.g << 8) +
               color.b;
    }

    public static Color32 ToColor32(int value)
    {
        byte a = (byte)(value >> 24 & 0xFF);
        byte r = (byte)(value >> 16 & 0xFF);
        byte g = (byte)(value >> 8 & 0xFF);
        byte b = (byte)(value & 0xFF);
        return new Color32(r, g, b, a);
    }

    public static Color32 ToColor32(string value)
    {
        return ToColor32(ToInt(value));
    }

    public static Color32 ToColor32(Color value)
    {
        return new Color32((byte)(value.r * 255f),
                           (byte)(value.g * 255f),
                           (byte)(value.b * 255f),
                           (byte)(value.a * 255f));
    }

    public static Color32 ToColor32(Vector3 value)
    {

        return new Color32((byte)(value.x * 255f),
                           (byte)(value.y * 255f),
                           (byte)(value.z * 255f), 255);
    }

    public static Color32 ToColor32(Vector4 value)
    {
        return new Color32((byte)(value.x * 255f),
                           (byte)(value.y * 255f),
                           (byte)(value.z * 255f),
                           (byte)(value.w * 255f));
    }

    #endregion
}