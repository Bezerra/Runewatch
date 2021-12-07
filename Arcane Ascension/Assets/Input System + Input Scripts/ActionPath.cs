using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for getting actions paths.
/// </summary>
public class ActionPath : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;

    /// <summary>
    /// This
    /// </summary>
    /// <param name="bindingAction"></param>
    /// <returns></returns>
    public string GetPath(BindingsAction bindingAction)
    {
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

