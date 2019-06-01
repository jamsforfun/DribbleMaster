using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomColliderEffector : MonoBehaviour
{
    public void OnCollisionStay2D(Collision2D collision)
	{
		foreach(ContactPoint2D contactPoint in collision.contacts)
		{
			ExtDrawGuizmos.DebugWireSphere(contactPoint.point, Color.red, 2, 1, false);
		}
	}
}
