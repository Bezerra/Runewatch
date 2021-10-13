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
        pauseSystem.PauseGame();
        pauseMenuReaction.EnableDisableInterface();
    }

    public void MainMenu()
    {
        Debug.Log("loads main menu");

        //FindObjectOfType<SceneControl>().LoadScene(SceneEnum.MainMenu);
    }
}
