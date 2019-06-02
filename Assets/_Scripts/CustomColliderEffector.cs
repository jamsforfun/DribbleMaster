using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomColliderEffector : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _sideCollider;
	private Dictionary<int, BoxCollider2D> _subColliders;
	[SerializeField] private Transform _subCollidersBearer;
	public float ColliderSideInWorldSpace = 1;

	private void OnEnable()
	{
		_subColliders = new Dictionary<int, BoxCollider2D>();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.CompareTag("Player"))
		{
			return;
		}

		ClearSubColliders();

		List<float> collisionXs = new List<float>();
		collisionXs.Add(-ColliderSideInWorldSpace / 2);
		collisionXs.Add(ColliderSideInWorldSpace / 2);

		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			if (Vector2.Dot(contactPoint.normal, transform.up) > 0)
			{
				collisionXs.Add(transform.InverseTransformPoint(contactPoint.point).x);
			}
		}
		collisionXs.Sort();
		
		for (int index = 1; index < collisionXs.Count; index++)
		{
			BoxCollider2D subCollider = _subCollidersBearer.gameObject.AddComponent<BoxCollider2D>();
			float offsetX = (collisionXs[index] + collisionXs[index - 1]) / 2;
			float sizeX = collisionXs[index] - collisionXs[index - 1];
			subCollider.offset = new Vector2(offsetX, 0);
			subCollider.size = new Vector2(sizeX, FrameSizer.SPRITE_SIZE_IN_WORLD_SPACE / 2);
			_subColliders.Add(index-1, subCollider);

		}

		//set the middle subcollider as trigger
		_subColliders[1].isTrigger = true;

		_sideCollider.enabled = false;
	}

	public void OnCollisionExitChild(Collider2D collider)
	{
		if (!_sideCollider.enabled && !collider.gameObject.CompareTag("Player"))
		{
			ClearSubColliders();
			_sideCollider.enabled = true;
		}
	}

	private void ClearSubColliders()
	{
		foreach (BoxCollider2D collider in new List<BoxCollider2D>(_subColliders.Values))
		{
			Destroy(collider);
		}
		_subColliders.Clear();
	}
}
