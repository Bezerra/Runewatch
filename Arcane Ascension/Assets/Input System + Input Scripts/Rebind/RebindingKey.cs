using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingKey : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private TMP_Text bindingDisplayText;
    [SerializeField] private GameObject startRebindGameobject;
    [SerializeField] private GameObject waitingForInputGameobject;
    [SerializeField] private GameObject keyAlreadySetGameobject;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        UpdateKey();
    }

    public void UpdateKey()
    {
        string keyOverride = PlayerPrefs.GetString($"ACTION_{inputAction.action}", "null");
        if (keyOverride != "null")
        {
            inputAction.action.ApplyBindingOverride(PlayerPrefs.GetString($"ACTION_{inputAction.action}"));
        }
        
        bindingDisplayText.text =
            InputControlPath.ToHumanReadableString(inputAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebind()
    {
        keyAlreadySetGameobject.SetActive(false);
        startRebindGameobject.SetActive(false);
        waitingForInputGameobject.SetActive(true);

        rebindingOperation = inputAction.action.PerformInteractiveRebinding().
            WithControlsExcluding("Mouse").
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
            if (binding.action == currentBinding.action)
            {
                continue;
            }
            if (binding.effectivePath == currentBinding.effectivePath)
            {
                RebindCanceled();
                keyAlreadySetGameobject.SetActive(true);
                return;
            }
        }

        // If it doesn't exist, updates button text
        bindingDisplayText.text = 
            InputControlPath.ToHumanReadableString(inputAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation?.Dispose();

        waitingForInputGameobject.SetActive(false); 
        startRebindGameobject.SetActive(true);

        PlayerPrefs.SetString($"ACTION_{inputAction.action}", inputAction.action.bindings[0].effectivePath);
    }

    /// <summary>
    /// Executed if rebind was canceled through escape.
    /// </summary>
    private void RebindCanceled()
    {
        rebindingOperation?.Dispose();

        waitingForInputGameobject.SetActive(false);
        startRebindGameobject.SetActive(true);
    }
}
