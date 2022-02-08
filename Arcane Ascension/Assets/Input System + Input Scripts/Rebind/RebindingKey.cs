using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Class responsible for rebinding a key.
/// </summary>
public class RebindingKey : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private InputActionReference inputActionSecondary;
    [SerializeField] private TMP_Text bindingDisplayText;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private GameObject startRebindGameobject;
    [SerializeField] private GameObject waitingForInputGameobject;
    [SerializeField] private GameObject keyAlreadySetGameobject;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public RebindingManager RebindingManager { get; private set; }

    private void OnValidate() =>
        UpdateTextWithActionInformation();

    /// <summary>
    /// Updates text with action name.
    /// </summary>
    private void UpdateTextWithActionInformation()
    {
        if (inputAction != null)
        {
            actionText.text = inputAction.action.bindings[0].action.ToString();
            bindingDisplayText.text =
                InputControlPath.ToHumanReadableString(inputAction.action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            if (RebindingManager != null)
                RebindingManager.OnKeybindingsUpdated();
        }
    }

    /// <summary>
    /// Updates key asset and its button text.
    /// </summary>
    public void Initialize(RebindingManager rebindingManager)
    {
        RebindingManager = rebindingManager;

        string keyOverride = PlayerPrefs.GetString($"ACTION_{inputAction.action.bindings[0].action}", "null");
        if (keyOverride != "null")
        {
            inputAction.action.ApplyBindingOverride(
                PlayerPrefs.GetString($"ACTION_{inputAction.action.bindings[0].action}"));

            if (inputActionSecondary != null)
            {
                inputActionSecondary.action.ApplyBindingOverride(
                PlayerPrefs.GetString($"ACTION_{inputAction.action.bindings[0].action}"));
            }
        }

        UpdateTextWithActionInformation();
    }

    /// <summary>
    /// Starts a rebind operation.
    /// </summary>
    public void StartRebind()
    {
        keyAlreadySetGameobject.SetActive(false);
        startRebindGameobject.SetActive(false);
        waitingForInputGameobject.SetActive(true);
        RebindingManager.DeactivateKeysAlreadySet();

        PlayerPrefs.SetString("CurrentKeyPath", inputAction.action.bindings[0].effectivePath);

        rebindingOperation = inputAction.action.PerformInteractiveRebinding().
            WithControlsExcluding("<Pointer>/position").
            WithCancelingThrough("<Keyboard>/escape").
            OnMatchWaitForAnother(0.1f).
            OnCancel(operation => RebindCanceled()).
            OnComplete(operation => RebindComplete()).
            Start();
    }

    /// <summary>
    /// Executed if rebind was complete.
    /// </summary>
    private void RebindComplete()
    {
        // Checks if binding already exists
        InputBinding currentBinding = inputAction.action.bindings[0];
        foreach (InputBinding binding in inputAction.action.actionMap.bindings)
        {
            if (binding.action != null)
            {
                // If it's checking this action
                if (binding.action == currentBinding.action)
                {
                    // Updates secondary key to be the same as this one
                    if (inputActionSecondary != null)
                    {
                        inputActionSecondary.action.ApplyBindingOverride(
                            inputAction.action.bindings[0].effectivePath);
                    }

                    continue;
                }
                // If there's a key with the same path already
                if (binding.effectivePath == currentBinding.effectivePath)
                {
                    RebindCanceled();
                    keyAlreadySetGameobject.SetActive(true);
                    inputAction.action.ApplyBindingOverride(
                        PlayerPrefs.GetString("CurrentKeyPath"));

                    if (inputActionSecondary != null)
                    {
                        inputActionSecondary.action.ApplyBindingOverride(
                        PlayerPrefs.GetString("CurrentKeyPath"));
                    }

                    return;
                }
            }
        }

        // If the path of this action doesn't exist, updates button text
        UpdateTextWithActionInformation();
        Debug.Log("END");
        // Saves key to player prefs
        PlayerPrefs.SetString($"ACTION_{inputAction.action.bindings[0].action}",
            inputAction.action.bindings[0].effectivePath);

        Clean();

        waitingForInputGameobject.SetActive(false); 
        startRebindGameobject.SetActive(true);
    }

    /// <summary>
    /// Executed if rebind was canceled through escape.
    /// </summary>
    private void RebindCanceled()
    {
        Clean();

        waitingForInputGameobject.SetActive(false);
        startRebindGameobject.SetActive(true);
    }

    /// <summary>
    /// Disposes rebinding operation.
    /// </summary>
    private void Clean()
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;
    }

    /// <summary>
    /// Deactivates keyalreadyset gameobject.
    /// </summary>
    public void DeactivateKeyAlreadySet()
    {
        if (keyAlreadySetGameobject.activeSelf) 
            keyAlreadySetGameobject.SetActive(false);
    }
}
