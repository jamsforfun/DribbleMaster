using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtRigidbody
{
    /// <summary>
    /// effectue une force constante en direction voulu
    /// </summary>
    public static void ApplyConstForce(Rigidbody rb, Vector3 dir, float force)
    {
        //Debug.Log("here constant force");
        Debug.DrawRay(rb.transform.position, dir.normalized * 0.5f, Color.cyan, 0.1f);
        //rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        //Debug.Log("ici ajout petite force");
        rb.velocity += dir * Physics.gravity.y * (force - 1) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Allow for speed limit constant move
    /// </summary>
    public static void AccelerateTo(this Rigidbody body, Vector3 targetVelocity, float maxAccel, ForceMode forceMode = ForceMode.Acceleration)
	{
		Vector3 deltaV = targetVelocity - body.velocity;
		Vector3 accel = deltaV / Time.fixedDeltaTime;

		if (accel.sqrMagnitude > maxAccel * maxAccel)
			accel = accel.normalized * maxAccel;

        Debug.DrawRay(body.transform.position, accel, Color.blue, 1f);
        body.AddForce(accel, forceMode);
	}

    public static Rigidbody ClearVelocity(this Rigidbody rigid)
    {
        rigid.velocity = Vector3.zero;
        //rigid.angularDrag = 0;
        //rigid.angularVelocity = Vector3.zero;
        return (rigid);
    }

    /// <summary>
    /// Changes the direction of a rigidbody without changing its speed.
    /// </summary>
    /// <param name="rigidbody">Rigidbody.</param>
    /// <param name="direction">New direction.</param>
    public static void ChangeDirection(this Rigidbody rigidbody, Vector3 direction)
    {
        rigidbody.velocity = direction * rigidbody.velocity.magnitude;
    }

    /// <summary>
    /// Return kinectic energy of a body
    /// </summary>
    public static float KineticEnergy(this Rigidbody rb)
	{
		// mass in kg
		// velocity in meters per second
		// result is joules
		return (0.5f * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2));
	}
}
