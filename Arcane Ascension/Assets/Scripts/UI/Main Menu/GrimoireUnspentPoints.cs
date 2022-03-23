using UnityEngine;

/// <summary>
/// Controls an icon. Turns it on or off depending if new arcane power points
/// were obtained.
/// </summary>
public class GrimoireUnspentPoints : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject grimoireMenu;

    private CharacterSaveDataController characterSaveData;

    private void Awake()
    {
        characterSaveData = FindObjectOfType<CharacterSaveDataController>();
    }

    private void OnEnable()
    {
        if (characterSaveData.SaveData.ArcanePower > 
            PlayerPrefs.GetInt("ArcanePowerOnLastEnable", 0))
        {
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }

        PlayerPrefs.SetInt("ArcanePowerOnLastEnable", 
            characterSaveData.SaveData.ArcanePower);
    }

    private void Update()
    {
        if (grimoireMenu.activeSelf)
        {
            if (icon.activeSelf)
            {
                icon.SetActive(false);
            }
        }
    }
}
