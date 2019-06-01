using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// InputPlayer Description
/// </summary>
[TypeInfoBox("Player input")]
public class PlayerInput : MonoBehaviour
{
    [FoldoutGroup("Debug"), Tooltip("input for moving Camera horizontally"), ReadOnly]
    public Vector2 Move;

    [FoldoutGroup("Debug"), Tooltip("input for moving Camera horizontally"), ReadOnly]
    public bool Shoot;

    [FoldoutGroup("Object"), Tooltip("id unique du joueur correspondant à sa manette"), SerializeField]
    protected PlayerController playerController;
    public PlayerController PlayerController { get { return (playerController); } }
    
    private void OnEnable()
    {
        EventManager.StartListening(GameData.Event.GameOver, GameOver);
    }

    /// <summary>
    /// tout les input du jeu, à chaque update
    /// </summary>
    private void GetInput()
    {
        //all button
        Shoot = PlayerConnected.Instance.GetPlayer(playerController.PlayerSettings.Id).GetButton("Shoot");

        Move = new Vector2(
            PlayerConnected.Instance.GetPlayer(playerController.PlayerSettings.Id).GetAxis("Move Horizontal"),
            PlayerConnected.Instance.GetPlayer(playerController.PlayerSettings.Id).GetAxis("Move Vertical"));
    }

    public bool IsMoving()
    {
        return (Move != Vector2.zero);
    }

    private void Update()
    {
        GetInput();
    }

    private void GameOver()
    {

    }

    private void OnDisable()
    {
        EventManager.StartListening(GameData.Event.GameOver, GameOver);
    }
}
