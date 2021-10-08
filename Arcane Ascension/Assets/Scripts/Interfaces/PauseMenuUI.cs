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
}
