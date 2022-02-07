using UnityEngine;

/// <summary>
/// Abstract class responsible for updating player prefs.
/// </summary>
public abstract class PlayerPreferencesOption : MonoBehaviour
{
    /// <summary>
    /// TEMP, DPS FAZER 1 class PARA FAZER UPDATE A TODOS
    /// </summary>
    private void OnEnable() => UpdateValueToMatchPlayerPrefs();

    protected abstract void UpdateValueToMatchPlayerPrefs();

    public abstract void UpdateValue(float value);
}
