using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Saves and loads player custom controls.
/// Needs input system 1.1.0 preview 3.
/// </summary>
public class RebindSaveLoad : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    /*
    private void OnEnable()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", null);
        if (!string.IsNullOrEmpty(rebinds))
            inputActions.LoadBindingOverridesFromJson(rebinds);
    }

    private void OnDisable()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
    */
}
