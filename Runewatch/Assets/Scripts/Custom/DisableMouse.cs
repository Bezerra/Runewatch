using UnityEngine;

/// <summary>
/// Disables mouse.
/// </summary>
public class DisableMouse : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
