using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Save data to binary format
/// </summary>
public class DataSaver
{
    #region Core
    /// <summary>
    /// Save data from path
    /// </summary>
	public static void Save(PersistantData data)
    {
		if (!data.GetType().IsSerializable)
		{
            Debug.LogWarning("NOT SERIALIZABLE");
			return;
		}

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.streamingAssetsPath + "/" + data.GetFilePath());

		bf.Serialize(file, data);
        file.Close();
        Debug.Log("<color=red>" + data.GetFilePath() + " saved!</color>");
    }

    /// <summary>
    /// Load data from path
    /// </summary>
	public static T Load<T>(string path)
    {
		if (typeof(T).IsSerializable && File.Exists (Application.streamingAssetsPath + "/" + path))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.streamingAssetsPath + "/" + path, FileMode.Open);
			T currentData = (T)bf.Deserialize (file);
			file.Close ();

			//Debug.Log (path + " loaded!");
			return currentData;
		}

		return default(T);
    }

    /// <summary>
    /// Delete save file
    /// </summary>
	public static void DeleteSave(string name)
    {
		if (File.Exists(Application.streamingAssetsPath + "/" + name))
        {
            Debug.Log("delete");
            File.Delete(Application.streamingAssetsPath + "/" + name);
        }
    }
		
    #endregion
}

[Serializable]
public abstract class PersistantData
{
	public abstract string GetFilePath ();
}