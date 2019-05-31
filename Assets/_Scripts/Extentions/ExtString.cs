using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

public static class ExtString
{
    public enum Alignment
    {
        Left = 0,
        Right = 1,
        Center = 2
    }

    /// <summary>
    /// truncate a string with a defined maxLenght
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Returns true if value is an int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInt(this string value)
    {
        try
        {
            int tempInt;
            return int.TryParse(value, out tempInt);
        }

        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Like LINQ take - takes the first x characters
    /// </summary>
    /// <param name="value">the string</param>
    /// <param name="count">number of characters to take</param>
    /// <param name="ellipsis">true to add ellipsis (...) at the end of the string</param>
    /// <returns></returns>
    public static string Take(this string value, int count, bool ellipsis = false)
    {
        // get number of characters we can actually take
        int lengthToTake = Math.Min(count, value.Length);

        // Take and return
        return (ellipsis && lengthToTake < value.Length) ?
            string.Format("{0}...", value.Substring(0, lengthToTake)) :
            value.Substring(0, lengthToTake);
    }

    /// <summary>
    /// like LINQ skip - skips the first x characters and returns the remaining string
    /// </summary>
    /// <param name="value">the string</param>
    /// <param name="count">number of characters to skip</param>
    /// <returns></returns>
    public static string Skip(this string value, int count)
    {
        return value.Substring(Math.Min(count, value.Length) - 1);
    }

    /// <summary>
    /// Reverses the string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Reverse(this string input)
    {
        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new String(chars);
    }

    /// <summary>
    /// Null or empty check as extension
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// ditches html tags - note it doesn't get rid of things like nbsp;
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string StripHtml(this string html)
    {
        if (html.IsNullOrEmpty())
            return string.Empty;

        return Regex.Replace(html, @"<[^>]*>", string.Empty);
    }

    /// <summary>
    /// Returns true if the pattern matches
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool Match(this string value, string pattern)
    {
        return Regex.IsMatch(value, pattern);
    }

    /// <summary>
    /// Remove white space, not line end
    /// Useful when parsing user input such phone,
    /// price int.Parse("1 000 000".RemoveSpaces(),.....
    /// </summary>
    /// <param name="value"></param>
    public static string RemoveSpaces(this string value)
    {
        return value.Replace(" ", string.Empty);
    }

    /// <summary>
    /// Replaces line endings (\r \n) with &lt;br /&gt;
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ReplaceRNWithBr(this string value)
    {
        return value.Replace("\r\n", "<br />").Replace("\n", "<br />");
    }


    /// <summary>
    /// Converts a null or "" to string.empty. Useful for XML code. Does nothing if <paramref name="value"/> already has a value
    /// </summary>
    /// <param name="value">string to convert</param>
    public static string ToEmptyString(string value)
    {
        return (string.IsNullOrEmpty(value)) ? string.Empty : value;
    }

    /// <summary>
    /// Inverts the case of each character in the given string and returns the new string.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <returns>The converted string.</returns>
    public static string InvertCase(this string s)
    {
        return new string(
            s.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                  char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
    }

    /// <summary>
    /// Checks whether the given string is null, else if empty after trimmed.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <returns>True if string is Null or Empty, false otherwise.</returns>
    public static bool IsNullOrEmptyAfterTrimmed(this string s)
    {
        return (s.IsNullOrEmpty() || s.Trim().IsNullOrEmpty());
    }

    /// <summary>
    /// Extracts the substring starting from 'start' position to 'end' position.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <param name="start">The start position.</param>
    /// <param name="end">The end position.</param>
    /// <returns>The substring.</returns>
    public static string SubstringFromXToY(this string s, int start, int end)
    {
        if (s.IsNullOrEmpty())
            return string.Empty;

        // if start is past the length of the string
        if (start >= s.Length)
            return string.Empty;

        // if end is beyond the length of the string, reset
        if (end >= s.Length)
            end = s.Length - 1;

        return s.Substring(start, end - start);
    }

    /// <summary>
    /// Returns the number of words in the given string.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <returns>The word count.</returns>
    public static int GetWordCount(this string s)
    {
        return (new Regex(@"\w+")).Matches(s).Count;
    }

    /// <summary>
    /// Checks whether the given string is a palindrome.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <returns>True if the given string is palindrome, false otherwise.</returns>
    public static bool IsPalindrome(this string s)
    {
        return s.Equals(s.Reverse());
    }

    /// <summary>
    /// Checks whether the given string is a valid IP address using regular expressions.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <returns>True if it is a valid IP address, false otherwise.</returns>
    public static bool IsValidIPAddress(this string s)
    {
        return Regex.IsMatch(s, @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
    }

    #region Morphing

    public static string ToLower(string value)
    {
        //return (value != null) ? value.ToLower() : null;
        return (value + "").ToLower();
    }

    public static string ToUpper(string value)
    {
        //return (value != null) ? value.ToUpper() : null;
        return (value + "").ToUpper();
    }

    public static string Trim(string value)
    {
        if (value == null) return null;

        return value.Trim();
    }

    public static string[] Split(string value, string delim)
    {
        if (value == null) return null;
        return value.Split(new string[] { delim }, StringSplitOptions.None);
    }

    public static string[] Split(string value, params char[] delim)
    {
        if (value == null) return null;
        return value.Split(delim);
    }

    public static string[] SplitFixedLength(string value, string delim, int len)
    {
        if (value == null) return new string[len];

        string[] arr = value.Split(new string[] { delim }, StringSplitOptions.None);
        if (arr.Length != len) Array.Resize(ref arr, len);
        return arr;
    }

    public static string[] SplitFixedLength(string value, char delim, int len)
    {
        if (value == null) return new string[len];

        string[] arr = value.Split(delim);
        if (arr.Length != len) Array.Resize(ref arr, len);
        return arr;
    }

    public static string[] SplitFixedLength(string value, char[] delims, int len)
    {
        if (value == null) return new string[len];

        string[] arr = value.Split(delims);
        if (arr.Length != len) Array.Resize(ref arr, len);
        return arr;
    }

    public static string EnsureLength(string sval, int len, bool bPadWhiteSpace = false, Alignment eAlign = Alignment.Left)
    {
        if (sval.Length > len) sval = sval.Substring(0, len);

        if (bPadWhiteSpace) sval = PadWithChar(sval, len, eAlign, ' ');

        return sval;
    }

    public static string EnsureLength(string sval, int len, char cPadChar, Alignment eAlign = Alignment.Left)
    {
        if (sval.Length > len) sval = sval.Substring(0, len);

        sval = PadWithChar(sval, len, eAlign, cPadChar);

        return sval;
    }


    public static string PadWithChar(string sString,
                                          int iLength,
                                          Alignment eAlign = 0,
                                          char sChar = ' ')
    {
        if (sChar == '\0') return null;

        switch (eAlign)
        {
            case Alignment.Right:
                return new String(sChar, (int)Math.Max(0, iLength - sString.Length)) + sString;
            case Alignment.Center:
                iLength = Math.Max(0, iLength - sString.Length);
                var sr = new String(sChar, (int)(Math.Ceiling(iLength / 2.0f))); // if odd, pad more on the right
                var sl = new String(sChar, (int)(Math.Floor(iLength / 2.0f)));
                return sl + sString + sr;
            case Alignment.Left:
                return sString + new String(sChar, (int)Math.Max(0, iLength - sString.Length));
        }

        //default trap
        return sString;
    }

    public static string PadWithChar(string sString,
                                      int iLength,
                                      char sAlign,
                                      char sChar = ' ')
    {
        switch (Char.ToUpper(sAlign))
        {
            case 'L':
                return PadWithChar(sString, iLength, Alignment.Left, sChar);
            case 'C':
                return PadWithChar(sString, iLength, Alignment.Center, sChar);
            case 'R':
                return PadWithChar(sString, iLength, Alignment.Right, sChar);

        }

        return null;
    }

    #endregion
}