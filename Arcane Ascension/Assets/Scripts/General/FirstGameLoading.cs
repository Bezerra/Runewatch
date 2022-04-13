using UnityEngine;

/// <summary>
/// Sets player prefs variable for first loading.
/// </summary>
public class FirstGameLoading : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetInt(PPrefsGeneral.MenuLoadFirstTime.ToString(), 0);
    }
}
