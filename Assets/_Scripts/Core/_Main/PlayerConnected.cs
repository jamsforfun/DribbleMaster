using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;


/// <summary>
/// Do a vibration
/// </summary>
[Serializable]
public struct Vibration
{
    [Tooltip("vibre le rotor droit"), SerializeField]
    public bool vibrateLeft;
    [EnableIf("vibrateLeft"), Range(0, 1), Tooltip("force du rotor"), SerializeField]
    public float strenthLeft;
    [EnableIf("vibrateLeft"), Range(0, 10), Tooltip("temps de vibration"), SerializeField]
    public float durationLeft;

    [Tooltip("cooldown du jump"), SerializeField]
    public bool vibrateRight;
    [EnableIf("vibrateRight"), Range(0, 1), Tooltip("cooldown du jump"), SerializeField]
    public float strenthRight;
    [EnableIf("vibrateRight"), Range(0, 10), Tooltip("cooldown du jump"), SerializeField]
    public float durationRight;
}

/// <summary>
/// Manage plug/unplug gamepads
/// <summary>
[TypeInfoBox("Manage global input gamePad/keyboard and switch")]
public class PlayerConnected : SingletonMono<PlayerConnected>
{
    protected PlayerConnected() { } // guarantee this will be always a singleton only - can't use the constructor!

    #region variable
    [FoldoutGroup("Debug"), Tooltip("Active les vibrations"), ReadOnly]
    public bool enabledVibration = true;
    [FoldoutGroup("Debug"), Tooltip("show gamePad active"), ReadOnly]
    public bool[] playerArrayConnected;                      //tableau d'état des controller connecté
    [FoldoutGroup("Debug"), Tooltip("Active les vibrations")]
    public bool simulatePlayerOneifNoGamePad = false;   //Si aucune manette n'est connecté, active le player 1 avec le clavier !
    

    private short playerNumber = 4;                     //size fixe de joueurs (0 = clavier, 1-4 = manette)
    private Player[] playersRewired;                 //tableau des class player (rewired)
    private float timeToGo;
    private FrequencyCoolDown desactiveVibrationAtStart = new FrequencyCoolDown();
    private float timeDesactiveAtStart = 2f;

    #endregion

    #region  initialisation
    private void OnEnable()
    {
        EventManager.StartListening(GameData.Event.SceneLoaded, Init);
    }

    /// <summary>
    /// Initialisation
    /// </summary>
    private void Awake()                                                    //initialisation referencce
    {
        playerArrayConnected = new bool[playerNumber];                           //initialise 
        playersRewired = new Player[playerNumber];
        InitPlayerRewired();                                                //initialise les event rewired
        InitController();                                                   //initialise les controllers rewired   
    }

    /// <summary>
    /// Initialisation à l'activation
    /// </summary>
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        enabledVibration = GameManager.Instance.EnableVibration;
        desactiveVibrationAtStart.StartCoolDown(timeDesactiveAtStart);
    }

    /// <summary>
    /// initialise les players
    /// </summary>
    private void InitPlayerRewired()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;

        //parcourt les X players...
        for (int i = 0; i < playerNumber; i++)
        {
            playersRewired[i] = ReInput.players.GetPlayer(i);       //get la class rewired du player X
            playerArrayConnected[i] = false;                             //set son état à false par défault
        }

        SetKeyboardForPlayerOne();
    }

    /// <summary>
    /// défini le keyboard pour le joueur 1 SI il n'y a pas de manette;
    /// </summary>
    private void SetKeyboardForPlayerOne()
    {
        if (simulatePlayerOneifNoGamePad && NoPlayer())
            playerArrayConnected[0] = true;
    }

    /// <summary>
    /// initialise les players (manettes)
    /// </summary>
    private void InitController()
    {
        foreach (Player player in ReInput.players.GetPlayers(true))
        {
            foreach (Joystick j in player.controllers.Joysticks)
            {
                SetPlayerController(player.id, true);
                break;
            }
        }
    }
    #endregion

    #region core script

    /// <summary>
    /// actualise le player ID si il est connecté ou déconnecté
    /// </summary>
    /// <param name="id">id du joueur</param>
    /// <param name="isConnected">statue de connection du joystick</param>
    private void SetPlayerController(int id, bool isConnected)
    {
        playerArrayConnected[id] = isConnected;
    }

    private void UpdatePlayerController(int id, bool isConnected)
    {
        playerArrayConnected[id] = isConnected;
    }

    /// <summary>
    /// renvoi s'il n'y a aucun player de connecté
    /// </summary>
    /// <returns></returns>
    public bool NoPlayer()
    {
        for (int i = 0; i < playerArrayConnected.Length; i++)
        {
            if (playerArrayConnected[i])
                return (false);
        }
        return (true);
    }
    public int GetNbPlayer()
    {
        int nb = 0;
        for (int i = 0; i < playerArrayConnected.Length; i++)
        {
            if (playerArrayConnected[i])
                nb++;
        }
        return (nb);
    }

    /// <summary>
    /// get id of player
    /// </summary>
    /// <param name="id"></param>
    public Player GetPlayer(int id)
    {
        if (id == -1)
            return (ReInput.players.GetSystemPlayer());
        else if (id >= 0 && id < playerNumber)
            return (playersRewired[id]);
        Debug.LogError("problème d'id");
        return (null);
    }
    /// <summary>
    /// renvoi vrai si n'importe quel gamePad/joueur active
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool GetButtonDownFromAnyGamePad(string action)
    {
        for (int i = 0; i < playersRewired.Length; i++)
        {
            if (playersRewired[i].GetButtonDown(action))
                return (true);
        }
        return (false);
    }
    public bool GetButtonUpFromAnyGamePad(string action)
    {
        for (int i = 0; i < playersRewired.Length; i++)
        {
            if (playersRewired[i].GetButtonUp(action))
                return (true);
        }
        return (false);
    }

    /// <summary>
    /// set les vibrations du gamepad
    /// </summary>
    /// <param name="id">l'id du joueur</param>
    public void SetVibrationPlayer(int id, int motorIndex = 0, float motorLevel = 1.0f, float duration = 1.0f)
    {
        if (!enabledVibration)
            return;

        //if we are at start of the game, don't vibrate
        if (!desactiveVibrationAtStart.IsReady())
            return;

        GetPlayer(id).SetVibration(motorIndex, motorLevel, duration);
    }

    /// <summary>
    /// set les vibrations du gamepad
    /// </summary>
    /// <param name="id">l'id du joueur</param>
    public void SetVibrationPlayer(int id, Vibration vibration)
    {
        if (!enabledVibration)
            return;

        //if we are at start of the game, don't vibrate
        if (!desactiveVibrationAtStart.IsReady())
            return;

        if (vibration.vibrateLeft)
            GetPlayer(id).SetVibration(0, vibration.strenthLeft, vibration.durationLeft);
        if (vibration.vibrateRight)
            GetPlayer(id).SetVibration(1, vibration.strenthRight, vibration.durationRight);
    }

    #endregion

    #region unity fonction and ending

    /// <summary>
    /// a controller is connected
    /// </summary>
    /// <param name="args"></param>
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        UpdatePlayerController(args.controllerId, true);

        EventManager.TriggerEvent(GameData.Event.GamePadConnectionChange, true, args.controllerId);
    }

    /// <summary>
    /// a controller is disconnected
    /// </summary>
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        UpdatePlayerController(args.controllerId, false);
        SetKeyboardForPlayerOne();

        EventManager.TriggerEvent(GameData.Event.GamePadConnectionChange, false, args.controllerId);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameData.Event.SceneLoaded, Init);
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
    }
    #endregion
}
