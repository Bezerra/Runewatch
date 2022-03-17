using UnityEngine;

/// <summary>
/// Deletes old files on a new version.
/// </summary>
public class UpdateGameVersionFiles : MonoBehaviour
{
    private CharacterSaveDataController characterSaveData;
    private RunSaveDataController runSaveData;
    private BossRaidRunSaveDataController bossRaidRunSaveData;

    private readonly int VERSION = 1;

    private void Awake()
    {
        characterSaveData = FindObjectOfType<CharacterSaveDataController>();
        runSaveData = FindObjectOfType<RunSaveDataController>();
        bossRaidRunSaveData = FindObjectOfType<BossRaidRunSaveDataController>();
        if (PlayerPrefs.GetInt("VersionControl", 0) <= VERSION - 1)
        {
            if (characterSaveData.FileExists()) characterSaveData.DeleteFile();
            if (runSaveData.FileExists()) runSaveData.DeleteFile();
            if (bossRaidRunSaveData.FileExists()) bossRaidRunSaveData.DeleteFile();
            PlayerPrefs.SetInt("VersionControl", VERSION);
            Debug.Log("Detected old version. Deleted files.");
        }
    }
}
