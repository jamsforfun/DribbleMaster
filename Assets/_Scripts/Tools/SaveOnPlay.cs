using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SaveOnPlay : MonoBehaviour
{
    public bool saveOnPlay = true;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            Debug.Log("enable save on play");
        }
        else
        {
            Debug.Log("enable save on editor");
        }

    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            Debug.Log("disable save on play");
        }
        else
        {
            Debug.Log("disable save on editor");
        }
    }
}
