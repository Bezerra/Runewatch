using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible for saving and loading game on boss raids.
/// </summary>
public class BossRaidRunSaveDataController : MonoBehaviour, IFindPlayer
{
    public BossRaidRunSaveData SaveData { get; private set; }
    private FileManager fileManager;

    private void Awake()
    {
        SaveData = new BossRaidRunSaveData();
        fileManager = new FileManager();
        SaveData = LoadGame();
    }

    /// <summary>
    /// Makes every class that implements ISaveable save their stats.
    /// </summary>
    public void Save()
    {
        IEnumerable<IBossRaidSaveable> iSaveables = FindObjectsOfType<MonoBehaviour>().OfType<IBossRaidSaveable>();

        foreach (IBossRaidSaveable iSaveable in iSaveables)
            iSaveable.SaveCurrentData(SaveData);

        // Writes file with saved JSON
        if (fileManager.WriteToFile("BOSSRAIDRUNPROGRESSFILE.d4s", SaveData.ToJson()))
            Debug.Log("Boss Raid Run Progress Saved");
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public IEnumerator LoadPlayerData()
    {
        IEnumerable<IBossRaidSaveable> iSaveables =
            FindObjectsOfType<MonoBehaviour>().OfType<IBossRaidSaveable>();

        if (fileManager.ReadFile("BOSSRAIDRUNPROGRESSFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);

            foreach (IBossRaidSaveable iSaveable in iSaveables)
                yield return iSaveable.LoadData(SaveData);

            Debug.Log("Raid Boss Run Progress Loaded");
        }

        yield return null;
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public BossRaidRunSaveData LoadGame()
    {
        if (fileManager.ReadFile("BOSSRAIDRUNPROGRESSFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);

            Debug.Log("Boss Raid Run Progress Loaded");
            return SaveData;
        }
        return SaveData;
    }

    /// <summary>
    /// Deletes run progress fgile.
    /// </summary>
    public void DeleteFile()
    {
        fileManager.DeleteFile("BOSSRAIDRUNPROGRESSFILE.d4s");
        SaveData = new BossRaidRunSaveData();
    }

    /// <summary>
    /// Checks if file exists
    /// </summary>
    /// <returns>True if the file exists.</returns>
    public bool FileExists()
    {
        if (fileManager.FileExists("BOSSRAIDRUNPROGRESSFILE.d4s"))
            return true;
        return false;
    }

    public void FindPlayer(Player player)
    {
        if (gameObject.activeSelf)
            StartCoroutine(LoadPlayerDataCoroutine());
    }

    private IEnumerator LoadPlayerDataCoroutine()
    {
        yield return new WaitForFixedUpdate();

        // Updates player stats
        yield return LoadPlayerData();

        // Saves every run progress to a file
        Save();
    }

    public void PlayerLost(Player player)
    {
        // Left blank on purpose
    }
}
