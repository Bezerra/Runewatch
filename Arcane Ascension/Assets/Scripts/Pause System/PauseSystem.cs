using UnityEngine;

/// <summary>
/// Class responsible for pausing and upausing the game.
/// </summary>
public class PauseSystem : MonoBehaviour
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

    private void OnEnable()
    {
        playerInputCustom.PauseGame += PauseGame;
        Debug.LogError(playerInputCustom);
    }

    private void OnDisable()
    {
        playerInputCustom.PauseGame -= PauseGame;
    }

    /// <summary>
    /// Pauses/Unpauses game and switches action map.
    /// </summary>
    public void PauseGame()
    {
        Debug.LogError("AH");
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
}
