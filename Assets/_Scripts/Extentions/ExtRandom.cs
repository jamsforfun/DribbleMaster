using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Random functions
/// <summary>
public static class ExtRandom
{
    // <summary>
    /// Generator for the random number generator
    /// </summary>
    private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();

    #region core script
    /// <summary>
    /// generate a random based on static hash
    /// </summary>
    /// <param name="seed">string based</param>
    /// <returns>rendom generated from the seed</returns>
    public static System.Random Seedrandom(string seed)
    {
        System.Random random = new System.Random(seed.GetHashCode());
        return (random);
    }

    /// <summary>
    /// here we have a min & max, we remap the random 0,1 value finded to this min&max
    /// warning: we cast it into an int at the end
    /// use: 
    /// System.Random seed = ExtRandom.Seedrandom("hash");
    /// int randomInt = ExtRandom.RemapFromSeed(0, 50, seed);
    /// </summary>
    public static int RemapFromSeed(double min, double max, System.Random randomSeed)
    {
        double zeroToOneValue = randomSeed.NextDouble();
        int minToMaxValue = (int)ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
        return (minToMaxValue);
    }
    public static double RemapFromSeedDecimal(double min, double max, System.Random randomSeed)
    {
        double zeroToOneValue = randomSeed.NextDouble();
        double minToMaxValue = ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
        return (minToMaxValue);
    }
    public static float RemapFromSeedDecimal(float min, float max, System.Random randomSeed)
    {
        float zeroToOneValue = (float)randomSeed.NextDouble();
        float minToMaxValue = ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
        return (minToMaxValue);
    }

    /// <summary>
    /// Returns a random direction in a cone. a spread of 0 is straight, 0.5 is 180*
    /// </summary>
    /// <param name="spread"></param>
    /// <param name="forward">must be unit</param>
    /// <returns></returns>
    public static Vector3 RandomDirection(float spread, Vector3 forward)
    {
        return Vector3.Slerp(forward, UnityEngine.Random.onUnitSphere, spread);
    }

    /// <summary>
    /// Get random int between min and max
    /// use: int randomInt = ExtRandom.GetRandomNumber(0, 2);
    /// </summary>
    /// <param name="minimum"></param>
    /// <param name="maximum">EXCLUSIVE</param>
    /// <returns></returns>
    public static int GetRandomNumber(int minimum, int maximum)
    {
        int number = UnityEngine.Random.Range(minimum, maximum);
        return (number);
    }
    /// <summary>
    /// Get random float between min an dmax included
    /// </summary>
    public static float GetRandomNumber(float minimum, float maximum)
    {
        float number = UnityEngine.Random.Range(minimum, maximum);
        return (number);
    }
    /// <summary>
    /// Get a random from chance / max: 2 / 3
    /// use: ExtRandom.GetRandomNumberProbability(2, 3);
    /// </summary>
    /// <param name="chance"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool GetRandomNumberProbability(int chance, int max)
    {
        float number = GetRandomNumber(0f, 1f);
        return (number > chance / max);
    }
    /// <summary>
    /// do a coin flip, return true or false
    /// </summary>
    public static bool GetRandomBool()
    {
        float number = UnityEngine.Random.Range(0f, 1f);
        return (number > 0.5f);
    }

    /// <summary>
    /// get a random color
    /// </summary>
    /// <returns></returns>
    public static Color GetRandomColor()
    {
        Color randomColor = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
        return (randomColor);
    }
    /// <summary>
    /// get a random color, with alpha 1
    /// </summary>
    /// <returns></returns>
    public static Color GetRandomColorSeed(System.Random randomSeed)
    {
        Color randomColor = new Color((float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), (float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), (float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), 1);
        return (randomColor);
    }

    /// <summary>
    /// get a normal random
    /// use: GenerateNormalRandom(0, 0.1);
    /// </summary>
    /// <param name="mu">centre of the distribution</param>
    /// <param name="sigma">Standard deviation (spread or "width") of the distribution</param>
    /// <returns></returns>
    public static float GenerateNormalRandom(float mu, float sigma)
    {
        float rand1 = UnityEngine.Random.Range(0.0f, 1.0f);
        float rand2 = UnityEngine.Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

        return (mu + sigma * n);
    }

