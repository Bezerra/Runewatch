using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible for saving and loading game.
/// </summary>
public class RunSaveDataController : MonoBehaviour, IFindPlayer
{
    public RunSaveData SaveData { get; private set; }
    private FileManager fileManager;

    private void Awake()
    {
        SaveData = new RunSaveData();
        fileManager = new FileManager();
        SaveData = LoadGame();
    }

    /// <summary>
    /// Makes every class that implements ISaveable save their stats.
    /// </summary>
    public void Save()
    {
        IEnumerable<ISaveable> iSaveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

        foreach (ISaveable iSaveable in iSaveables)
            iSaveable.SaveCurrentData(SaveData);

        // Writes file with saved JSON
        if (fileManager.WriteToFile("RUNPROGRESSFILE.d4s", SaveData.ToJson()))
            Debug.Log("Run Progress Saved");
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public RunSaveData LoadGame()
    {
        IEnumerable<IDungeonSaveable> iSaveables = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDungeonSaveable>();

        if (fileManager.ReadFile("RUNPROGRESSFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);

            foreach (IDungeonSaveable iSaveable in iSaveables)
                iSaveable.LoadData(SaveData);

            Debug.Log("Run Progress Loaded");
            return SaveData;
        }
        return SaveData;
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public IEnumerator LoadPlayerData()
    {
        IEnumerable<IPlayerSaveable> iSaveables =
            FindObjectsOfType<MonoBehaviour>().OfType<IPlayerSaveable>();

        if (fileManager.ReadFile("RUNPROGRESSFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);

            foreach (IPlayerSaveable iSaveable in iSaveables)
                yield return iSaveable.LoadData(SaveData);

            Debug.Log("Player Progress Loaded");
        }

        yield return null;
    }

    /// <summary>
    /// Deletes run progress fgile.
    /// </summary>
    public void DeleteFile()
    {
        fileManager.DeleteFile("RUNPROGRESSFILE.d4s");
        SaveData = new RunSaveData();
    }

    /// <summary>
    /// Checks if file exists
    /// </summary>
    /// <returns>True if the file exists.</returns>
    public bool FileExists()
    {
        if (fileManager.FileExists("RUNPROGRESSFILE.d4s"))
            return true;
        return false;
    }

    public void FindPlayer()
    {
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

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
