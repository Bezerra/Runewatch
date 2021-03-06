using System;
using UnityEngine;

/// <summary>
/// Class responsible for pausing and upausing the game.
/// </summary>
public class PauseSystem : MonoBehaviour, IFindInput, IFindPlayer
{
    [SerializeField] private GameObject[] canvasFalseToPauseTheGame;

    // It's gray but it's being used on editor
    [Header("Cheat Console")]
    [SerializeField] private GameObject cheatConsoleGO;
    [SerializeField] private CheatConsole cheatConsole;

    // Components
    private IInput input;

    public bool GameIsPaused { get; private set; }

    /// <summary>
    /// Set to true after scene ended loading.
    /// Only allows the pause the game after everything was loaded.
    /// </summary>
    public bool CanPauseTheGame { get; set; }

    private void Awake()
    {
        this.input = FindObjectOfType<PlayerInputCustom>();
    }

    private void Start()
    {
        GameIsPaused = false;
        CanPauseTheGame = false;

        if (Application.isEditor)
        {
            CanPauseTheGame = true;
        }
    }

    private void OnEnable() =>
        this.input.PauseGame += PauseGame;

    private void OnDisable() =>
        LostInput();

#if UNITY_EDITOR == false
    private void OnApplicationFocus(bool focus)
    {
        if (CanPauseTheGame &&
            IsAnyCanvasActive() == false)
        {
            if (cheatConsoleGO.activeSelf)
            {
                cheatConsole.DisableConsole();
            }

            if (focus == false) PauseGame();
        }
    }
#endif

    /// <summary>
    /// Checks if any other canvas different than pause is active
    /// </summary>
    /// <returns>Returns true if any canvas is active</returns>
    public bool IsAnyCanvasActive()
    {
        for (int i = 0; i < canvasFalseToPauseTheGame.Length; i++)
        {
            if (canvasFalseToPauseTheGame[i].activeSelf) return true;
        }
        return false;
    }

    /// <summary>
    /// Pauses/Unpauses game and switches action map.
    /// </summary>
    public void PauseGame()
    {
        if (CanPauseTheGame)
        {
            if (GameIsPaused == false)
            {
                GameIsPaused = true;
                Time.timeScale = 0;
                this.input.SwitchActionMapToUI();
                OnGamePausedEvent();
            }
        }
    }

    /// <summary>
    /// Resumes game.
    /// </summary>
    public void ResumeGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        this.input.SwitchActionMapToGameplay();
    }

    public void FindInput(PlayerInputCustom input = null)
    {
        if (this.input != null)
        {
            this.input.PauseGame -= PauseGame;
        }

        if (input != null)
        {
            this.input = input;
        }
        else
        {
            this.input = FindObjectOfType<PlayerInputCustom>();
        }

        this.input.PauseGame += PauseGame;
    }

    public void LostInput(PlayerInputCustom input = null)
    {
        if (this.input != null)
            this.input.PauseGame -= PauseGame;
    }

    public void FindPlayer(Player player)
    {
        // Left blank on purpose
    }

    public void PlayerLost(Player player)
    {
        if (this.input != null)
            this.input.PauseGame -= PauseGame;
    }

    protected virtual void OnGamePausedEvent() => GamePausedEvent?.Invoke();
    public event Action GamePausedEvent;
}
