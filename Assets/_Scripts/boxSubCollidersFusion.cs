using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSubCollidersFusion : MonoBehaviour
{
	[SerializeField] private CustomColliderEffector _colliderEffector;

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (!collision.collider.gameObject.CompareTag("Player"))
		{
			Debug.Log("collisionExit");
		}
		_colliderEffector.OnCollisionExitChild(collision.collider);
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (!collider.gameObject.CompareTag("Player"))
		{
			Debug.Log("TriggerExit");
		}
		_colliderEffector.OnCollisionExitChild(collider);
	}
}
