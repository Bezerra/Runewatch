using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for getting actions paths.
/// </summary>
public class ActionPath : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;

    public string GetPath(BindingsAction bindingAction)
    {
        Dictionary<BindingsAction, string> bindings =
            new Dictionary<BindingsAction, string>();

        foreach (InputBinding binding in inputAction.actionMaps[0].bindings)
        {
            if (binding.action.ToString() == bindingAction.ToString())
            {
                return InputControlPath.ToHumanReadableString(binding.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }
        Debug.Log("No key found with this name");
        return null;
    }
}

