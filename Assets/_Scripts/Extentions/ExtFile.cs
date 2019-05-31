using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ExtFile
{
    /// <summary>
    /// get a screenshot name based on height and width, with the current date
    /// use: ExtRandom
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static string ScreenShotName(int width, int height)
    {
        return string.Format("screen_{0}x{1}_{2}.png",
                             width, height,
                             ExtDateTime.GetDateNow());
    }

    /// <summary>
    /// read every line in the file
    /// </summary>
    public static void ReadTextFile(string filePath)
    {
        StreamReader file = new StreamReader(filePath);

        while (!file.EndOfStream)
        {
            string line = file.ReadLine();
            Debug.Log(line);            
        }
        file.Close();
    }

    #region SaveTo

    /// <summary>
    /// Creates a directory at <paramref name="folder"/> if it doesn't exist
    /// </summary>
    /// <param name="folder"></param>
    public static void CreateDirectoryIfNotExists(this string folder)
    {
        if (string.IsNullOrEmpty(folder))
            return;

        string path = Path.GetDirectoryName(folder);
        if (string.IsNullOrEmpty(path))
            return;

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Saves the data to a file
    /// </summary>
    /// <param name="data"></param>
    /// <param name="path"></param>
    public static void SaveTo(this string data, string path, bool append)
    {
        // exit if no data or no filename
        if ((string.IsNullOrEmpty(data)) || (string.IsNullOrEmpty(path)))
            return;

        // create the folder if it doesn't exist
        path.CreateDirectoryIfNotExists();

        if (append)
        {
            File.AppendAllText(path, data);
            return;
        }

        // save the data
        File.WriteAllText(path, data);
    }

    // SaveTo
    #endregion

    #region SaveToPersistentDataPath

    /// <summary>
    /// Saves the data to the PersistentDataPath, which is a directory where your application can store user specific 
    /// data on the target computer. This is a recommended way to store files locally for a user like highscores or savegames. 
    /// C:\Users\Hed\AppData\LocalLow\DefaultCompany\EkkoUnity\
    /// </summary>
    /// <param name="data">data to save</param>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static void SaveToPersistentDataPath(this string data, string folderName, string filename, bool append = false)
    {
        // exit if no data or no filename
        if ((string.IsNullOrEmpty(data)) || (string.IsNullOrEmpty(filename)))
            return;

        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.persistentDataPath, filename) :
            Path.Combine(Path.Combine(Application.persistentDataPath, folderName), filename);

        // save the data
        SaveTo(data, path, append);
    }

    // SaveToPersistentDataPath
    #endregion

    #region SaveToDataPath

    /// <summary>
    /// Saves the data to the DataPath, which points to your asset/project directory. This directory is typically read-only after
    /// your game has been compiled. Use SaveToDataPath only from Editor scripts
    /// </summary>
    /// <param name="data">data to save</param>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static void SaveToDataPath(this string data, string folderName, string filename, bool append = false)
    {
        // exit if no data or no filename
        if ((string.IsNullOrEmpty(data)) || (string.IsNullOrEmpty(filename)))
            return;

        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.dataPath, filename) :
            Path.Combine(Path.Combine(Application.dataPath, folderName), filename);

        // save the data
        SaveTo(data, path, append);
    }

    // SaveToDataPath
    #endregion

    #region LoadFrom

    /// <summary>
    /// Loads the data as a string 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadFrom_AsString(this string path)
    {
        // exit if no path
        if (string.IsNullOrEmpty(path))
            return null;

        // exit if the file doesn't exist
        if (!File.Exists(path))
            return null;

        // read the file
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Loads the data as a byte array 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static byte[] LoadFrom_AsBytes(this string path)
    {
        // exit if no path
        if (string.IsNullOrEmpty(path))
            return null;

        // exit if the file doesn't exist
        if (!File.Exists(path))
            return null;

        // read the file
        return File.ReadAllBytes(path);
    }

    // LoadFrom
    #endregion

    #region LoadFromPeristantDataPath

    /// <summary>
    /// Loads the data from PersistantDataPath as a string 
    /// </summary>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static string LoadFromPeristantDataPath_AsString(this string filename, string folderName)
    {
        // exit if no path
        if (string.IsNullOrEmpty(filename))
            return null;

        // build the path
        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.persistentDataPath, filename) :
            Path.Combine(Path.Combine(Application.persistentDataPath, folderName), filename);

        // load the data
        return LoadFrom_AsString(path);
    }

    /// <summary>
    /// Loads the data from PersistantDataPath as a byte array 
    /// </summary>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static byte[] LoadFromPeristantDataPath_AsBytes(this string filename, string folderName)
    {
        // exit if no path
        if (string.IsNullOrEmpty(filename))
            return null;

        // build the path
        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.persistentDataPath, filename) :
            Path.Combine(Path.Combine(Application.persistentDataPath, folderName), filename);

        // load the data
        return LoadFrom_AsBytes(path);
    }

    // LoadFromPeristantDataPath
    #endregion

    #region LoadFromDataPath

    /// <summary>
    /// Loads the data from PersistantDataPath as a string 
    /// </summary>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static string LoadFromDataPath_AsString(this string filename, string folderName)
    {
        // exit if no path
        if (string.IsNullOrEmpty(filename))
            return null;

        // build the path
        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.dataPath, filename) :
            Path.Combine(Path.Combine(Application.dataPath, folderName), filename);

        // load the data
        return LoadFrom_AsString(path);
    }

    /// <summary>
    /// Loads the data from PersistantDataPath as a byte array 
    /// </summary>
    /// <param name="folderName">OPTIONAL - sub folder name (ex. DataFiles\SavedGames</param>
    /// <param name="filename">the filename (ex. SavedGameData.xml)</param>
    public static byte[] LoadFromDataPath_AsBytes(this string filename, string folderName)
    {
        // exit if no path
        if (string.IsNullOrEmpty(filename))
            return null;

        // build the path
        string path = string.IsNullOrEmpty(folderName) ?
            Path.Combine(Application.dataPath, filename) :
            Path.Combine(Path.Combine(Application.dataPath, folderName), filename);

        // load the data
        return LoadFrom_AsBytes(path);
    }

    // LoadFromDataPath
    #endregion

    #region directory

    /// <summary>
    /// Determine whether a given path is a directory.
    /// </summary>
    public static bool PathIsDirectory(string absolutePath)
    {
        FileAttributes attr = File.GetAttributes(absolutePath);
        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Given an absolute path, return a path rooted at the Assets folder.
    /// </summary>
    /// <remarks>
    /// Asset relative paths can only be used in the editor. They will break in builds.
    /// </remarks>
    /// <example>
    /// /Folder/UnityProject/Assets/resources/music returns Assets/resources/music
    /// </example>
    public static string AssetsRelativePath(string absolutePath)
    {
        if (absolutePath.StartsWith(Application.dataPath))
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }
        else
        {
            throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
        }
    }

    public static string[] GetResourcesDirectories()
    {
        List<string> result = new List<string>();
        Stack<string> stack = new Stack<string>();
        // Add the root directory to the stack
        stack.Push(Application.dataPath);
        // While we have directories to process...
        while (stack.Count > 0)
        {
            // Grab a directory off the stack
            string currentDir = stack.Pop();
            try
            {
                foreach (string dir in Directory.GetDirectories(currentDir))
                {
                    if (Path.GetFileName(dir).Equals("Resources"))
                    {
                        // If one of the found directories is a Resources dir, add it to the result
                        result.Add(dir);
                    }
                    // Add directories at the current level into the stack
                    stack.Push(dir);
                }
            }
            catch
            {
                Debug.LogError("Directory " + currentDir + " couldn't be read from.");
            }
        }
        return result.ToArray();
    }

    #endregion

}
