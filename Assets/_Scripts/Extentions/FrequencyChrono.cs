using UnityEngine;

[System.Serializable]
public class FrequencyChrono
{
    private float _timeStart = 0;
    private bool _loop = false;
    private float _maxTime = 0;
    private float _saveTime = 0;
    private bool isInPause = false;
    private bool isOver = false;

    private float currentTime;

    /// <summary>
    /// 
    /// </summary>
    public void StartCoolDown()
    {
        _timeStart = Time.fixedTime;
        _loop = false;
        isInPause = false;
        isOver = false;
    }

    public void StartCoolDown(float maxTime, bool loop)
    {
        _timeStart = Time.fixedTime;
        _loop = loop;
        _maxTime = maxTime;
        isInPause = false;
        isOver = false;
    }
    
    /// <summary>
    /// return actual time
    /// </summary>
    public float GetTimer(bool managePause = true)
    {
        if (managePause && isInPause)
        {
            return (_saveTime);
        }

        currentTime = Time.time - _timeStart;

        if (_loop)
        {
            currentTime %= _maxTime;
        }
        else if (currentTime > _maxTime)
        {
            currentTime = _maxTime;
        }

        return (currentTime);
    }

    public float GetMaxTime()
    {
        return (_maxTime);
    }

    public void ChangeMaxTime(float newMaxTime)
    {
        _maxTime = newMaxTime;
    }

    public bool IsReady()
    {
        currentTime = Time.time - _timeStart;
        return (currentTime >= _maxTime);
    }
    /// <summary>
    /// return true one at the end
    /// </summary>
    /// <returns></returns>
    public bool IsStartedAndOver()
    {
        if (!isOver && IsReady())
        {
            isOver = true;
            return (true);
        }
        return (false);
    }

    public void ManualForward()
    {
        _timeStart -= 0.016f;
        _saveTime += 0.016f;
    }

    public void ManualBackward()
    {
        _timeStart += 0.16f;
        _saveTime -= 0.016f;
    }

    public void Pause()
    {
        _saveTime = GetTimer(false);
        isInPause = true;
    }
    public void Resume()
    {
        isInPause = false;
        _timeStart = Time.fixedTime - _saveTime;
    }

    public float GetMinutes()
    {
        float time = Time.time - _timeStart;
        int minutes = (int)(time / 60);
        return (minutes);
    }
    public float GetSecondes()
    {
        float time = Time.time - _timeStart;
        //int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return (seconds);
    }
}
