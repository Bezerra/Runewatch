using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens and closes pause interface when the game is paused.
/// </summary>
public class PauseMenuReactionUI : MonoBehaviour
{
    [SerializeField] private GameObject backgroundCanvas;
    [SerializeField] private GameObject menuIntro;
    [SerializeField] private GameObject menuOptions;

    // Components
    private PlayerInputCustom playerInputCustom;
    private Canvas canvas;
    private GraphicRaycaster raycaster;

    private void Awake()
    {
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
        }
    }

    public void MenuIntroEnable() => menuIntro.SetActive(true);
    public void MenuIntroDisable() => menuIntro.SetActive(false);
    public void MenuOptionsEnable() => menuOptions.SetActive(true);
    public void MenuOptionsDisable() => menuOptions.SetActive(false);
}
