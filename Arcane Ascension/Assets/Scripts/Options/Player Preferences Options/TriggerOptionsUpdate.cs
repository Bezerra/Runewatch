using UnityEngine;

/// <summary>
/// Enables all menus from pause menu in order to update all its variables and sliders.
/// </summary>
public class TriggerOptionsUpdate : MonoBehaviour
{
    public void UpdateOptions()
    {
        PauseMenuCanvasUI pauseMenu = FindObjectOfType<PauseMenuCanvasUI>();
        pauseMenu.StartCoroutine(pauseMenu.UpdateAllMenuVariables());
    }
}
