using UnityEngine;

public class UpdateChangeInput : MonoBehaviour
{
    private void Awake()
    {
        DisableMouse();
    }

    private void Start()
    {
        DisableMouse();
    }

    private void Update()
    {
        DisableMouse();
    }

    private void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
