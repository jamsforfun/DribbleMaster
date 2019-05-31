using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculate next ready update
/// <summary>
[System.Serializable]
public class FrequencyTimer
{
	[SerializeField]
	private float updateFrequency;
    [SerializeField]
    private bool notTheFirstTime = false;   //ne s'exécute pas la première fois ?
    [SerializeField]
    private bool waitOnlyOnce = false;      //s'exécute tout le temps à partir du moment ou il a été exécuté une fois

    private float nextUpdate;
    private bool hasBeenReady = false;

	public FrequencyTimer(float updateFrequency)
	{
		this.updateFrequency = updateFrequency;
	}

    public void Reset()
    {
        hasBeenReady = false;
    }

    /// <summary>
    /// lorsque le timer est pret, renvoyer true
    /// </summary>
	public bool Ready(bool setReadyWhenTest = true)
	{

        if (waitOnlyOnce && hasBeenReady)
            return (true);

		if (Time.fixedTime >= nextUpdate)
		{

            if (notTheFirstTime)
            {
                notTheFirstTime = false;
                nextUpdate = Time.fixedTime + updateFrequency;
                return (false);
            }

            if (!setReadyWhenTest)  //si on test
            {
                if (hasBeenReady)   //ici on est pret, et on a déja été solicité
                {
                    return (true);
                }
                else
                {
                    return (false);
                }

            }

			nextUpdate = Time.fixedTime + updateFrequency;
            hasBeenReady = true;
			return (true);
		}
		return (false);
	}
}
