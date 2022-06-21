using UnityEngine;

/// <summary>
/// Abstract class responsible for updating player prefs.
/// </summary>
public abstract class PlayerPreferencesOption : MonoBehaviour
{
    private void OnEnable() => UpdateValueToMatchPlayerPrefs();

    protected abstract void UpdateValueToMatchPlayerPrefs();

    public abstract void UpdateValue(float value);
}
