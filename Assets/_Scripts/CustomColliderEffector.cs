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
		if (collision.contactCount <= 1)
		{
			return;
		}
		List<float> collisionXs = new List<float>();
		collisionXs.Add(-ColliderSideInWorldSpace / 2);
		collisionXs.Add(ColliderSideInWorldSpace / 2);

		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			if (Vector2.Dot(contactPoint.normal, transform.up) > 0)
			{
				ExtDrawGuizmos.DebugWireSphere(contactPoint.point, Color.red, 1, 2, false);
				collisionXs.Add(transform.InverseTransformPoint(contactPoint.point).x);
			}
		}
		collisionXs.Sort();

		foreach(float f in collisionXs)
		{
			ExtDrawGuizmos.DebugWireSphere(transform.TransformPoint(new Vector3(f, 0)), Color.red, 1, 100, false);
		}
		
		for (int index = 1; index < collisionXs.Count; index++)
		{
			BoxCollider2D subCollider = _subCollidersBearer.gameObject.AddComponent<BoxCollider2D>();
			float offsetX = (collisionXs[index] + collisionXs[index - 1]) / 2;
			float sizeX = collisionXs[index] - collisionXs[index - 1];
			subCollider.offset = new Vector2(offsetX, 0);
			subCollider.size = new Vector2(sizeX, FrameSizer.SPRITE_SIZE_IN_WORLD_SPACE);
			_subColliders.Add(index, subCollider);

		}

		_sideCollider.enabled = false;
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Exit");
		foreach(BoxCollider2D collider in _subColliders.Values)
		{
			collider.DestroyComponent<BoxCollider2D>();
		}

		_sideCollider.enabled = true;
	}
}
