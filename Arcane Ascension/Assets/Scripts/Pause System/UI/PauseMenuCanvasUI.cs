using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains methods for pause menu.
/// </summary>
public class PauseMenuCanvasUI : MonoBehaviour, IFindInput, IFindPlayer
{
    [SerializeField] private GameObject backgroundCanvas;
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuSettings;
    [SerializeField] private GameObject menuControls;
    [SerializeField] private GameObject menuVideo;
    [SerializeField] private GameObject menuAudio;

    // Components
    private IInput playerInputCustom;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private PauseSystem pauseSystem;

    private void Awake()
    {
        pauseSystem = FindObjectOfType<PauseSystem>();
        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
        raycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();
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
        if (backgroundCanvas.activeSelf)
        {
            raycaster.enabled = false;
            canvas.enabled = false;
            backgroundCanvas.SetActive(false);
            MenuIntroDisable();
            MenuSettingsDisable();
            MenuControlsDisable();
            MenuVideoDisable();
            MenuAudioDisable();
        }
        else
        {
            raycaster.enabled = true;
            canvas.enabled = true;
            backgroundCanvas.SetActive(true);
            MenuIntroEnable();
        }
    }

    public void MenuIntroEnable() => menuMain.SetActive(true);
    public void MenuIntroDisable() => menuMain.SetActive(false);
    public void MenuSettingsEnable() => menuSettings.SetActive(true);
    public void MenuSettingsDisable() => menuSettings.SetActive(false);
    public void MenuControlsEnable() => menuControls.SetActive(true);
    public void MenuControlsDisable() => menuControls.SetActive(false);
    public void MenuVideoEnable() => menuVideo.SetActive(true);
    public void MenuVideoDisable() => menuVideo.SetActive(false);
    public void MenuAudioEnable() => menuAudio.SetActive(true);
    public void MenuAudioDisable() => menuAudio.SetActive(false);

    public void ButtonIntroMenuResumeGame()
    {
        EnableDisableInterface();
        pauseSystem.PauseGame();
    }
    public void ButtonIntroMenuMainMenu() => 
        SceneManager.LoadScene("MainMenu");

    public void FindInput()
    {
        if (playerInputCustom != null)
        {
            playerInputCustom.PauseGame -= EnableDisableInterface;
        }

        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
        playerInputCustom.PauseGame += EnableDisableInterface;
    }

    public void LostInput()
    {
        if (playerInputCustom != null)
        {
            playerInputCustom.PauseGame -= EnableDisableInterface;
        }
    }

    public void FindPlayer()
    {
        // Left blank on purpose
    }

    public void PlayerLost()
    {
        if (playerInputCustom != null)
            playerInputCustom.PauseGame -= EnableDisableInterface;
    }
}
