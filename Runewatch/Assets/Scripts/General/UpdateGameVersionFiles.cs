using UnityEngine;

/// <summary>
/// Deletes old files on a new version.
/// </summary>
public class UpdateGameVersionFiles : MonoBehaviour
{
    private CharacterSaveDataController characterSaveData;
    private RunSaveDataController runSaveData;
    private BossRaidRunSaveDataController bossRaidRunSaveData;

    private readonly float VERSION = 20f;

    private void Awake()
    {
        characterSaveData = FindObjectOfType<CharacterSaveDataController>();
        runSaveData = FindObjectOfType<RunSaveDataController>();
        bossRaidRunSaveData = FindObjectOfType<BossRaidRunSaveDataController>();
        if (PlayerPrefs.GetFloat(PPrefsGeneral.VersionControl.ToString(), 0) < VERSION)
        {
            if (characterSaveData.FileExists()) characterSaveData.DeleteFile();
            if (runSaveData.FileExists()) runSaveData.DeleteFile();
            if (bossRaidRunSaveData.FileExists()) bossRaidRunSaveData.DeleteFile();
            PlayerPrefs.SetFloat(PPrefsGeneral.VersionControl.ToString(), VERSION);
            Debug.Log("Detected old version. Deleted files.");
        }
    }

    private void OnValidate()
    {
        if (Mathf.FloorToInt(VERSION) != int.Parse(Application.version.Split('.')[0]))
        {
            Debug.LogError($"Version on Player Settings: " +
                $"{Application.version.Split('.')[0]};" +
                $" Current Version: {VERSION};\nVersions don't match.");
        }
    }
}
