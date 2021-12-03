using UnityEngine;

/// <summary>
/// Class responsible for pausing and upausing the game.
/// </summary>
public class PauseSystem : MonoBehaviour, IFindInput
{
    // Components
    private PlayerInputCustom playerInputCustom;

    private bool gameIsPaused;

    private void Awake()
    {
        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
    }

    private void Start()
    {
        gameIsPaused = false;
    }

    private void OnEnable() =>
        playerInputCustom.PauseGame += PauseGame;

    private void OnDisable() =>
        LostInput();

    /// <summary>
    /// Pauses/Unpauses game and switches action map.
    /// </summary>
    public void PauseGame()
    {
        if (gameIsPaused == false)
        {
            gameIsPaused = true;
            Time.timeScale = 0;
            playerInputCustom.SwitchActionMapToUI();
        }
        else
        {
            gameIsPaused = false;
            Time.timeScale = 1;
            playerInputCustom.SwitchActionMapToGameplay();
        }
    }

    public void FindInput()
    {
        if (playerInputCustom != null)
        {
            playerInputCustom.PauseGame -= PauseGame;
        }

        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
        playerInputCustom.PauseGame += PauseGame;
    }

    public void LostInput()
    {
        if (playerInputCustom != null)
        {
            playerInputCustom.PauseGame -= PauseGame;
        }
    }
}