    /// <summary>
    /// Get a random unitsphere (or donut)
    /// </summary>
    /// <param name="radius">radius of circle</param>
    /// <param name="toreCenter">Excluse center from random</param>
    /// <returns></returns>
    public static Vector3 GetRandomInsideUnitSphere(float radius, float toreCenter = 0)
    {
        if (toreCenter == 0)
            return (UnityEngine.Random.insideUnitSphere * radius);

        if (toreCenter > radius)
        {
            Debug.LogError("radiusCenter can't be superior then radius");
        }

        Vector3 donut = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            int absRandom = GetRandomNumber(0, 2);
            absRandom = (absRandom == 0) ? -1 : 1;

            donut.x = GetRandomNumber(toreCenter, radius) * absRandom;
            donut.y = GetRandomNumber(toreCenter, radius) * absRandom;
            donut.z = GetRandomNumber(toreCenter, radius) * absRandom;
        }
        return (donut);
    }

    /// <summary>
    /// Creates a random password mixed case, with numbers and special characters
    /// NOTE: sets value of sVal as well
    /// </summary>
    /// <param name="sVal"></param>
    /// <param name="PasswordLength">length of the new password</param>
    public static string CreateRandomPassword(this string sVal, int PasswordLength)
    {
        return CreateRandomPassword(sVal, PasswordLength, true, true, true, true, true, true, false);
    }

    /// <summary>
    /// Creates a random password mixed case, with numbers and special characters (numbers and special chars are allowed, but not required)
    /// NOTE: sets value of sVal as well
    /// </summary>
    /// <param name="sVal"></param>
    /// <param name="PasswordLength">length of the new password</param>
    /// <param name="allowMixedCase">allow upper and lower case</param>
    /// <param name="allowNumbers">allow numbers to be part of the password</param>
    /// <param name="allowSpecialCharacters">allow special characters</param>
    public static string CreateRandomPassword(this string sVal, int PasswordLength, bool allowMixedCase, bool allowNumbers, bool allowSpecialCharacters)
    {
        return CreateRandomPassword(sVal, PasswordLength, true, true, true, false, false, false, false);
    }

    /// <summary>
    /// Creates a random password mixed case, with numbers and special characters
    /// NOTE: sets value of sVal as well
    /// </summary>
    /// <param name="sVal"></param>
    /// <param name="PasswordLength">length of the new password</param>
    /// <param name="ignoreProgrammingCharacters">Avoid punctuation used in programming <![CDATA[ ($, ', ", &, [space], ,, ?, @, #, <, >, (, ), {, }, [, ], /, \) ]]> .</param>
    public static string CreateRandomPassword(this string sVal, int PasswordLength, bool ignoreProgrammingCharacters)
    {
        return CreateRandomPassword(sVal, PasswordLength, true, true, true, true, true, true, ignoreProgrammingCharacters);
    }

    /// <summary>
    /// Creates a random password mixed case, with numbers and special characters (numbers and special chars are allowed, but not required)
    /// NOTE: sets value of sVal as well
    /// </summary>
    /// <param name="sVal"></param>
    /// <param name="PasswordLength">length of the new password</param>
    /// <param name="allowMixedCase">allow upper and lower case</param>
    /// <param name="allowNumbers">allow numbers to be part of the password</param>
    /// <param name="allowSpecialCharacters">allow special characters</param>
    /// <param name="ignoreProgrammingCharacters">Avoid punctuation used in programming <![CDATA[ ($, ', ", &, [space], ,, ?, @, #, <, >, (, ), {, }, [, ], /, \) ]]> .</param>
    public static string CreateRandomPassword(this string sVal, int PasswordLength, bool allowMixedCase, bool allowNumbers, bool allowSpecialCharacters, bool ignoreProgrammingCharacters)
    {
        return CreateRandomPassword(sVal, PasswordLength, true, true, true, false, false, false, ignoreProgrammingCharacters);
    }

    /// <summary>
    /// Returns a random number within the range, inclusive
    /// </summary>
    /// <param name="minimumValue">min number</param>
    /// <param name="maximumValue">max number, inclusive</param>
    public static int RandomRangeInclusive(this int minimumValue, int maximumValue)
    {
        byte[] randomNumber = new byte[1];
        RandomGenerator.GetBytes(randomNumber);

        // We are using Math.Max, and subtracting 0.00000000001, 
        // to ensure "multiplier" will always be between 0.0 and .99999999999
        // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
        //double multiplier = Math.Max(0, (randomNumber[0].ToDouble() / 255d) - 0.00000000001d);

        //// We need to add one to the range, to allow for the rounding done with Math.Floor
        //int range = maximumValue - minimumValue + 1;
        //double randomValueInRange = Math.Floor(multiplier * range);

        // We need to add one to the range, to allow for the rounding done with Math.Floor
        return (int)(
            minimumValue +
            Math.Floor( // randomValueInRange
                (Math.Max(0, (randomNumber[0].ToDouble() / 255d) - 0.00000000001d)) *  // multiplier
                (maximumValue - minimumValue + 1))); // range
    }

    /// <summary>
    /// Creates a random password (a-z, A-Z, 0-9), and with special characters.
    /// NOTE: sets value of sVal as well
    /// </summary>
    /// <param name="sVal"></param>
    /// <param name="PasswordLength">length of the new password</param>
    /// <param name="allowMixedCase">allow upper and lower case</param>
    /// <param name="allowNumbers">allow numbers to be part of the password</param>
    /// <param name="allowSpecialCharacters">allow special characters</param>
    /// <param name="mixedCaseRequired">true to required both upper and lower case letters in the password</param>
    /// <param name="numberRequired">true to require at least one number in the password</param>
    /// <param name="specialRequired">true to require at least one special character</param>
    /// <param name="ignoreProgrammingCharacters">Avoid punctuation used in programming <![CDATA[ ($, ', ", &, [space], ,, ?, @, #, <, >, (, ), {, }, [, ], /, \) ]]> .</param>
    public static string CreateRandomPassword(this string sVal, int PasswordLength, bool allowMixedCase, bool allowNumbers, bool allowSpecialCharacters, bool mixedCaseRequired, bool numberRequired, bool specialRequired, bool ignoreProgrammingCharacters)
    {
        #region Validation

        if ((!allowMixedCase) && (mixedCaseRequired))
            throw new ArgumentException("mixedCaseRequired cannot be true if allowMixedCase is false", "allowMixedCase");
        if ((!allowNumbers) && (numberRequired))
            throw new ArgumentException("numberRequired cannot be true if allowNumbers is false", "allowNumbers");
        if ((!allowSpecialCharacters) && (specialRequired))
            throw new ArgumentException("specialRequired cannot be true if allowSpecialCharacters is false", "allowSpecialCharacters");

        // Validation
        #endregion

        StringBuilder sb = new StringBuilder();

        // pick random locations for each
        int UpperCaseIndex = 0.RandomRangeInclusive(PasswordLength);
        int NumberIndex = 1.RandomRangeInclusive(PasswordLength);
        while (NumberIndex == UpperCaseIndex)
            NumberIndex = 1.RandomRangeInclusive(PasswordLength);
        int SpecialIndex = 1.RandomRangeInclusive(PasswordLength);
        while ((SpecialIndex == UpperCaseIndex) || (SpecialIndex == NumberIndex))
            SpecialIndex = 1.RandomRangeInclusive(PasswordLength);

        // loop through and randomly create the password
        for (int i = 0; i < PasswordLength; i++)
        {
            // if we are using a required then, test now
            if ((mixedCaseRequired) && (i == UpperCaseIndex))
            {
                // A-Z (65-90)
                sb.Append(65.RandomRangeInclusive(90).ToChar());
                continue;
            }

            // force lower case
            if (i == 0)
            {
                // a-z (97-122)
                sb.Append(97.RandomRangeInclusive(122).ToChar());
                continue;
            }

            if ((numberRequired) && (i == NumberIndex))
            {
                // 0-9 (48-57)
                sb.Append(48.RandomRangeInclusive(57).ToChar());
                continue;
            }

            if ((specialRequired) && (i == SpecialIndex))
            {
                sb.Append(GetSpecialCode(ignoreProgrammingCharacters));
                continue;
            }

            // when true, we've assigned a code for this iteration
            bool HasCode = false;

            // loop until we get a code for this iteration
            while (!HasCode)
            {
                // random number 1-4
                switch (1.RandomRangeInclusive(4))
                {
                    case 1: // a-z (97-122)
                        sb.Append(97.RandomRangeInclusive(123).ToChar());
                        HasCode = true;
                        break;
                    case 2: // A-Z (65-90)
                        if (allowMixedCase)
                        {
                            sb.Append(65.RandomRangeInclusive(91).ToChar());
                            HasCode = true;
                        }
                        break;
                    case 3: // 0-9 (48-57)
                        if (allowNumbers)
                        {
                            sb.Append(48.RandomRangeInclusive(58).ToChar());
                            HasCode = true;
                        }
                        break;
                    case 4: // # $ % ^ & * ( )  (32-47 & 58-64)
                        if (allowSpecialCharacters)
                        {
                            sb.Append(GetSpecialCode(ignoreProgrammingCharacters));
                            HasCode = true;
                        }
                        break;
                }
            }
        }

        // return
        return sb.ToString().Trim();
    }

    /// <summary>
    /// Returns a special code
    /// </summary>
    /// <param name="ignoreProgrammingCharacters">true to ignore programming characters</param>
    private static char GetSpecialCode(bool ignoreProgrammingCharacters)
    {
        // $, ', ", &, [space], ,, ?, @, #, <, >, (, ), {, }, [, ], /, \

        int specialCode;

        do
        {
            // get the code
            specialCode = 32.RandomRangeInclusive(65);

            // keep going until we get a valid one
            // skip numbers (ASCII 48 - 57)
        } while (((specialCode > 47) && (specialCode < 58)) ||
            (ignoreProgrammingCharacters && specialCode != 37 && specialCode != 42 
            && specialCode != 43 && specialCode != 45 && specialCode != 61)); // only allow %, *, +, -, =   if ignore programming

        return specialCode.ToChar();
    }
    #endregion
}
