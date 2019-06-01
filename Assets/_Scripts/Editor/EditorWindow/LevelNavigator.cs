using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// search for every scene in the project, and then just swap with buttons
/// </summary>
public class LevelNavigator : EditorWindow
{
    [MenuItem("PERSO/Level Navigator")]
    public static void ShowLevelNavigator()
    {
        EditorWindow window = EditorWindow.GetWindow<LevelNavigator>("Level Navigator");
        Vector2 minSize = window.minSize;
        minSize.y = 26;
        window.minSize = minSize;
    }

    private string pathWeAreInterested = "RACE";
    private List<string> allSceneFound = new List<string>();
    private string currentScene = string.Empty;
    private int currentIndex = 0;

    private string[] allAssetPath;

    private void OnEnable()
    {
        allAssetPath = AssetDatabase.GetAllAssetPaths();
        SetupListAssets();
    }

    private void SetupListAssets()
    {
        allSceneFound = new List<string>();
        for (int i = 0; i < allAssetPath.Length; i++)
        {
            if (allAssetPath[i].EndsWith(".unity") && allAssetPath[i].Contains(pathWeAreInterested))
            {
                allSceneFound.Add(allAssetPath[i]);
            }
        }
        GrabCurrentScene();
        GrabCurrentIndexScene();
    }

    private void GrabCurrentScene()
    {
        currentScene = EditorSceneManager.GetActiveScene().name;
    }

    private void GrabCurrentIndexScene()
    {
        for (int i = 0; i < allSceneFound.Count; i++)
        {
            string shortName = Path.GetFileNameWithoutExtension(allSceneFound[i]);
            if (allSceneFound[i].Equals(shortName))
            {
                currentIndex = i;
                return;
            }
        }
    }

    private void OnDisable()
    {
        allSceneFound.Clear();
        allSceneFound = null;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(currentScene);

        string old = pathWeAreInterested;
        pathWeAreInterested = GUILayout.TextField(pathWeAreInterested);

        if (old != pathWeAreInterested)
        {
            SetupListAssets();
        }

        if (GUILayout.Button("<<"))
        {
            ChangeScene(-1);
            EditorGUIUtility.ExitGUI();
        }
        if (GUILayout.Button(">>"))
        {
            ChangeScene(1);
            EditorGUIUtility.ExitGUI();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        for (int i = 0; i < allSceneFound.Count; i++)
        {
            string text = allSceneFound[i];
            if (i == currentIndex)
            {
                text = "(*) " + text;
                GUILayout.Label(text);
            }
            else if (GUILayout.Button(text))
            {
                OpenScene(i);
            }                
        }
        GUILayout.EndVertical();
    }

    private void OpenScene(int index)
    {
        currentIndex = index;
        currentScene = allSceneFound[index];
        EditorSceneManager.OpenScene(allSceneFound[index]);
    }

    private void ChangeScene(int direction)
    {
        if (currentScene == string.Empty || allSceneFound.Count == 0)
        {
            return;
        }
        currentIndex += direction;
        if (currentIndex < 0)
        {
            currentIndex = allSceneFound.Count - 1;
        }
        else if (currentIndex == allSceneFound.Count)
        {
            currentIndex = 0;
        }
        OpenScene(currentIndex);
        GrabCurrentScene();
        GrabCurrentIndexScene();
    }
}
