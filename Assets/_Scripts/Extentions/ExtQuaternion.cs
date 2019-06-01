using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtQuaternion
{
    public enum TurnType
    {
        X,
        Y,
        Z,
        ALL,
    }

    #region Transform
    /// <summary>
    /// Turret lock rotation
    /// https://gamedev.stackexchange.com/questions/167389/unity-smooth-local-rotation-around-one-axis-oriented-toward-a-target/167395#167395
    /// 
    /// Vector3 relativeDirection = mainReferenceObjectDirection.right * dirInput.x + mainReferenceObjectDirection.forward * dirInput.y;
    /// Vector3 up = objectToRotate.up;
    /// Quaternion desiredOrientation = TurretLookRotation(relativeDirection, up);
    ///objectToRotate.rotation = Quaternion.RotateTowards(
    ///                         objectToRotate.rotation,
    ///                         desiredOrientation,
    ///                         turnRate* Time.deltaTime
    ///                        );
    /// </summary>
    public static Quaternion TurretLookRotation(Vector3 approximateForward, Vector3 exactUp)
    {
        Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, -approximateForward);
        Quaternion rotateYToZ = Quaternion.Euler(90f, 0f, 0f);

        return rotateZToUp * rotateYToZ;
    }
    public static Quaternion SmoothTurretLookRotation(Vector3 approximateForward, Vector3 exactUp,
        Quaternion objCurrentRotation, float maxDegreesPerSecond)
    {
        Quaternion desiredOrientation = TurretLookRotation(approximateForward, exactUp);
        Quaternion smoothOrientation = Quaternion.RotateTowards(
                                    objCurrentRotation,
                                    desiredOrientation,
                                    maxDegreesPerSecond * Time.deltaTime
                                 );
        return (smoothOrientation);
    }

    /// <summary>
    /// Create a LookRotation for a non-standard 'forward' axis.
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="forwardAxis"></param>
    /// <returns></returns>
    public static Quaternion AltForwardLookRotation(Vector3 dir, Vector3 forwardAxis, Vector3 upAxis)
    {
        //return Quaternion.LookRotation(dir, upAxis) * Quaternion.FromToRotation(forwardAxis, Vector3.forward);
        return Quaternion.LookRotation(dir) * Quaternion.Inverse(Quaternion.LookRotation(forwardAxis, upAxis));
    }

    /// <summary>
    /// Get the rotated forward axis based on some base forward.
    /// </summary>
    /// <param name="rot">The rotation</param>
    /// <param name="baseForward">Forward with no rotation</param>
    /// <returns></returns>
    public static Vector3 GetAltForward(Quaternion rot, Vector3 baseForward)
    {
        return rot * baseForward;
    }

    /// <summary>
    /// Returns a rotation of up attempting to face in the general direction of forward.
    /// </summary>
    /// <param name="up"></param>
    /// <param name="targForward"></param>
    /// <returns></returns>
    public static Quaternion FaceRotation(Vector3 forward, Vector3 up)
    {
        forward = ExtVector3.GetForwardTangent(forward, up);
        return Quaternion.LookRotation(forward, up);
    }

    public static void GetAngleAxis(this Quaternion q, out Vector3 axis, out float angle)
    {
        if (q.w > 1) q = ExtQuaternion.Normalize(q);

        //get as doubles for precision
        var qw = (double)q.w;
        var qx = (double)q.x;
        var qy = (double)q.y;
        var qz = (double)q.z;
        var ratio = System.Math.Sqrt(1.0d - qw * qw);

        angle = (float)(2.0d * System.Math.Acos(qw)) * Mathf.Rad2Deg;
        if (ratio < 0.001d)
        {
            axis = new Vector3(1f, 0f, 0f);
        }
        else
        {
            axis = new Vector3(
                (float)(qx / ratio),
                (float)(qy / ratio),
                (float)(qz / ratio));
            axis.Normalize();
        }
    }

    public static void GetShortestAngleAxisBetween(Quaternion a, Quaternion b, out Vector3 axis, out float angle)
    {
        var dq = Quaternion.Inverse(a) * b;
        if (dq.w > 1) dq = ExtQuaternion.Normalize(dq);

        //get as doubles for precision
        var qw = (double)dq.w;
        var qx = (double)dq.x;
        var qy = (double)dq.y;
        var qz = (double)dq.z;
        var ratio = System.Math.Sqrt(1.0d - qw * qw);

        angle = (float)(2.0d * System.Math.Acos(qw)) * Mathf.Rad2Deg;
        if (ratio < 0.001d)
        {
            axis = new Vector3(1f, 0f, 0f);
        }
        else
        {
            axis = new Vector3(
                (float)(qx / ratio),
                (float)(qy / ratio),
                (float)(qz / ratio));
            axis.Normalize();
        }
    }
    #endregion

    /// <summary>
    /// is a quaternion NaN
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public static bool IsNaN(Quaternion q)
    {
        return float.IsNaN(q.x * q.y * q.z * q.w);
    }

    /// <summary>
    /// Lerp a rotation
    /// </summary>
    /// <param name="currentRotation"></param>
    /// <param name="desiredRotation"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static Quaternion LerpRotation(Quaternion currentRotation, Quaternion desiredRotation, float speed)
    {
        return (Quaternion.Lerp(currentRotation, desiredRotation, Time.time * speed));
    }
    
    /// <summary>
    /// clamp a quaternion around one local axis
    /// </summary>
    /// <param name="q"></param>
    /// <param name="minX"></param>
    /// <param name="maxX"></param>
    /// <returns></returns>
    public static Quaternion ClampRotationAroundXAxis(Quaternion q, float minX, float maxX)
    {
        if (q.w == 0)
            return (q);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    
    public static Quaternion ClampRotationAroundYAxis(Quaternion q, float minY, float maxY)
    {
        if (q.w == 0)
            return (q);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

        angleY = Mathf.Clamp(angleY, minY, maxY);

        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        return q;
    }

    public static Quaternion ClampRotationAroundZAxis(Quaternion q, float minZ, float maxZ)
    {
        if (q.w == 0)
            return (q);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);

        angleZ = Mathf.Clamp(angleZ, minZ, maxZ);

        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return q;
    }

    public static bool IsCloseYToClampAmount(Quaternion q, float minY, float maxY, float margin = 2)
    {
        if (q.w == 0)
            return (true);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

        if (ExtMathf.IsClose(angleY, minY, margin)
            || ExtMathf.IsClose(angleY, maxY, margin))
        {
            return (true);
        }
        return (false);
    }
    public static bool IsCloseZToClampAmount(Quaternion q, float minZ, float maxZ, float margin = 2)
    {
        if (q.w == 0)
            return (true);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);

        if (ExtMathf.IsClose(angleZ, minZ, margin)
            || ExtMathf.IsClose(angleZ, maxZ, margin))
        {
            return (true);
        }
        return (false);
    }
    public static bool IsCloseXToClampAmount(Quaternion q, float minX, float maxX, float margin = 2)
    {
        if (q.w == 0)
            return (true);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        if (ExtMathf.IsClose(angleX, minX, margin)
            || ExtMathf.IsClose(angleX, maxX, margin))
        {
            return (true);
        }
        return (false);
    }

    
    /// <summary>
    /// rotate an object in 2D coordonate
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="dir"></param>
    /// <param name="turnRate"></param>
    /// <param name="typeRotation"></param>
    /// <returns></returns>
    public static Quaternion DirObject2d(this Quaternion rotation, Vector2 dir, float turnRate, TurnType typeRotation = TurnType.Z, bool inverseX = false, bool inverseY = false, bool isPhysics = false)
    {
        float heading = Mathf.Atan2(dir.x * (inverseX ? -1 : 1) * turnRate, dir.y * (inverseY ? -1 : 1) * turnRate);

        Quaternion _targetRotation = Quaternion.identity;

        float x = (typeRotation == TurnType.X) ? heading * 1 * Mathf.Rad2Deg : 0;
        float y = (typeRotation == TurnType.Y) ? heading * -1 * Mathf.Rad2Deg : 0;
        float z = (typeRotation == TurnType.Z) ? heading * -1 * Mathf.Rad2Deg : 0;

        _targetRotation = Quaternion.Euler(x, y, z);
        rotation = Quaternion.RotateTowards(rotation, _targetRotation, turnRate * (isPhysics ? Time.fixedDeltaTime : Time.deltaTime));
        return (rotation);
    }


    /// <summary>
    /// rotate smoothly selon 2 axe
    /// </summary>
	public static Quaternion DirObject(this Quaternion rotation, float horizMove, float vertiMove, float turnRate, out Quaternion _targetRotation, TurnType typeRotation = TurnType.Z)
    {
        float heading = Mathf.Atan2(horizMove * turnRate * Time.deltaTime, -vertiMove * turnRate * Time.deltaTime);

        //Quaternion _targetRotation = Quaternion.identity;

        float x = (typeRotation == TurnType.X) ? heading * 1 * Mathf.Rad2Deg : 0;
        float y = (typeRotation == TurnType.Y) ? heading * -1 * Mathf.Rad2Deg : 0;
        float z = (typeRotation == TurnType.Z) ? heading * -1 * Mathf.Rad2Deg : 0;

        _targetRotation = Quaternion.Euler(x, y, z);
        rotation = Quaternion.RotateTowards(rotation, _targetRotation, turnRate * Time.deltaTime);
        return (rotation);
    }

    public static Vector3 DirLocalObject(Vector3 rotation, Vector3 dirToGo, float turnRate, TurnType typeRotation = TurnType.Z)
    {
        Vector3 returnRotation = rotation;
        float x = returnRotation.x;
        float y = returnRotation.y;
        float z = returnRotation.z;

        //Debug.Log("Y current: " + y + ", y to go: " + dirToGo.y);



        x = (typeRotation == TurnType.X || typeRotation == TurnType.ALL) ? Mathf.LerpAngle(returnRotation.x, dirToGo.x, Time.deltaTime * turnRate) : x;
        y = (typeRotation == TurnType.Y || typeRotation == TurnType.ALL) ? Mathf.LerpAngle(returnRotation.y, dirToGo.y, Time.deltaTime * turnRate) : y;
        z = (typeRotation == TurnType.Z || typeRotation == TurnType.ALL) ? Mathf.LerpAngle(returnRotation.z, dirToGo.z, Time.deltaTime * turnRate) : z;

        //= Vector3.Lerp(rotation, dirToGo, Time.deltaTime * turnRate);
        return (new Vector3(x, y, z));
    }

    /// <summary>
    /// rotate un quaternion selon un vectir directeur
    /// use: transform.rotation.LookAtDir((transform.position - target.transform.position) * -1);
    /// </summary>
    public static Quaternion LookAtDir(Vector3 dir)
    {
        Quaternion rotation = Quaternion.LookRotation(dir * -1);
        return (rotation);
    }
    
    /// <summary>
    /// prend un quaternion en parametre, et retourn une direction selon un repère
    /// </summary>
    /// <param name="quat">rotation d'un transform</param>
    /// <param name="up">Vector3.up</param>
    /// <returns>direction du quaternion</returns>
    public static Vector3 QuaternionToDir(Quaternion quat, Vector3 up)
    {
        return ((quat * up).normalized);
    }

    public static Quaternion DirToQuaternion(Vector3 dir)
    {
        return (Quaternion.Euler(dir));
    }

    /// <summary>
    /// A cleaner version of FromToRotation, Quaternion.FromToRotation for some reason can only handle down to #.## precision.
    /// This will result in true 7 digits of precision down to depths of 0.00000# (depth tested so far).
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Quaternion FromToRotation(Vector3 v1, Vector3 v2)
    {
        var a = Vector3.Cross(v1, v2);
        var w = Mathf.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude) + Vector3.Dot(v1, v2);
        return new Quaternion(a.x, a.y, a.z, w);
    }

    /// <summary>
    /// Get the rotation that would be applied to 'start' to end up at 'end'.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Quaternion FromToRotation(Quaternion start, Quaternion end)
    {
        return Quaternion.Inverse(start) * end;
    }

    public static Quaternion SpeedSlerp(Quaternion from, Quaternion to, float angularSpeed, float dt, bool bUseRadians = false)
    {
        if (bUseRadians) angularSpeed *= Mathf.Rad2Deg;
        var da = angularSpeed * dt;
        return Quaternion.RotateTowards(from, to, da);
    }

    #region misc
    /// <summary>
    /// normalise a quaternion
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public static Quaternion Normalize(Quaternion q)
    {
        var mag = System.Math.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
        q.w = (float)((double)q.w / mag);
        q.x = (float)((double)q.x / mag);
        q.y = (float)((double)q.y / mag);
        q.z = (float)((double)q.z / mag);
        return q;
    }

    ///returns quaternion raised to the power pow. This is useful for smoothly multiplying a Quaternion by a given floating-point value.
    ///transform.rotation = rotateOffset.localRotation.Pow(Time.time);
    public static Quaternion Pow(this Quaternion input, float power)
	{
		float inputMagnitude = input.Magnitude();
		Vector3 nHat = new Vector3(input.x, input.y, input.z).normalized;
		Quaternion vectorBit = new Quaternion(nHat.x, nHat.y, nHat.z, 0)
			.ScalarMultiply(power * Mathf.Acos(input.w / inputMagnitude))
				.Exp();
		return vectorBit.ScalarMultiply(Mathf.Pow(inputMagnitude, power));
	}
 
    ///returns euler's number raised to quaternion
	public static Quaternion Exp(this Quaternion input)
	{
		float inputA = input.w;
		Vector3 inputV = new Vector3(input.x, input.y, input.z);
		float outputA = Mathf.Exp(inputA) * Mathf.Cos(inputV.magnitude);
		Vector3 outputV = Mathf.Exp(inputA) * (inputV.normalized * Mathf.Sin(inputV.magnitude));
		return new Quaternion(outputV.x, outputV.y, outputV.z, outputA);
	}
 
    ///returns the float magnitude of quaternion
	public static float Magnitude(this Quaternion input)
	{
		return Mathf.Sqrt(input.x * input.x + input.y * input.y + input.z * input.z + input.w * input.w);
	}
 
    ///returns quaternion multiplied by scalar
	public static Quaternion ScalarMultiply(this Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}
    #endregion

    #region String

    public static string Stringify(Quaternion q)
    {
        return q.x.ToString() + "," + q.y.ToString() + "," + q.z.ToString() + "," + q.w.ToString();
    }

    public static string ToDetailedString(this Quaternion v)
    {
        return System.String.Format("<{0}, {1}, {2}, {3}>", v.x, v.y, v.z, v.w);
    }

    #endregion
}
