using System;
using UnityEngine;

/// <summary>
/// Class responsible for pausing and upausing the game.
/// </summary>
public class PauseSystem : MonoBehaviour, IFindInput, IFindPlayer
{
    // Components
    private IInput playerInputCustom;

    public bool GameIsPaused { get; private set; }

    private void Awake()
    {
        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
    }

    private void Start()
    {
        GameIsPaused = false;
    }

    private void OnEnable() =>
        playerInputCustom.PauseGame += PauseGame;

    private void OnDisable() =>
        LostInput();

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false) PauseGame();
    }

    /// <summary>
    /// Pauses/Unpauses game and switches action map.
    /// </summary>
    public void PauseGame()
    {
        if (GameIsPaused == false)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            playerInputCustom.SwitchActionMapToUI();
            OnGamePausedEvent();
        }
    }

    /// <summary>
    /// Resumes game.
    /// </summary>
    public void ResumeGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        playerInputCustom.SwitchActionMapToGameplay();
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
            playerInputCustom.PauseGame -= PauseGame;
    }

    public void FindPlayer()
    {
        // Left blank on purpose
    }

    public void PlayerLost()
    {
        if (playerInputCustom != null)
            playerInputCustom.PauseGame -= PauseGame;
    }

    protected virtual void OnGamePausedEvent() => GamePausedEvent?.Invoke();
    public event Action GamePausedEvent;
}
