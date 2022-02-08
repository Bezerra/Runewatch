using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Contains methods for pause menu.
/// </summary>
public class PauseMenuCanvasUI : MonoBehaviour
{
    [SerializeField] private GameObject backgroundCanvas;
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuGeneral;
    [SerializeField] private GameObject menuSettings;
    [SerializeField] private GameObject menuControls;
    [SerializeField] private GameObject menuVideo;
    [SerializeField] private GameObject menuAudio;

    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private PauseSystem pauseSystem;

    private void Awake()
    {
        pauseSystem = FindObjectOfType<PauseSystem>();
        raycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();
    }
    private void OnEnable()
    {
        if (pauseSystem != null)
            pauseSystem.GamePausedEvent += EnablePauseInterface;
    }

    private void OnDisable()
    {
        if (pauseSystem != null)
            pauseSystem.GamePausedEvent -= EnablePauseInterface;
    }

    private void Start()
    {
        if (Application.isEditor &&
            (SceneManager.GetActiveScene().name == "Level" ||
            SceneManager.GetActiveScene().name == "MainMenu"))
            StartCoroutine(UpdateAllMenuVariables());
    }

    /// <summary>
    /// Updates all variables and disables everything after.
    /// Must be on start, after everything was loaded on awake.
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateAllMenuVariables()
    {
        backgroundCanvas.SetActive(true);
        MenuIntroEnable();
        MenuGeneralEnable();
        MenuSettingsEnable();
        MenuControlsEnable();
        MenuVideoEnable();
        MenuAudioEnable();

        yield return null;

        MenuIntroDisable();
        MenuGeneralDisable();
        MenuSettingsDisable();
        MenuControlsDisable();
        MenuVideoDisable();
        MenuAudioDisable();
        backgroundCanvas.SetActive(false);
    }

    /// <summary>
    /// Enables or disables pause interface.
    /// </summary>
    public void EnablePauseInterface()
    {
        raycaster.enabled = true;
        canvas.enabled = true;
        backgroundCanvas.SetActive(true);
        MenuIntroEnable();
    }

    /// <summary>
    /// Resumes game.
    /// </summary>
    public void ResumeGame()
    {
        raycaster.enabled = false;
        canvas.enabled = false;
        backgroundCanvas.SetActive(false);
        MenuIntroDisable();
        MenuGeneralDisable();
        MenuSettingsDisable();
        MenuControlsDisable();
        MenuVideoDisable();
        MenuAudioDisable();

        if (pauseSystem != null)
            pauseSystem.ResumeGame();
    }

    public void MenuIntroEnable() => menuMain.SetActive(true);
    public void MenuIntroDisable() => menuMain.SetActive(false);
    public void MenuGeneralEnable() => menuGeneral.SetActive(true);
    public void MenuGeneralDisable() => menuGeneral.SetActive(false);
    public void MenuSettingsEnable() => menuSettings.SetActive(true);
    public void MenuSettingsDisable() => menuSettings.SetActive(false);
    public void MenuControlsEnable() => menuControls.SetActive(true);
    public void MenuControlsDisable() => menuControls.SetActive(false);
    public void MenuVideoEnable() => menuVideo.SetActive(true);
    public void MenuVideoDisable() => menuVideo.SetActive(false);
    public void MenuAudioEnable() => menuAudio.SetActive(true);
    public void MenuAudioDisable() => menuAudio.SetActive(false);
    
    public void ButtonIntroMenuMainMenu() => SceneManager.LoadScene("MainMenu");
}
