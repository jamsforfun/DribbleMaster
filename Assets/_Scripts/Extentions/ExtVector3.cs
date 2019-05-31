using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class ExtVector3
{
	public static readonly Vector3[] GeneralDirections = new Vector3[] {Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back};

    #region nullVector
    /// <summary>
    /// define a null vector
    /// </summary>
    private static Vector3 wrongVector = new Vector3(0.042f, 0, 0);

    /// <summary>
    /// return a null vector
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetNullVector()
    {
        return (wrongVector);
    }
    public static bool IsNullVector(Vector3 vecToTest)
    {
        return (vecToTest == wrongVector);
    }
    /// <summary>
    /// create/fill an array of size lenght with null vector
    /// </summary>
    public static Vector3[] CreateNullVectorArray(int lenght)
    {
        Vector3[] arrayPoints = new Vector3[lenght];
        FillArrayWithWrongVector(ref arrayPoints);
        return (arrayPoints);
    }
    public static void FillArrayWithWrongVector(ref Vector3[] arrayToFill)
    {
        for (int i = 0; i < arrayToFill.Length; i++)
        {
            arrayToFill[i] = GetNullVector();
        }
    }
    #endregion

    #region misc
    
    /// <summary>
    /// formula behind smoothDamp
    /// </summary>
    public static float OwnSmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = Mathf.Max(0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        float num4 = current - target;
        float num5 = target;
        float num6 = maxSpeed * smoothTime;
        num4 = Mathf.Clamp(num4, -num6, num6);
        target = current - num4;
        float num7 = (currentVelocity + num * num4) * deltaTime;
        currentVelocity = (currentVelocity - num * num7) * num3;
        float num8 = target + (num4 + num7) * num3;
        if (num5 - current > 0f == num8 > num5)
        {
            num8 = num5;
            currentVelocity = (num8 - num5) / deltaTime;
        }
        return num8;
    }

    /// <summary>
    /// has a target reach a position in space ?
    /// </summary>
    /// <param name="objectMoving"></param>
    /// <param name="target"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static bool HasReachedTargetPosition(Vector3 objectMoving, Vector3 target, float margin = 0)
    {
        float x = objectMoving.x;
        float y = objectMoving.y;
        float z = objectMoving.z;

        return (x > target.x - margin
            && x < target.x + margin
            && y > target.y - margin
            && y < target.y + margin
            && z > target.z - margin
            && z < target.z + margin);
    }


    public static Vector3 ClosestDirectionTo(Vector3 direction1, Vector3 direction2, Vector3 targetDirection)
    {
        return (Vector3.Dot(direction1, targetDirection) > Vector3.Dot(direction2, targetDirection)) ? direction1 : direction2;
    }

    /// <summary>
    /// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
    /// compares the square of the distance to the square of the range as this
    /// avoids calculating a square root which is much slower than squaring the range
    /// </summary>
    /// <param name="val"></param>
    /// <param name="about"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static bool IsClose(Vector3 val, Vector3 about, float range)
    {
        return ((val - about).sqrMagnitude < range * range);
    }

    /// <summary>
    /// Direct speedup of <seealso cref="Vector3.Lerp"/>
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 Lerp(Vector3 v1, Vector3 v2, float value)
    {
        if (value > 1.0f)
            return v2;
        if (value < 0.0f)
            return v1;
        return new Vector3(v1.x + (v2.x - v1.x) * value,
                           v1.y + (v2.y - v1.y) * value,
                           v1.z + (v2.z - v1.z) * value);
    }
    public static Vector3 Sinerp(Vector3 from, Vector3 to, float value)
    {
        value = Mathf.Sin(value * Mathf.PI * 0.5f);
        return Vector3.Lerp(from, to, value);
    }

    /// <summary>
    /// is a vector considered by unity as NaN
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static bool IsNaN(this Vector3 vec)
    {
        return float.IsNaN(vec.x * vec.y * vec.z);
    }

    #endregion

    #region vector calculation
    /// <summary>
    /// get the max lenght of this vector
    /// </summary>
    /// <returns>min lenght of x, y or z</returns>
    public static float Maximum(this Vector3 vector)
    {
        return ExtMathf.Max(vector.x, vector.y, vector.z);
    }

    /// <summary>
    /// get the min lenght of this vector
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>min lenght of x, y or z</returns>
	public static float Minimum(this Vector3 vector)
    {
        return ExtMathf.Min(vector.x, vector.y, vector.z);
    }

    /// <summary>
    /// is 2 vectors parallel
    /// </summary>
    /// <param name="direction">vector 1</param>
    /// <param name="otherDirection">vector 2</param>
    /// <param name="precision">test precision</param>
    /// <returns>is parallel</returns>
	public static bool IsParallel(Vector3 direction, Vector3 otherDirection, float precision = .000001f)
    {
        return Vector3.Cross(direction, otherDirection).sqrMagnitude < precision;
    }

    public static float MagnitudeInDirection(Vector3 vector, Vector3 direction, bool normalizeParameters = true)
    {
        if (normalizeParameters) direction.Normalize();
        return Vector3.Dot(vector, direction);
    }

    /// <summary>
    /// Absolute value of vector
    /// </summary>
    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// from a given plane (define by a normal), return the projection of a vector
    /// </summary>
    /// <param name="relativeDirection"></param>
    /// <param name="normalPlane"></param>
    /// <returns></returns>
    public static Vector3 ProjectVectorIntoPlane(Vector3 relativeDirection, Vector3 normalPlane)
    {
        //Projection of a vector on a plane and matrix of the projection.
        //http://users.telenet.be/jci/math/rmk.htm

        Vector3 Pprime = Vector3.Project(relativeDirection, normalPlane);
        Vector3 relativeProjeted = relativeDirection - Pprime;
        return (relativeProjeted);
    }


    /// <summary>
    /// https://docs.unity3d.com/ScriptReference/Vector3.Reflect.html
    /// VectorA: input
    /// VectorB: normal
    /// Vector3: result
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetReflectAngle(Vector3 inputVector, Vector3 normalVector)
    {
        return (Vector3.Reflect(inputVector.normalized, normalVector.normalized));
    }


    public static void Reflect(ref Vector2 v, Vector2 normal)
    {
        var dp = 2f * Vector2.Dot(v, normal);
        var ix = v.x - normal.x * dp;
        var iy = v.y - normal.y * dp;
        v.x = ix;
        v.y = iy;
    }

    public static Vector2 Reflect(Vector2 v, Vector2 normal)
    {
        var dp = 2 * Vector2.Dot(v, normal);
        return new Vector2(v.x - normal.x * dp, v.y - normal.y * dp);
    }

    public static void Mirror(ref Vector2 v, Vector2 axis)
    {
        v = (2 * (Vector2.Dot(v, axis) / Vector2.Dot(axis, axis)) * axis) - v;
    }

    public static Vector2 Mirror(Vector2 v, Vector2 axis)
    {
        return (2 * (Vector2.Dot(v, axis) / Vector2.Dot(axis, axis)) * axis) - v;
    }

    /// <summary>
    /// Returns a vector orthogonal to up in the general direction of forward.
    /// </summary>
    /// <param name="up"></param>
    /// <param name="targForward"></param>
    /// <returns></returns>
    public static Vector3 GetForwardTangent(Vector3 forward, Vector3 up)
    {
        return Vector3.Cross(Vector3.Cross(up, forward), up);
    }

    /// <summary>
    /// Dot product de 2 vecteur, retourne négatif si l'angle > 90°, 0 si angle = 90, positif si moin de 90
    /// </summary>
    /// <param name="a">vecteur A</param>
    /// <param name="b">vecteur B</param>
    /// <returns>retourne négatif si l'angle > 90°</returns>
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return (Vector3.Dot(a, b));
    }

    /// <summary>
    /// retourne le vecteur de droite au vecteur A, selon l'axe Z
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector3 CrossProduct(Vector3 a, Vector3 z)
    {
        return (Vector3.Cross(a, z));
    }

    /// <summary>
    /// return if we are right or left from a vector. 1: right, -1: left, 0: forward
    /// </summary>
    /// <param name="forwardDir"></param>
    /// <param name="upDir">up reference of the forward dir</param>
    /// <param name="toGoDir">target direction to test</param>
    public static int IsRightOrLeft(Vector3 forwardDir, Vector3 upDir, Vector3 toGoDir, Vector3 debugPos, ref float dotLeft, ref float dotRight)
    {
        Vector3 left = CrossProduct(forwardDir, upDir);
        Vector3 right = -left;

        //Debug.DrawRay(debugPos, left, Color.magenta, 5f);
        //Debug.DrawRay(debugPos, right, Color.magenta, 5f);


        dotRight = DotProduct(right, toGoDir);
        dotLeft = DotProduct(left, toGoDir);
        //Debug.Log("left: " + dotLeft + ", right: " + dotRight);
        if (dotRight > 0)
        {
            //Debug.Log("go right");
            return (1);
        }
        else if (dotLeft > 0)
        {
            //Debug.Log("go left");
            return (-1);
        }
        //Debug.Log("go pls");
        return (0);
    }

    /// <summary>
    /// get mirror of a vector, according to a normal
    /// </summary>
    /// <param name="point">Vector 1</param>
    /// <param name="normal">normal</param>
    /// <returns>vector mirror to 1 (reference= normal)</returns>
    public static Vector3 ReflectionOverPlane(Vector3 point, Vector3 normal)
    {
        return point - 2 * normal * Vector3.Dot(point, normal) / Vector3.Dot(normal, normal);
    }

    /// <summary>
    /// Return the projection of A on B (with the good magnitude), based on ref (ex: Vector3.up)
    /// </summary>
    public static Vector3 GetProjectionOfAOnB(Vector3 A, Vector3 B, Vector3 refVector)
    {
        float angleDegre = SignedAngleBetween(A, B, refVector); //get angle A-B
        angleDegre *= Mathf.Deg2Rad;                            //convert to rad
        float magnitudeX = Mathf.Cos(angleDegre) * A.magnitude; //get magnitude
        Vector3 realDir = B.normalized * magnitudeX;            //set magnitude of new Vector
        return (realDir);   //vector A with magnitude based on B
    }

    /// <summary>
    /// Return the projection of A on B (with the good magnitude), based on ref (ex: Vector3.up)
    /// </summary>
    public static Vector3 GetProjectionOfAOnB(Vector3 A, Vector3 B, Vector3 refVector, float minMagnitude, float maxMagnitude)
    {
        float angleDegre = SignedAngleBetween(A, B, refVector); //get angle A-B
        angleDegre *= Mathf.Deg2Rad;                            //convert to rad
        float magnitudeX = Mathf.Cos(angleDegre) * A.magnitude; //get magnitude
        //set magnitude of new Vector
        Vector3 realDir = B.normalized * Mathf.Clamp(Mathf.Abs(magnitudeX), minMagnitude, maxMagnitude) * Mathf.Sign(magnitudeX);
        return (realDir);   //vector A with magnitude based on B
    }

    /// <summary>
    /// return the fast inverse squared of a float, based of the magic number
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public unsafe static float FastInvSqrt(float x)
    {
        float xhalf = 0.5f * x;
        int i = *(int*)&x;
        i = 0x5f375a86 - (i >> 1); //this constant is slightly more accurate than the common one
        x = *(float*)&i;
        x = x * (1.5f - xhalf * x * x);
        return (x);
    }

    /// <summary>
    /// Using the magic of 0x5f3759df
    /// </summary>
    /// <param name="vec1"></param>
    /// <returns></returns>
    public static Vector3 FastNormalized(this Vector3 vec1)
    {
        var componentMult = FastInvSqrt(vec1.sqrMagnitude);
        return new Vector3(vec1.x * componentMult, vec1.y * componentMult, vec1.z * componentMult);
    }

    /// <summary>
    /// Gets the normal of the triangle formed by the 3 vectors
    /// </summary>
    /// <param name="vec1"></param>
    /// <param name="vec2"></param>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3 GetNormalFromTriangle(Vector3 vec1, Vector3 vec2, Vector3 vec3)
    {
        return Vector3.Cross((vec3 - vec1), (vec2 - vec1));
    }

    /// <summary>
    /// Calculates the intersection line segment between 2 lines (not segments).
    /// Returns false if no solution can be found.
    /// </summary>
    /// <returns></returns>
    public static bool CalculateLineLineIntersection(Vector3 line1Point1, Vector3 line1Point2,
        Vector3 line2Point1, Vector3 line2Point2, out Vector3 resultSegmentPoint1, out Vector3 resultSegmentPoint2)
    {
        // Algorithm is ported from the C algorithm of 
        // Paul Bourke at http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline3d/
        resultSegmentPoint1 = new Vector3(0, 0, 0);
        resultSegmentPoint2 = new Vector3(0, 0, 0);

        var p1 = line1Point1;
        var p2 = line1Point2;
        var p3 = line2Point1;
        var p4 = line2Point2;
        var p13 = p1 - p3;
        var p43 = p4 - p3;

        if (p4.sqrMagnitude < float.Epsilon)
        {
            return false;
        }
        var p21 = p2 - p1;
        if (p21.sqrMagnitude < float.Epsilon)
        {
            return false;
        }

        var d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
        var d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
        var d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
        var d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
        var d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

        var denom = d2121 * d4343 - d4321 * d4321;
        if (Mathf.Abs(denom) < float.Epsilon)
        {
            return false;
        }
        var numer = d1343 * d4321 - d1321 * d4343;

        var mua = numer / denom;
        var mub = (d1343 + d4321 * (mua)) / d4343;

        resultSegmentPoint1.x = p1.x + mua * p21.x;
        resultSegmentPoint1.y = p1.y + mua * p21.y;
        resultSegmentPoint1.z = p1.z + mua * p21.z;
        resultSegmentPoint2.x = p3.x + mub * p43.x;
        resultSegmentPoint2.y = p3.y + mub * p43.y;
        resultSegmentPoint2.z = p3.z + mub * p43.z;

        return true;
    }
    #endregion

    #region Get middle & direction
    //Gets an XY direction of magnitude from a radian angle relative to the x axis
    //Simple version
    public static Vector3 GetXYDirection(float angle, float magnitude)
    {
        return (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * magnitude);
    }

    public static Vector3 GetMiddleOf2Vector(Vector3 a, Vector3 b)
    {
        return ((a + b).normalized);
    }
    public static Vector3 GetMiddleOfXVector(ContactPoint[] arrayVect)
    {
        Vector3[] arrayTmp = new Vector3[arrayVect.Length];

        Vector3 sum = Vector3.zero;
        for (int i = 0; i < arrayVect.Length; i++)
        {
            arrayTmp[i] = arrayVect[i].normal;
        }
        return (GetMiddleOfXVector(arrayTmp));
    }

    public static Vector3 GetMiddleOfXVector(Vector3[] arrayVect)
    {
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < arrayVect.Length; i++)
        {
            if (ExtVector3.IsNullVector(arrayVect[i]))
                continue;

            sum += arrayVect[i];
        }
        return ((sum).normalized);
    }
    /// <summary>
    /// return the middle of X points (POINTS, NOT vector)
    /// </summary>
    public static Vector3 GetMiddleOfXPoint(Vector3[] arrayVect, bool barycenter = true)
    {
        if (arrayVect.Length == 0)
            return (ExtVector3.GetNullVector());

        if (!barycenter)
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < arrayVect.Length; i++)
            {
                sum += arrayVect[i];
            }
            return (sum / arrayVect.Length);
        }
        else
        {
            if (arrayVect.Length == 1)
                return (arrayVect[0]);

            float xMin = arrayVect[0].x;
            float yMin = arrayVect[0].y;
            float zMin = arrayVect[0].z;
            float xMax = arrayVect[0].x;
            float yMax = arrayVect[0].y;
            float zMax = arrayVect[0].z;

            for (int i = 1; i < arrayVect.Length; i++)
            {
                if (arrayVect[i].x < xMin)
                    xMin = arrayVect[i].x;
                if (arrayVect[i].x > xMax)
                    xMax = arrayVect[i].x;

                if (arrayVect[i].y < yMin)
                    yMin = arrayVect[i].y;
                if (arrayVect[i].y > yMax)
                    yMax = arrayVect[i].y;

                if (arrayVect[i].z < zMin)
                    zMin = arrayVect[i].z;
                if (arrayVect[i].z > zMax)
                    zMax = arrayVect[i].z;
            }
            Vector3 lastMiddle = new Vector3((xMin + xMax) / 2, (yMin + yMax) / 2, (zMin + zMax) / 2);
            return (lastMiddle);
        }
    }

    /// <summary>
    /// get la bisection de 2 vecteur
    /// </summary>
    public static Vector3 GetbisectionOf2Vector(Vector3 a, Vector3 b)
    {
        return ((a + b) * 0.5f);
    }

    /// <summary>
    /// is a vector is in the same direction of another, with a given precision
    /// </summary>
    /// <param name="direction">vector 1</param>
    /// <param name="otherDirection">vector 2</param>
    /// <param name="precision">precision</param>
    /// <param name="normalizeParameters">Should normalise the vector first</param>
    /// <returns>is in the same direction</returns>
    public static bool IsInDirection(Vector3 direction, Vector3 otherDirection, float precision, bool normalizeParameters = true)
    {
        if (normalizeParameters)
        {
            direction.Normalize();
            otherDirection.Normalize();
        }
        return Vector3.Dot(direction, otherDirection) > 0f + precision;
    }
    public static bool IsInDirection(Vector3 direction, Vector3 otherDirection)
    {
        return Vector3.Dot(direction, otherDirection) > 0f;
    }


    public static Vector3 ClosestGeneralDirection(Vector3 vector) { return ClosestGeneralDirection(vector, GeneralDirections); }
    public static Vector3 ClosestGeneralDirection(Vector3 vector, IList<Vector3> directions)
    {
        float maxDot = float.MinValue;
        int closestDirectionIndex = 0;

        for (int i = 0; i < directions.Count; i++)
        {
            float dot = Vector3.Dot(vector, directions[i]);
            if (dot > maxDot)
            {
                closestDirectionIndex = i;
                maxDot = dot;
            }
        }

        return directions[closestDirectionIndex];
    }

    /// <summary>
    /// return an angle in degree between 2 vector, based on an axis
    /// </summary>
    /// <param name="dir1"></param>
    /// <param name="dir2"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static float AngleAroundAxis(Vector3 dir1, Vector3 dir2, Vector3 axis)
    {
        dir1 = dir1 - Vector3.Project(dir1, axis);
        dir2 = dir2 - Vector3.Project(dir2, axis);

        float angle = Vector3.Angle(dir1, dir2);
        return angle * (Vector3.Dot(axis, Vector3.Cross(dir1, dir2)) < 0 ? -1 : 1);
    }

    #endregion

    #region angle
    /// <summary>
    /// get an angle in degree using 2 vector
    /// (must be normalized)
    /// </summary>
    /// <param name="from">vector 1</param>
    /// <param name="to">vector 2</param>
    /// <returns></returns>
    public static float Angle(Vector3 from, Vector3 to)
    {
        return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from, to), -1f, 1f)) * Mathf.Rad2Deg;
    }


    /// <summary>
    /// prend un vecteur2 et retourne l'angle x, y en degré
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static float GetAngleFromVector2(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));       //Cross for testing -1, 0, 1
        //float signed_angle = angle * sign;                                  // angle in [-179,180]
        float angle360 = (angle + 360) % 360;                       // angle in [0,360]
        return (angle360);

        //return (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    /// <summary>
    /// Get Vector2 from angle
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector2 AngleToVector2(float a, bool useRadians = false, bool yDominant = false)
    {
        if (!useRadians) a *= Mathf.Rad2Deg;
        if (yDominant)
        {
            return new Vector2(Mathf.Sin(a), Mathf.Cos(a));
        }
        else
        {
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        }
    }
    /// <summary>
    /// return an angle from a vector
    /// </summary>
    public static float GetAngleFromVector3(Vector3 dir, Vector3 reference)
    {
        float angle = Vector3.Angle(dir, reference);
        return (angle);
    }

    /// <summary>
    /// check la différence d'angle entre les 2 vecteurs
    /// </summary>
    public static float GetDiffAngleBetween2Vectors(Vector2 dir1, Vector2 dir2)
    {
        float angle1 = GetAngleFromVector2(dir1);
        float angle2 = GetAngleFromVector2(dir2);

        float diffAngle;
        IsAngleCloseToOtherByAmount(angle1, angle2, 10f, out diffAngle);
        return (diffAngle);
    }

    /// <summary>
    /// prend un angle A, B, en 360 format, et test si les 2 angles sont inférieur à différence (180, 190, 20 -> true, 180, 210, 20 -> false)
    /// </summary>
    /// <param name="angleReference">angle A</param>
    /// <param name="angleToTest">angle B</param>
    /// <param name="differenceAngle">différence d'angle accepté</param>
    /// <returns></returns>
    public static bool IsAngleCloseToOtherByAmount(float angleReference, float angleToTest, float differenceAngle, out float diff)
    {
        if (angleReference < 0 || angleReference > 360 ||
            angleToTest < 0 || angleToTest > 360)
        {
            Debug.LogError("angle non valide: " + angleReference + ", " + angleToTest);
        }

        diff = 180 - Mathf.Abs(Mathf.Abs(angleReference - angleToTest) - 180);

        //diff = Mathf.Abs(angleReference - angleToTest);        

        if (diff <= differenceAngle)
            return (true);
        return (false);
    }
    public static bool IsAngleCloseToOtherByAmount(float angleReference, float angleToTest, float differenceAngle)
    {
        if (angleReference < 0 || angleReference > 360 ||
            angleToTest < 0 || angleToTest > 360)
        {
            Debug.LogError("angle non valide: " + angleReference + ", " + angleToTest);
        }

        float diff = 180 - Mathf.Abs(Mathf.Abs(angleReference - angleToTest) - 180);

        //diff = Mathf.Abs(angleReference - angleToTest);        

        if (diff <= differenceAngle)
            return (true);
        return (false);
    }

    /// <summary>
    /// retourne un vecteur2 par rapport à un angle
    /// </summary>
    /// <param name="angle"></param>
    public static Vector3 GetVectorFromAngle(float angle)
    {
        return (new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0));
    }

    /// <summary>
    /// renvoi l'angle entre deux vecteur, avec le 3eme vecteur de référence
    /// </summary>
    /// <param name="a">vecteur A</param>
    /// <param name="b">vecteur B</param>
    /// <param name="n">reference</param>
    /// <returns>Retourne un angle en degré</returns>
    public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        float angle = Vector3.Angle(a, b);                                  // angle in [0,180]
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));       //Cross for testing -1, 0, 1
        float signed_angle = angle * sign;                                  // angle in [-179,180]
        float angle360 = (signed_angle + 360) % 360;                       // angle in [0,360]
        return (angle360);
    }
    #endregion
}
