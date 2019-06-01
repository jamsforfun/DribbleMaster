using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FrameSizer : MonoBehaviour
{
	[SerializeField, OnValueChanged("ScaleXAxis")] private float _xScale = 1;
	[SerializeField, OnValueChanged("ScaleYAxis")] private float _yScale = 1;

	[FoldoutGroup("Frame sides"), SerializeField] private Transform _top;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _right;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _bottom;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _left;

	private float _frameSide;
	private float _borderHalfWidth;
	private Vector2 _frameCenter;
	private const float NORMAL_SCALE = 10;

	private void Awake()
	{
		_frameSide = _top.localPosition.y - _bottom.localPosition.y;
		_borderHalfWidth = Mathf.Abs(_bottom.localPosition.y);
		_frameCenter = (_top.localPosition + _bottom.localPosition) / 2;
	}

	private void ScaleXAxis()
	{
		_top.localScale = new Vector3(_xScale, _top.localScale.y, _top.localScale.z);
		_bottom.localScale = new Vector3(_xScale, _bottom.localScale.y, _bottom.localScale.z);

		_right.localPosition = _frameCenter + _xScale / NORMAL_SCALE * (_borderHalfWidth + _frameSide / 2) * Vector2.right;
		_left.localPosition = _frameCenter + _xScale / NORMAL_SCALE * (_borderHalfWidth + _frameSide / 2) * Vector2.left;
		Debug.Log(_borderHalfWidth);
	}

	private void ScaleYAxis()
	{
		_right.localScale = new Vector3(_yScale, _right.localScale.y, _right.localScale.z);
		_left.localScale = new Vector3(_yScale, _left.localScale.y, _left.localScale.z);

		_top.localPosition = _frameCenter + _yScale / NORMAL_SCALE * (_borderHalfWidth + _frameSide / 2) * Vector2.up;
		_bottom.localPosition = _frameCenter + _yScale / NORMAL_SCALE * (_borderHalfWidth + _frameSide / 2) * Vector2.down;
	}
}
