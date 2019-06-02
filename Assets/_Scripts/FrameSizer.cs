using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FrameSizer : MonoBehaviour
{
    public enum AmountPlayer
    {
        ONE = 1,
        TWO = 2,
        TREE = 3,
        FOUR = 4,
        MORE = 5,
        LOCKED = 10,
    }

    

    [FoldoutGroup("GamePlay"), SerializeField, ReadOnly]
    public AmountPlayer AmountPlayerNeeded = AmountPlayer.ONE;
    [FoldoutGroup("GamePlay"), SerializeField]
    public FrameSizerSettings FrameSizerSettings;

    [SerializeField, OnValueChanged("Init")] private float _xScale = 1;
	[SerializeField, OnValueChanged("Init")] private float _yScale = 1;
    [SerializeField, ReadOnly] private float _air = 0;

    [FoldoutGroup("Frame sides"), SerializeField] private Transform _top;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _right;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _bottom;
	[FoldoutGroup("Frame sides"), SerializeField] private Transform _left;

	private float _frameSide;
	private float _borderHalfWidth;
	private Vector2 _frameCenter;
	private const float NORMAL_SCALE = 10;

    /// <summary>
    /// determine, with the number of player pushing given, we can push us !
    /// </summary>
    /// <param name="numberOfPLayerPushing">number of player pushing this</param>
    /// <returns>true if we can be pushed</returns>
    public bool CanPushThis(int numberOfPLayerPushing)
    {
        //do nothing if we are locked
        if (AmountPlayerNeeded == AmountPlayer.LOCKED)
        {
            return (false);
        }

        //if the number of player is the same as AmountPlayerNeeded, ok
        if ((int)AmountPlayerNeeded == numberOfPLayerPushing)
        {

            return (true);
        }

        //if the number of player is more than 4, ok
        if (AmountPlayerNeeded == AmountPlayer.MORE && numberOfPLayerPushing > 4)
        {
            return (true);
        }

        return (false);
    }

    /// <summary>
    /// the the number of player required for pushing this
    /// </summary>
    /// <param name="numberOfPLayerPushing"></param>
    public void SetupNumberOfNeededPlayerForPushing()
    {
        if (AmountPlayerNeeded == AmountPlayer.LOCKED)
        {
            return;
        }

        if (_air <= FrameSizerSettings.AmountMinAirForPush.onePlayerAir)
        {
            AmountPlayerNeeded = AmountPlayer.ONE;
        }
        else if (_air <= FrameSizerSettings.AmountMinAirForPush.twoPlayerAir)
        {
            AmountPlayerNeeded = AmountPlayer.TWO;
        }
        else if (_air <= FrameSizerSettings.AmountMinAirForPush.treePlayerAir)
        {
            AmountPlayerNeeded = AmountPlayer.TREE;
        }
        else if (_air <= FrameSizerSettings.AmountMinAirForPush.fourPlayerAir)
        {
            AmountPlayerNeeded = AmountPlayer.FOUR;
        }
        else
        {
            AmountPlayerNeeded = AmountPlayer.MORE;
        }
    }

#if UNITY_EDITOR
    //[UnityEditor.Callbacks.DidReloadScripts]
    private void OnEnable()
    {
        Init();
    }

    [Button]
	private void Init()
	{
		_top.localPosition = new Vector3(92, 191, 0);
		_right.localPosition = new Vector3(190, 93, 0);
		_bottom.localPosition = new Vector3(92, -4, 0);
		_left.localPosition = new Vector3(-5, 93, 0);
		_frameSide = _top.localPosition.y - _bottom.localPosition.y;
		_borderHalfWidth = 3.5f;
		_frameCenter = (_top.localPosition + _bottom.localPosition) / 2;
		//Debug.DrawLine(transform.position, _frameCenter);

        ScaleXAxis();
        ScaleYAxis();

        _air = _xScale * _yScale;
        SetupNumberOfNeededPlayerForPushing();
    }

	private void ScaleXAxis()
	{
        _top.localScale = new Vector3(_xScale, _top.localScale.y, _top.localScale.z);
		_bottom.localScale = new Vector3(_xScale, _bottom.localScale.y, _bottom.localScale.z);

		float scaleRatio = _xScale / NORMAL_SCALE;
		if (scaleRatio > 1)
		{
			_right.localPosition = _frameCenter + scaleRatio * (_borderHalfWidth + _frameSide / 2) * Vector2.right;
			_left.localPosition = _frameCenter + scaleRatio * (_borderHalfWidth + _frameSide / 2) * Vector2.left;
		}
		else
		{
			_right.localPosition = _frameCenter + scaleRatio *  _frameSide/2  * Vector2.right;
			_left.localPosition = _frameCenter + scaleRatio * _frameSide/2 * Vector2.left;
		}
	}

	private void ScaleYAxis()
	{
		_right.localScale = new Vector3(_yScale, _right.localScale.y, _right.localScale.z);
		_left.localScale = new Vector3(_yScale, _left.localScale.y, _left.localScale.z);

		float scaleRatio = _yScale / NORMAL_SCALE;
		if (scaleRatio > 1)
		{
			_top.localPosition = _frameCenter + scaleRatio * (_borderHalfWidth + _frameSide / 2) * Vector2.up;
			_bottom.localPosition = _frameCenter + scaleRatio * (_borderHalfWidth + _frameSide / 2) * Vector2.down;
		}
		else
		{
			_top.localPosition = _frameCenter + scaleRatio * _frameSide / 2 * Vector2.up;
			_bottom.localPosition = _frameCenter + scaleRatio * _frameSide  / 2 * Vector2.down;
		}
	}
#endif
}
