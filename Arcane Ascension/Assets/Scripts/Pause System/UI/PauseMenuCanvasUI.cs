using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains methods for pause menu.
/// </summary>
public class PauseMenuCanvasUI : MonoBehaviour
{
    [SerializeField] private GameObject backgroundCanvas;
    [SerializeField] private GameObject menuIntro;
    [SerializeField] private GameObject menuOptions;

    // Components
    private PlayerInputCustom playerInputCustom;
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
        }
        else
        {
            raycaster.enabled = true;
            canvas.enabled = true;
            backgroundCanvas.SetActive(true);
            MenuIntroEnable();
        }
    }

    public void MenuIntroEnable() => menuIntro.SetActive(true);
    public void MenuIntroDisable() => menuIntro.SetActive(false);
    public void MenuOptionsEnable() => menuOptions.SetActive(true);
    public void MenuOptionsDisable() => menuOptions.SetActive(false);

    public void ButtonIntroMenuResumeGame()
    {
        EnableDisableInterface();
        pauseSystem.PauseGame();
    }
    public void ButtonIntroMenuMainMenu() => 
        SceneManager.LoadScene("MainMenu");
}
