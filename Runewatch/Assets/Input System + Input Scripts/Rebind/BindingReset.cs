using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls reset.
/// </summary>
public class BindingReset : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    // Resets controls
    public void ResetAllBindings()
    {
        foreach(InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }

        PlayerPrefs.DeleteKey("rebinds");
    }
}
