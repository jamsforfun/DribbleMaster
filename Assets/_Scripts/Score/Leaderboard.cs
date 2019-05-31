using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

/// <summary>
/// Leaderboard Description
/// Pour gerer la BDD:
/// saved url: http://dreamlo.com/lb/Zuy6exDtzECGsloaUcXDAQvaxU7J93vES2Aq8I-c2QPQ
/// 
/// Ajouter un score comme ça n'import ou dans le jeu:
/// Leaderboard.GetSingleton.AddNewHighscore("Noob", 7);
/// </summary>
[ShowOdinSerializedPropertiesInInspector]
public class Leaderboard : SingletonMono<Leaderboard>
{
    protected Leaderboard() { } // guarantee this will be always a singleton only - can't use the constructor!

    #region Attributes
    const string privateCode = "Zuy6exDtzECGsloaUcXDAQvaxU7J93vES2Aq8I-c2QPQ";
    const string publicCode = "5a5b77e939992b09e430621e";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    public bool uploadedScore = false;

    #endregion

    #region Initialization

    private void Awake()
    {
        DownloadHighscores();
    }
    
    #endregion

    #region Core
    [Button]
    public void addScore()
    {
        AddNewHighscore("Toto", 90600);
    }

    /// <summary>
    /// cette fonction est static pour être appelé de n'importe quel script
    /// </summary>
    public void AddNewHighscore(string username, int score)
    {
        Instance.StartCoroutine(Instance.UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Upload Successful");
            DownloadHighscores();
        }
        else
        {
            Debug.Log("Error uploading: " + www.error);
        }
    }

    public void DownloadHighscores()
    {
        uploadedScore = false;
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        //WWW www = new WWW(webURL + publicCode + "/pipe/");
        UnityWebRequest www = new UnityWebRequest(webURL + publicCode + "/pipe/0/12");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //result as test, sent to formatter
            FormatHighscores(www.downloadHandler.text);

            uploadedScore = true;
        }
        else
        {
            Debug.Log("Error Downloading: " + www.error);
        }
    }

    /// <summary>
    /// formate le score du site dans une structure compréhensible par nous
    /// </summary>
    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
            //highscoresList[i].Print();
            //Debug.Log(highscoresList[i].username + ": " + highscoresList[i].score);
        }
    }

    #endregion

    #region Unity ending functions

    #endregion
}

/// <summary>
/// structure du HighScore pour une personne
/// </summary>
public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
    public void Print()
    {
        Debug.Log(username + ": " + score);
    }

}