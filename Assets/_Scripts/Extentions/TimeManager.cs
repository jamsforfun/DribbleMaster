using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Unity 3D Time Manager
// You may encounter cases where multiple scripts may require the game to enter paused state(e.g.user interaction with pause button, opening a UI menu, waiting for an async operation to finish). This script helps you avoid having an isPaused variable in each of these scripts and/or checking all the isPaused variables before actually changing the value of Time.timeScale.
//
// Simply call TimeManager.Pause( this ); to pause the game and TimeManager.Unpause( this ); to try to unpause the game. If another object has also requested a pause, then timeScale will stay at 0 until that object unpauses the game as well. You can use any object while pausing the game (instead of this), just make sure to unpause the game with the same object.
//
// You can also register to TimeManager.OnPauseStateChanged to get notified when pause state changes and TimeManager.OnTimeScaleChanged to get notified when time scale changes. Note that you will have to use TimeManager.TimeScale instead of Time.timeScale in order to avoid actually changing time scale when the game is paused (and for OnTimeScaleChanged event to work, obviously).
/// </summary>
public static class TimeManager
{
    private static List<object> pauseHolders = new List<object>();

    public static event System.Action<bool> OnPauseStateChanged = null;
    public static event System.Action<float> OnTimeScaleChanged = null;

    public static bool IsPaused { get { return pauseHolders.Count > 0; } }

    private static float m_timeScale = 1f;
    public static float TimeScale
    {
        get
        {
            return m_timeScale;
        }
        set
        {
            if (value >= 0f && m_timeScale != value)
            {
                m_timeScale = value;

                if (pauseHolders.Count == 0)
                {
                    Time.timeScale = m_timeScale;

                    if (OnTimeScaleChanged != null)
                        OnTimeScaleChanged(m_timeScale);
                }
            }
        }
    }

    static TimeManager()
    {
        SceneManager.sceneUnloaded += OnSceneChanged;
    }

    private static void OnSceneChanged(Scene arg)
    {
        for (int i = pauseHolders.Count - 1; i >= 0; i--)
        {
            if (pauseHolders[i] == null || pauseHolders[i].Equals(null))
                pauseHolders.RemoveAt(i);
        }

        if (pauseHolders.Count == 0)
            Unpause();
    }

    public static void Pause(object pauseHolder)
    {
        if (!pauseHolders.Contains(pauseHolder))
        {
            pauseHolders.Add(pauseHolder);

            if (pauseHolders.Count == 1)
                Pause();
        }
    }

    public static void Unpause(object pauseHolder)
    {
        if (pauseHolders.Remove(pauseHolder) && pauseHolders.Count == 0)
            Unpause();
    }

    public static bool IsPausedBy(object pauseHolder)
    {
        return pauseHolders.Contains(pauseHolder);
    }

    private static void Pause()
    {
        Time.timeScale = 0f;

        if (OnPauseStateChanged != null)
            OnPauseStateChanged(true);

        if (OnTimeScaleChanged != null)
            OnTimeScaleChanged(0f);
    }

    private static void Unpause()
    {
        Time.timeScale = m_timeScale;

        if (OnPauseStateChanged != null)
            OnPauseStateChanged(false);

        if (OnTimeScaleChanged != null)
            OnTimeScaleChanged(m_timeScale);
    }
}