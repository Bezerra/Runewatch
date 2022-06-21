using System;
using UnityEngine;

/// <summary>
/// Class responsible for updating input on the beggining.
/// </summary>
public class RebindingManager : MonoBehaviour
{
    private RebindingKey[] rebindKeys;

    private void Awake()
    {
        rebindKeys = GetComponentsInChildren<RebindingKey>(true);
    }

    private void Start()
    {
        UpdateKeys();
    }

    /// <summary>
    /// Deactivates all key already set buttons.
    /// </summary>
    public void DeactivateKeysAlreadySet()
    {
        foreach (RebindingKey rebinding in rebindKeys)
            rebinding.DeactivateKeyAlreadySet();
    }

    public void UpdateKeys()
    {
        foreach (RebindingKey rebinding in rebindKeys)
            rebinding.Initialize(this);
    }

    public virtual void OnKeybindingsUpdated() => KeybindingsUpdated?.Invoke();
    public event Action KeybindingsUpdated;
}
