using UnityEngine;

/// <summary>
/// Script with pause menu button methods.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    private PauseSystem pauseSystem;
    private PauseMenuReactionUI pauseMenuReaction;

    private void Awake()
    {
        pauseSystem = FindObjectOfType<PauseSystem>();
        pauseMenuReaction = GetComponentInParent<PauseMenuReactionUI>();
    }

    public void ResumeGame()
    {
        pauseMenuReaction.EnableDisableInterface();
        pauseSystem.PauseGame();
    }

    public void MainMenu()
    {
        FindObjectOfType<SceneControl>().LoadScene(SceneEnum.MainMenu);
    }
}
