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
    }

    private void OnDisable()
    {
        playerInputCustom.PauseGame -= PauseGame;
    }

    private void PauseGame()
    {
        if (gameIsPaused == false)
        {
            gameIsPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            gameIsPaused = false;
            Time.timeScale = 1;
        }
    }
}
