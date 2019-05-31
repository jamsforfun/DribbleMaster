using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTransition : SingletonMono<SceneTransition>
{
    public string pathScene = "Assets/_Scenes/StreamLine/";

    [Serializable]
    private struct SceneCharging
    {
        [FormerlySerializedAs("scene")]
        public string _scene;
        [FormerlySerializedAs("loadMode")]
        public LoadSceneMode _loadMode;

        public SceneCharging(string scene, LoadSceneMode loadSceneMode)
        {
            _scene = scene;
            _loadMode = loadSceneMode;
        }
    }

    [SerializeField]
    private List<SceneCharging> listScene = new List<SceneCharging>();

    protected SceneTransition() { } // guarantee this will be always a singleton only - can't use the constructor!


    private bool closing = false;

    [Button]
    public void PlayIndex(int index)
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(listScene[index]._scene, listScene[index]._loadMode);
        }            
        else
        {
#if UNITY_EDITOR
            EditorSceneManager.OpenScene(pathScene + listScene[index]._scene + ".unity", OpenSceneMode.Additive);//LoadScene(listScene[index].scene, listScene[index].loadMode);
#endif
        }

    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Called if we manualy close
    /// </summary>
    private void OnApplicationQuit()
    {
        if (closing)
            return;
        Quit();
    }

    /// <summary>
    /// Exit game (in play mode or in runtime)
    /// </summary>
    [ContextMenu("Quit")]
    private void Quit()
    {
        closing = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
