using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEditor.IMGUI.Controls;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEditorInternal;

/// <summary>
/// useful reflexion methods
/// </summary>
public class ExtReflexion
{
    public static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

    /// <summary>
    /// set an animation curbe to a linear curve, or other type of tangentMode
    /// use: ExtReflexion.SetAnimationCurveTangentMode(_animationCurve, AnimationUtility.TangentMode.Linear);
    /// </summary>
    /// <param name="curve"></param>
    /// <param name=""></param>
    public static void SetAnimationCurveTangentMode(AnimationCurve curve, AnimationUtility.TangentMode tangentMode)
    {
        for (int i = 0; i < curve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, tangentMode);
            AnimationUtility.SetKeyRightTangentMode(curve, i, tangentMode);
        }
    }

    /// <summary>
    /// get an linear AnimationCurve
    /// </summary>
    /// <returns></returns>
    public static AnimationCurve GetLinearDefaultCurve()
    {
        AnimationCurve newCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        SetAnimationCurveTangentMode(newCurve, AnimationUtility.TangentMode.Linear);
        return (newCurve);
    }

    /// <summary>
    /// play button on animator
    /// </summary>
    public static void SetPlayButton()
    {
        //GetAllEditorWindowTypes(true);

        //open animator
        System.Type animatorWindowType = null;
        EditorWindow animatorWindow = ExtReflexion.ShowAndReturnEditorWindow(ExtReflexion.AllNameAssemblyKnown.AnimatorControllerTool, ref animatorWindowType);

        //open animation
        System.Type animationWindowType = null;
        EditorWindow animationWindowEditor = ExtReflexion.ShowAndReturnEditorWindow(ExtReflexion.AllNameAssemblyKnown.AnimationWindow, ref animationWindowType);

        //Get animationWindow Type
        //animationWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.AnimationWindow");

        //Get field m_AnimEditor
        FieldInfo animEditorFI = animationWindowType.GetField("m_AnimEditor", ExtReflexion.GetFullBinding());

        /*
        object animEditorObject = animEditorFI.GetValue(animationWindowEditor);
        MethodInfo playMI = animEditorFI.FieldType.GetMethod("TogglePlayAnimation", ExtReflexion.GetFullBinding());
        Debug.Log(playMI.Name);


        Type[] types = new Type[1];
        object paramFunction = playMI.GetType().GetConstructor(GetFullBinding(), null, new type)
        ConstructorInfo constructorInfoObj = playMI.GetType().GetConstructor(GetFullBinding(), null,
                CallingConventions.HasThis, types, null);

        playMI.Invoke(animEditorObject, new object[0]);
        */
        //PlayButtonOnGUI



        //Get the propertue of animEditorFI
        PropertyInfo controlInterfacePI = animEditorFI.FieldType.GetProperty("controlInterface", ExtReflexion.GetFullBinding());

        //Get property i splaying or not
        PropertyInfo isPlaying = controlInterfacePI.PropertyType.GetProperty("playing", ExtReflexion.GetFullBinding());

        //get object controlInterface
        object controlInterface = controlInterfacePI.GetValue(animEditorFI.GetValue(animationWindowEditor));
        bool playing = (bool)isPlaying.GetValue(controlInterface);

        if (!playing)
        {
            MethodInfo playMI = controlInterfacePI.PropertyType.GetMethod("StartPlayback", ExtReflexion.GetFullBinding());
            playMI.Invoke(controlInterface, new object[0]);
        }
        else
        {
            MethodInfo playMI = controlInterfacePI.PropertyType.GetMethod("StopPlayback", ExtReflexion.GetFullBinding());
            playMI.Invoke(controlInterface, new object[0]);
        }

    }

    /// <summary>
    /// search for an object in all editro search bar
    /// </summary>
    /// <param name="search"></param>
    public static void SetSearch(string search)
    {
        System.Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        //MethodInfo[] allMethods = GetAllMethodeOfType(type, GetFullBinding(), true);
        MethodInfo methodInfo = type.GetMethod("SetSearchFilter", GetFullBinding());

        EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        if (methodInfo == null)
        {
            Debug.Log("null");
            return;
        }

        var window = EditorWindow.focusedWindow;
        methodInfo.Invoke(window, new object[] { search, SearchableEditorWindow.SearchMode.All, true, false });
    }

    /// <summary>
    /// collapse an object
    /// </summary>
    public static void Collapse(GameObject go, bool collapse)
    {
        // bail out immediately if the go doesn't have children
        if (go.transform.childCount == 0)
            return;

        if (collapse)
        {
            EditorGUIUtility.PingObject(go.transform.GetChild(0).gameObject);
            Selection.activeObject = go;
        }
        else
        {
            SetExpandedRecursive(go, false);
        }
    }

    /// <summary>
    /// expand recursivly a hierarchy foldout
    /// </summary>
    /// <param name="go"></param>
    /// <param name="expand"></param>
    public static void SetExpandedRecursive(GameObject go, bool expand)
    {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var methodInfo = type.GetMethod("SetExpandedRecursive");

        EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        var window = EditorWindow.focusedWindow;

        methodInfo.Invoke(window, new object[] { go.GetInstanceID(), expand });
    }

    [MenuItem("CONTEXT/Component/Collapse All")]
    private static void CollapseAll(MenuCommand command)
    {
        SetAllInspectorsExpanded((command.context as Component).gameObject, false);
    }

    [MenuItem("CONTEXT/Component/Expand All")]
    private static void ExpandAll(MenuCommand command)
    {
        SetAllInspectorsExpanded((command.context as Component).gameObject, true);
    }

    public static void SetAllInspectorsExpanded(GameObject go, bool expanded)
    {
        Component[] components = go.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Renderer)
            {
                var mats = ((Renderer)component).sharedMaterials;
                for (int i = 0; i < mats.Length; ++i)
                {
                    InternalEditorUtility.SetIsInspectorExpanded(mats[i], expanded);
                }
            }
            InternalEditorUtility.SetIsInspectorExpanded(component, expanded);
        }
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    /// <summary>
    /// repaint an Editor
    /// use: RepaintInspector(typeof(SomeTypeInspector));
    /// </summary>
    /// <param name="t"></param>
    public static void RepaintInspector(System.Type t)
    {
        Editor[] ed = (Editor[])Resources.FindObjectsOfTypeAll<Editor>();
        for (int i = 0; i < ed.Length; i++)
        {
            if (ed[i].GetType() == t)
            {
                ed[i].Repaint();
                return;
            }
        }
    }


    /////////////////////////utility reflexion

    /// <summary>
    /// for adding, do a GetAllEditorWindowTypes(true);
    /// </summary>
    public enum AllNameAssemblyKnown
    {
        AnimatorControllerTool,
        AnimationWindow,
        SearchWindow,
        SceneHierarchySortingWindow,
        SceneHierarchyWindow,
        AssetStoreWindow,
        GameView,
        InspectorWindow,
        SearchableEditorWindow,
        SceneView,
    }

    public enum AllNameEditorWindowKnown
    {
        Lighting,
        Game,
        Scene,
        Hierarchy,
        Project,
        Inspector
    }

    /// <summary>
    /// Get all editor window type.
    /// If we want just the one open, we can do just:
    /// EditorWindow[] allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
    /// System.Type[] allUnityWindow = UtilityEditor.GetAllEditorWindowTypes(true);
    /// </summary>
    /// <returns></returns>
    private static System.Type[] GetAllEditorWindowTypes(bool showInConsol = false)
    {
        var result = new System.Collections.Generic.List<System.Type>();
        System.Reflection.Assembly[] AS = System.AppDomain.CurrentDomain.GetAssemblies();
        System.Type editorWindow = typeof(EditorWindow);
        foreach (var A in AS)
        {
            System.Type[] types = A.GetTypes();
            foreach (var T in types)
            {
                if (T.IsSubclassOf(editorWindow))
                {
                    result.Add(T);
                    if (showInConsol)
                    {
                        //Debug.Log(T.FullName);
                        Debug.Log(T.Name);
                    }
                }
                    
            }
        }
        return result.ToArray();
    }

    private static System.Type GetEditorWindowTypeByName(string editorToFind)
    {
        var result = new System.Collections.Generic.List<System.Type>();
        System.Reflection.Assembly[] AS = System.AppDomain.CurrentDomain.GetAssemblies();
        System.Type editorWindow = typeof(EditorWindow);
        foreach (var A in AS)
        {
            System.Type[] types = A.GetTypes();
            foreach (var T in types)
            {
                if (T.IsSubclassOf(editorWindow))
                {
                    if (T.Name.Equals(editorToFind))
                    {
                        return (T);
                    }
                }

            }
        }
        return (null);
    }


    /// <summary>
    /// System.Type animationWindowType = ExtReflexion.GetTypeFromAssembly("AnimationWindow", editorAssembly);
    /// </summary>
    private static MethodInfo[] GetAllMethodeOfType(System.Type type, System.Reflection.BindingFlags bindings, bool showInConsol = false)
    {
        MethodInfo[] allMathod = type.GetMethods(bindings);
        if (showInConsol)
        {
            for (int i = 0; i < allMathod.Length; i++)
            {
                Debug.Log(allMathod[i].Name);
            }
        }
        return (allMathod);
    }

    private static FieldInfo[] GetAllFieldOfType(System.Type type, System.Reflection.BindingFlags bindings, bool showInConsol = false)
    {
        FieldInfo[] allField = type.GetFields(bindings);
        if (showInConsol)
        {
            for (int i = 0; i < allField.Length; i++)
            {
                Debug.Log(allField[i].Name);
            }
        }
        return (allField);
    }

    private static PropertyInfo[] GetAllpropertiesOfType(System.Type type, System.Reflection.BindingFlags bindings, bool showInConsol = false)
    {
        PropertyInfo[] allProperties = type.GetProperties(bindings);
        if (showInConsol)
        {
            for (int i = 0; i < allProperties.Length; i++)
            {
                Debug.Log(allProperties[i].Name);
            }
        }
        return (allProperties);
    }

    /// <summary>
    /// show all opened window
    /// </summary>
    private static EditorWindow[] GetAllOpennedWindow(bool showInConsol = false)
    {
        EditorWindow[] allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();

        if (showInConsol)
        {
            for (int i = 0; i < allWindows.Length; i++)
            {
                Debug.Log(allWindows[i].titleContent.text);
            }
        }
        return (allWindows);
    }

    private static EditorWindow GetOpennedWindowByName(string editorToFind)
    {
        EditorWindow[] allWIndow = GetAllOpennedWindow();
        for (int i = 0; i < allWIndow.Length; i++)
        {
            if (allWIndow[i].titleContent.text.Equals(editorToFind))
            {
                return (allWIndow[i]);
            }
        }
        return (null);
    }

    private static System.Reflection.BindingFlags GetFullBinding()
    {
        return (BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static);
    }

    /// <summary>
    /// System.Reflection.Assembly editorAssembly = System.Reflection.Assembly.GetAssembly(typeof(EditorWindow));
    /// GetTypeFromAssembly("AnimationWindow", editorAssembly);
    /// </summary>
    /// <returns></returns>
    private static System.Type GetTypeFromAssembly(string typeName, System.Reflection.Assembly assembly, System.StringComparison ignoreCase = StringComparison.CurrentCultureIgnoreCase, bool showNames = false)
    {
        if (assembly == null)
            return (null);

        System.Type[] types = assembly.GetTypes();
        foreach (System.Type type in types)
        {
            if (showNames)
            {
                Debug.Log(type.Name);
            }
            if (type.Name.Equals(typeName, ignoreCase) || type.Name.Contains('+' + typeName))
                return (type);
        }
        return (null);
    }

    /*
    /// <summary>
    /// from a given name, return and open/show the editorWindow
    /// usage:
    /// System.Type animationWindowType = null;
    /// EditorWindow animationWindowEditor = ExtReflexion.ShowAndReturnEditorWindow(ExtReflexion.AllNameAssemblyKnown.AnimationWindow, ref animationWindowType);
    /// </summary>
    public static EditorWindow ShowAndReturnEditorWindow(AllNameAssemblyKnown editorWindow, ref System.Type animationWindowType)
    {
        System.Reflection.Assembly editorAssembly = System.Reflection.Assembly.GetAssembly(typeof(EditorWindow));
        animationWindowType = ExtReflexion.GetTypeFromAssembly(editorWindow.ToString(), editorAssembly);
        EditorWindow animationWindowEditor = EditorWindow.GetWindow(animationWindowType);

        return (animationWindowEditor);
    }
    */

    /*
meth_PickObjectMeth = type_HandleUtility.GetMethod("myFunction",
                                             BindingFlags.Static | BindingFlags.Public, //if static AND public
                                             null,
                                             new [] {typeof(Vector2), typeof(bool)},//specify arguments to tell reflection which variant to look for
                                             null)
*/

    private static EditorWindow ShowAndReturnEditorWindow(AllNameAssemblyKnown editorWindow, ref System.Type animationWindowType)
    {
        animationWindowType = GetEditorWindowTypeByName(editorWindow.ToString());
        EditorWindow animatorWindow = EditorWindow.GetWindow(animationWindowType);
        return (animatorWindow);
    }
}