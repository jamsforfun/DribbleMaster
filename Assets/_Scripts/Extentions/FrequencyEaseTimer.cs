using UnityEngine;

[System.Serializable]
public struct FrequencyEase
{
    [SerializeField, Tooltip("speed")]
    private float _speedIn;
    
    //[SerializeField, Tooltip("timer back to 0 speed (default 1)")]
    //private float ratioDecelerate;
    [SerializeField, Tooltip("curve, default: time: 0 to X; value: 0 to 1")]
    private AnimationCurve _animationCurve;

    [SerializeField, Tooltip("")]
    private bool _timerStarted;
    [SerializeField, Tooltip("")]
    private bool _timerIsEnding;

    [SerializeField, Tooltip("")]
    private float _timeStart;
    [SerializeField, Tooltip("")]
    private float _timeEnd;
    [SerializeField, Tooltip("")]
    private float _currentTime;

    private float _timeWhenStart;
    private float _previousTimeFrame;

    public void Init()
    {
        _speedIn = 5f;
        //ratioDecelerate = 1f;
        //ratioDecelerate = 2f;
        _animationCurve = new AnimationCurve();
        _animationCurve.AddKey(new Keyframe(0, 0));
        _animationCurve.AddKey(new Keyframe(1, 1));
        _animationCurve.postWrapMode = WrapMode.Clamp;
        _animationCurve.preWrapMode = WrapMode.Clamp;

        _timerStarted = false;
        _timerIsEnding = false;
        _timeStart = 0f;
        _timeEnd = 0f;
        _currentTime = 0f;
    }

    /// <summary>
    /// max: maxSecond
    /// </summary>
    public void StartCoolDown(float _maxSecond = 1)
    {
        _timeStart = _currentTime = 0;
        _timeEnd = _maxSecond;

        _timerStarted = true;
        _timerIsEnding = false;
        _timeWhenStart = Time.fixedTime;
        _previousTimeFrame = Time.fixedTime;
    }

    public float Evaluate()
    {
        return (_animationCurve.Evaluate(_currentTime) * Time.deltaTime * _speedIn);
    }

    /// <summary>
    /// start a cooldown, or continue it (clamp to the end !)
    /// </summary>
    /// <param name="_maxSecond"></param>
    public void StartOrContinue()
    {
        if (!_timerStarted)
        {
            StartCoolDown(_animationCurve.keys[_animationCurve.length - 1].time);
            return;
        }
        _timeEnd = _animationCurve.keys[_animationCurve.length - 1].time;
        AddOneFrame();
    }

    public void BackToTime()
    {
        if (!_timerStarted && !_timerIsEnding)
        {
            //Debug.Log("here go backward, but we did'nt go forward at first... do nothing");
            return;
        }

        if (_timerStarted && !_timerIsEnding)
        {
            //Debug.Log("here first time we go backward !");
            _timerIsEnding = true;
        }
        _timerStarted = false;

        RemoveOneFrame();

        if (_currentTime == 0)
        {
            _timerIsEnding = false;
        }        
    }

    private void AddOneFrame()
    {
        //float timePast = Time.fixedTime - timeWhenStart;
        float amountToAdd = Time.fixedTime - _previousTimeFrame;
        _currentTime += amountToAdd;

        _currentTime = Mathf.Clamp(_currentTime, 0, _timeEnd);

        //Debug.Log("timePast: " + currentTime);

        _previousTimeFrame = Time.fixedTime;
    }
    private void RemoveOneFrame()
    {
        float amountToDelete = Time.fixedTime - _previousTimeFrame;

        _currentTime -= amountToDelete;
        _currentTime = Mathf.Clamp(_currentTime, 0, _timeEnd);

        _previousTimeFrame = Time.fixedTime;
    }
    
}
