using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens and closes pause interface when the game is paused.
/// </summary>
public class PauseMenuBaseCanvasUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseBaseBackground;
    [SerializeField] private GameObject introMenu;
    [SerializeField] private GameObject optionsMenu;

    // Components
    private PlayerInputCustom playerInputCustom;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private PauseSystem pauseSystem;

    private void Awake()
    {
        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
        raycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();
        pauseSystem = FindObjectOfType<PauseSystem>();
    }

    private void OnEnable()
    {
        playerInputCustom.PauseGame += EnableDisableInterface;
    }

    private void OnDisable()
    {
        playerInputCustom.PauseGame -= EnableDisableInterface;
    }

    /// <summary>
    /// Enables or disables pause interface.
    /// </summary>
    public void EnableDisableInterface()
    {
        if (pauseBaseBackground.activeSelf)
        {
            raycaster.enabled = false;
            canvas.enabled = false;
            pauseBaseBackground.SetActive(false);
        }
        else
        {
            raycaster.enabled = true;
            canvas.enabled = true;
            pauseBaseBackground.SetActive(true);
        }
    }

    public void MenuControlIntro(bool condition) =>
        introMenu.SetActive(condition);

    public void MenuControlOptions(bool condition) =>
        optionsMenu.SetActive(condition);

    public void ButtonIntroResumeGame()
    {
        EnableDisableInterface();
        pauseSystem.PauseGame();
    }

    public void ButtonIntroMainMenu()
    {
        FindObjectOfType<SceneControl>().LoadScene(SceneEnum.MainMenu);
    }
}
