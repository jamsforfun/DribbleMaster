using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public static class ExtMathf
{
    /// <summary>
    /// number convert range (55 from 0 to 100, to a base 0 - 1 for exemple)
    /// </summary>
    public static double Remap(this double value, double from1, double to1, double from2, double to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    /// <summary>
    /// return the average number between an array of points
    /// </summary>
    public static float GetAverageOfNumbers(float[] arrayNumber)
    {
        if (arrayNumber.Length == 0)
            return (0);

        float sum = 0;
        for (int i = 0; i < arrayNumber.Length; i++)
        {
            sum += arrayNumber[i];
        }
        return (sum / arrayNumber.Length);
    }

    /// <summary>
    /// return the min of 3 value
    /// </summary>
    public static float Min(float value1, float value2, float value3)
	{
		float min = (value1 < value2) ? value1 : value2;
		return (min < value3) ? min : value3;
	}

    /// <summary>
    /// return the max of 3 value
    /// </summary>
	public static float Max(float value1, float value2, float value3)
	{
		float max = (value1 > value2) ? value1 : value2;
		return (max > value3) ? max : value3;
	}

	public static bool IsClose(float value1, float value2) {return IsClose(value1, value2, Mathf.Epsilon);}
	public static bool IsClose(float value1, float value2, float precision)
	{
		return Mathf.Abs(value1 - value2) < precision;
	}
    public static bool IsBetween(float value1, float valueToTest, float value2)
    {
        return (value1 <= valueToTest && valueToTest <= value2);
    }

    /// <summary>
    /// return the value clamped between the 2 value
    /// </summary>
    /// <param name="value1">must be less than value2</param>
    /// <param name="currentValue"></param>
    /// <param name="value2">must be more than value1</param>
    /// <returns></returns>
    public static float SetBetween(float currentValue, float value1, float value2)
    {
        if (value1 > value2)
        {
            Debug.LogError("value2 can be less than value1");
            return (0);
        }

        if (currentValue < value1)
        {
            currentValue = value1;
        }
        if (currentValue > value2)
        {
            currentValue = value2;
        }
        return (currentValue);
    }

	public static float Squared(this float value)
	{
		return value * value;
	}
	public static float Squared(this int value)
	{
		return value * value;
	}

    /// <summary>
    /// get closest point from an array of points
    /// </summary>
    public static Vector3 GetClosestPoint(Vector3 posEntity, Vector3[] arrayPos, ref int indexFound)
    {
        float sqrDist = 0;
        indexFound = -1;

        int firstIndex = 0;

        for (int i = 0; i < arrayPos.Length; i++)
        {
            if (ExtVector3.IsNullVector(arrayPos[i]))
                continue;

            float dist = (posEntity - arrayPos[i]).sqrMagnitude;
            if (firstIndex == 0)
            {
                indexFound = i;
                sqrDist = dist;
            }
            else if (dist < sqrDist)
            {
                sqrDist = dist;
                indexFound = i;
            }
            firstIndex++;
        }

        if (indexFound == -1)
        {
            //Debug.LogWarning("nothing found");
            return (ExtVector3.GetNullVector());
        }
        return (arrayPos[indexFound]);
    }

    /// <summary>
    /// from a given valueToTest, find the closest point
    /// </summary>
    /// <param name="valueToTest"></param>
    /// <param name="allValues"></param>
    /// <returns></returns>
    public static float GetClosestValueFromAnother(float targetNumber, float [] allValues)
    {
        float nearest = allValues.Min(x => Math.Abs((long)x - targetNumber));
        return (nearest);
    }

    /// <summary>
    /// return the point at a given percentage of a line
    /// </summary>
    /// <param name="percentage">0 to 1</param>
    /// <returns></returns>
    public static Vector3 GetPercentageAlong(Vector3 a, Vector3 b, float percentage)
    {
        return (Vector3.Lerp(a, b, percentage));
    }
    /// <summary>
    /// return percentage (from 0 to 1) of the position of the C vector
    /// </summary>
    public static float GetPercentageAlong(Vector3 a, Vector3 b, Vector3 c)
    {
        var ab = b - a;
        var ac = c - a;
        return Vector3.Dot(ac, ab) / ab.sqrMagnitude;
    }

    /// <summary>
    /// from a given paths (sets of points), return the lenght of the path
    /// </summary>
    /// <param name="chunkPath"></param>
    /// <returns></returns>
    public static float GetLenghtOfPath(Vector3[] chunkPath)
    {
        float lenghtPath = 0f;

        for (int i = 0; i < chunkPath.Length - 1; i++)
        {
            Vector3 posCurrent = chunkPath[i];
            Vector3 posNext = chunkPath[i + 1];
            lenghtPath += (posNext - posCurrent).magnitude;
        }
        return (lenghtPath);
    }

    /// <summary>
    /// from a given path (sets of points), get the closest position of pos from this path, and return the
    /// current percentage inside
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="chunkPath"></param>
    /// <param name="precision"></param>
    /// <returns></returns>
    public static float GetPercentageFromPosToChunkPath(Vector3 pos, Vector3[] chunkPath, float precision = 1f)
    {
        if (chunkPath == null || chunkPath.Length == 0)
        {
            //Debug.LogWarning("out of bounds");
            return (0);
        }

        float percent = 0f;
        float lenghtPath = ExtMathf.GetLenghtOfPath(chunkPath);

        Vector3[] closestPointInLines = new Vector3[chunkPath.Length];
        float[] lenghtAllLine = new float[chunkPath.Length];

        for (int i = 0; i < closestPointInLines.Length - 1; i++)
        {
            ExtLine line = new ExtLine(chunkPath[i], chunkPath[i + 1]);
            closestPointInLines[i] = line.ClosestPointTo(pos);

            lenghtAllLine[i] = (float)line.GetLenght();
        }

        int indexFound = -1;
        Vector3 closestPoint = ExtMathf.GetClosestPoint(pos, closestPointInLines, ref indexFound);

        if (indexFound >= lenghtAllLine.Length)
        {
            //Debug.LogWarning("out of bounds");
            return (0);
        }

        float lenghtLineClose = lenghtAllLine[indexFound];
        Vector3 veccA = chunkPath[indexFound];
        int indexPlusOne = indexFound + 1;
        if (indexPlusOne >= chunkPath.Length)
        {
            indexPlusOne = 0;
        }
        Vector3 veccB = chunkPath[indexPlusOne];
        float percentageAlongCLosestLine = ExtMathf.GetPercentageAlong(veccA, veccB, closestPoint);

        float lenghtTraveledInThisLine = (percentageAlongCLosestLine * lenghtLineClose) / 1f;

        //add lenght, from start to the line found
        float lenghtFromZeroToThisPoint = 0f;
        for (int i = 0; i < indexFound; i++)
        {
            lenghtFromZeroToThisPoint += lenghtAllLine[i];
        }
        lenghtFromZeroToThisPoint += lenghtTraveledInThisLine;

        //then add the additionnal percentage
        percent = (lenghtFromZeroToThisPoint * 100f) / lenghtPath;

        //Debug.Log("ChunkLenght: " + lenghtPath + ", lenght closest line: " + lenghtLineClose + ", percent along this line: " + percentageAlongCLosestLine
        //    + "lenght From Zero to this point: " + lenghtFromZeroToThisPoint + ", total percent: " + percent);

        return (percent);
    }

    #region series

    /// <summary>
    /// Sums a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Sum(1,2,3,4) == 10
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static short Sum(params short[] arr)
    {
        short result = 0;

        foreach (short value in arr)
        {
            result += value;
        }

        return result;
    }

    /// <summary>
    /// Sums a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Sum(1,2,3,4) == 10
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static int Sum(params int[] arr)
    {
        int result = 0;

        foreach (int value in arr)
        {
            result += value;
        }

        return result;
    }

    public static int Sum(int[] arr, int startIndex, int endIndex)
    {
        int result = 0;

        for (int i = startIndex; i <= Math.Min(endIndex, arr.Length - 1); i++)
        {
            result += arr[i];
        }

        return result;
    }

    public static int Sum(int[] arr, int startIndex)
    {
        return Sum(arr, startIndex, int.MaxValue);
    }

    /// <summary>
    /// Sums a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Sum(1,2,3,4) == 10
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static long Sum(params long[] arr)
    {
        long result = 0;

        foreach (long value in arr)
        {
            result += value;
        }

        return result;
    }

    /// <summary>
    /// Sums a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Sum(1,2,3,4) == 10
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static float Sum(params float[] arr)
    {
        float result = 0;

        foreach (float value in arr)
        {
            result += value;
        }

        return result;
    }


    /// <summary>
    /// Multiplies a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Product(2,3,4) == 24
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static float Product(params short[] arr)
    {
        if (arr == null || arr.Length == 0)
            return float.NaN;

        float result = 1;

        foreach (short value in arr)
        {
            result *= value;
        }

        return result;
    }

    /// <summary>
    /// Multiplies a series of numeric values passed as a param array...
    /// 
    /// MathUtil.Product(2,3,4) == 24
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static float Product(params int[] arr)
    {
        if (arr == null || arr.Length == 0)
            return float.NaN;

        float result = 1;

        foreach (int value in arr)
        {
            result *= value;
        }

        return result;
    }

    /// <summary>
    /// Multiplies a series of numeric values passed as a param array...
    /// 
    /// MathUtil.ProductSeries(2,3,4) == 24
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static float Product(params long[] arr)
    {
        if (arr == null || arr.Length == 0)
            return float.NaN;

        float result = 1;

        foreach (long value in arr)
        {
            result *= value;
        }

        return result;
    }

    /// <summary>
    /// Multiplies a series of numeric values passed as a param array...
    /// 
    /// MathUtil.ProductSeries(2,3,4) == 24
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static float Product(params float[] arr)
    {
        if (arr == null || arr.Length == 0)
            return float.NaN;

        float result = 1f;

        foreach (float value in arr)
        {
            result *= value;
        }

        return result;
    }

    public static float Product(this IEnumerable<float> coll)
    {
        if (coll == null) return float.NaN;

        float result = 1f;
        foreach (float value in coll)
        {
            result *= value;
        }
        return result;
    }

    #endregion

    #region "Advanced Math"

    /// <summary>
    /// Compute the logarithm of any value of any base
    /// </summary>
    /// <param name="value"></param>
    /// <param name="base"></param>
    /// <returns></returns>
    /// <remarks>
    /// a logarithm is the exponent that some constant (base) would have to be raised to 
    /// to be equal to value.
    /// 
    /// i.e.
    /// 4 ^ x = 16
    /// can be rewritten as to solve for x
    /// logB4(16) = x
    /// which with this function would be 
    /// LoDMath.logBaseOf(16,4)
    /// 
    /// which would return 2, because 4^2 = 16
    /// </remarks>
    public static float LogBaseOf(float value, float @base)
    {
        return (float)(Math.Log(value) / Math.Log(@base));
    }

    /// <summary>
    /// Check if a value is prime.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// In this method to increase speed we first check if the value is ltOReq 1, because values ltOReq 1 are not prime by definition. 
    /// Then we check if the value is even but not equal to 2. If so the value is most certainly not prime. 
    /// Lastly we loop through all odd divisors. No point in checking 1 or even divisors, because if it were divisible by an even 
    /// number it would be divisible by 2. If any divisor existed when i > value / i then its compliment would have already 
    /// been located. And lastly the loop will never reach i == val because i will never be > sqrt(val).
    /// 
    /// proof of validity for algorithm:
    /// 
    /// all trivial values are thrown out immediately by checking if even or less then 2
    /// 
    /// all remaining possibilities MUST be odd, an odd is resolved as the multiplication of 2 odd values only. (even * anyValue == even)
    /// 
    /// in resolution a * b = val, a = val / b. As every compliment a for b, b and a can be swapped resulting in b being ltOReq a. If a compliment for b 
    /// exists then that compliment would have already occured (as it is odd) in the swapped addition at the even split.
    /// 
    /// Example...
    /// 
    /// 16
    /// 1 * 16
    /// 2 * 8
    /// 4 * 4
    /// 8 * 2
    /// 16 * 1
    /// 
    /// checks for 1, 2, and 4 would have already checked the validity of 8 and 16.
    /// 
    /// Thusly we would only have to loop as long as i ltOReq val / i. Once we've reached the middle compliment, all subsequent factors have been resolved.
    /// 
    /// This shrinks the number of loops for odd values from [ floor(val / 2) - 1 ] down to [ ceil(sqrt(val) / 2) - 1 ]
    /// 
    /// example, if we checked EVERY odd number for the validity of the prime 7927, we'd loop 3962 times
    /// 
    /// but by this algorithm we loop only 43 times. Significant improvement!
    /// </remarks>
    public static bool IsPrime(long value)
    {
        // check if value is in prime number range
        if (value < 2)
            return false;

        // check if even, but not equal to 2
        if ((value % 2) == 0 & value != 2)
            return false;

        // if 2 or odd, check if any non-trivial divisors exist
        long sqrrt = (long)Math.Floor(Math.Sqrt(value));
        for (long i = 3; i <= sqrrt; i += 2)
        {
            if ((value % i) == 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Relative Primality between two integers
    /// 
    /// By definition two integers are considered relatively prime if their 
    /// 'greatest common divisor' is 1. So thusly we simply just check if 
    /// the GCD of m and n is 1.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static bool IsRelativelyPrime(short m, short n)
    {
        return GCD(m, n) == 1;
    }

    public static bool IsRelativelyPrime(int m, int n)
    {
        return GCD(m, n) == 1;
    }

    public static bool IsRelativelyPrime(long m, long n)
    {
        return GCD(m, n) == 1;
    }

    public static int[] FactorsOf(int value)
    {
        value = Math.Abs(value);
        List<int> arr = new List<int>();
        int sqrrt = (int)Math.Sqrt(value);
        int c = 0;

        for (int i = 1; i <= sqrrt; i++)
        {
            if ((value % i) == 0)
            {
                arr.Add(i);
                c = value / i;
                if (c != i)
                    arr.Add(c);
            }
        }

        arr.Sort();

        return arr.ToArray();
    }

    public static int[] CommonFactorsOf(int m, int n)
    {
        int i = 0;
        int j = 0;
        if (m < 0) m = -m;
        if (n < 0) n = -n;

        if (m > n)
        {
            i = m;
            m = n;
            n = i;
        }

        var set = new HashSet<int>(); //ensures no duplicates

        int r = (int)Math.Sqrt(m);
        for (i = 1; i <= r; i++)
        {
            if ((m % i) == 0 && (n % i) == 0)
            {
                set.Add(i);
                j = m / i;
                if ((n % j) == 0) set.Add(j);
                j = n / i;
                if ((m % j) == 0) set.Add(j);
            }
        }

        int[] arr = System.Linq.Enumerable.ToArray(set);
        System.Array.Sort(arr);
        return arr;



        //more loops
        /*
        List<int> arr = new List<int>();

        int i = 0;
        if (m < 0) m = -m;
        if (n < 0) n = -n;

        //make sure m is < n
        if (m > n)
        {
            i = m;
            m = n;
            n = i;
        }

        //could be sped up by looping to sqrt(m), but then would have to do extra work to make sure duplicates don't occur
        for (i = 1; i <= m; i++)
        {
            if ((m % i) == 0 && (n % i) == 0)
            {
                arr.Add(i);
            }
        }

        return arr.ToArray();
        */
    }

    /// <summary>
    /// Greatest Common Divisor using Euclid's algorithm
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static int GCD(int m, int n)
    {
        int r = 0;

        // make sure positive, GCD is always positive
        if (m < 0) m = -m;
        if (n < 0) n = -n;

        // m must be >= n
        if (m < n)
        {
            r = m;
            m = n;
            n = r;
        }

        // now start loop, loop is infinite... we will cancel out sooner or later
        while (true)
        {
            r = m % n;
            if (r == 0)
                return n;
            m = n;
            n = r;
        }

        // fail safe
        //return 1;
    }

    public static long GCD(long m, long n)
    {
        long r = 0;

        // make sure positive, GCD is always positive
        if (m < 0) m = -m;
        if (n < 0) n = -n;

        // m must be >= n
        if (m < n)
        {
            r = m;
            m = n;
            n = r;
        }

        // now start loop, loop is infinite... we will cancel out sooner or later
        while (true)
        {
            r = m % n;
            if (r == 0)
                return n;
            m = n;
            n = r;
        }

        // fail safe
        //return 1;
    }

    public static int LCM(int m, int n)
    {
        return (m * n) / GCD(m, n);
    }

    /// <summary>
    /// Factorial - N!
    /// 
    /// Simple product series
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// By definition 0! == 1
    /// 
    /// Factorial assumes the idea that the value is an integer >= 0... thusly UInteger is used
    /// </remarks>
    public static long Factorial(uint value)
    {
        if (value <= 0)
            return 1;

        long res = value;

        while (--value != 0)
        {
            res *= value;
        }

        return res;
    }

    /// <summary>
    /// Falling facotiral
    /// 
    /// defined: (N)! / (N - x)!
    /// 
    /// written subscript: (N)x OR (base)exp
    /// </summary>
    /// <param name="base"></param>
    /// <param name="exp"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static long FallingFactorial(uint @base, uint exp)
    {
        return Factorial(@base) / Factorial(@base - exp);
    }

    /// <summary>
    /// rising factorial
    /// 
    /// defined: (N + x - 1)! / (N - 1)!
    /// 
    /// written superscript N^(x) OR base^(exp)
    /// </summary>
    /// <param name="base"></param>
    /// <param name="exp"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static long RisingFactorial(uint @base, uint exp)
    {
        return Factorial(@base + exp - 1) / Factorial(@base - 1);
    }

    /// <summary>
    /// binomial coefficient
    /// 
    /// defined: N! / (k!(N-k)!)
    /// reduced: N! / (N-k)! == (N)k (fallingfactorial)
    /// reduced: (N)k / k!
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static long BinCoef(uint n, uint k)
    {
        return FallingFactorial(n, k) / Factorial(k);
    }

    /// <summary>
    /// rising binomial coefficient
    /// 
    /// as one can notice in the analysis of binCoef(...) that 
    /// binCoef is the (N)k divided by k!. Similarly rising binCoef 
    /// is merely N^(k) / k! 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static long RisingBinCoef(uint n, uint k)
    {
        return RisingFactorial(n, k) / Factorial(k);
    }
    #endregion

    public static bool IsHex(string value)
    {
        int i;
        for (i = 0; i < value.Length; i++)
        {
            if (value[i] == ' ' || value[i] == '+' || value[i] == '-') continue;

            break;
        }

        return (i < value.Length - 1 &&
                (
                (value[i] == '#') ||
                (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                ));
    }

    /// <summary>
    /// Returns true if the value is a numeric type that is a whole round number.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bBlankIsZero"></param>
    /// <returns></returns>
    public static bool IsInteger(object value)
    {
        if (value == null) return false;

        if (value is System.IConvertible)
        {
            var conv = value as System.IConvertible;
            if (IsInteger(conv.GetTypeCode())) return true;
            return (conv.ToDouble(null) % 1d) == 0d;
        }

        return false;
    }

    public static bool IsInteger(System.TypeCode code)
    {
        switch (code)
        {
            case System.TypeCode.SByte:
                //5
                return true;
            case System.TypeCode.Byte:
                //6
                return true;
            case System.TypeCode.Int16:
                //7
                return true;
            case System.TypeCode.UInt16:
                //8
                return true;
            case System.TypeCode.Int32:
                //9
                return true;
            case System.TypeCode.UInt32:
                //10
                return true;
            case System.TypeCode.Int64:
                //11
                return true;
            case System.TypeCode.UInt64:
                //12
                return true;
            default:
                return false;
        }
    }
}
