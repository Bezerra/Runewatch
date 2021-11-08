using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens and closes pause interface when the game is paused.
/// </summary>
public class PauseMenuReactionUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

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
        if (pauseCanvas.activeSelf)
        {
            raycaster.enabled = false;
            canvas.enabled = false;
            pauseCanvas.SetActive(false);
        }
        else
        {
            raycaster.enabled = true;
            canvas.enabled = true;
            pauseCanvas.SetActive(true);
        }
    }
}
