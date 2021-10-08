using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens and closes pause interface when the game is paused.
/// </summary>
public class PauseMenuReactionUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private PlayerInputCustom playerInputCustom;

    private void Awake()
    {
        playerInputCustom = FindObjectOfType<PlayerInputCustom>();
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
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
    }
}
