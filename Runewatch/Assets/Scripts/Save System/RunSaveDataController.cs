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

    [SerializeField] private RunStatsLogicSO achievementsLogic;

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
    /// Saves achievements.
    /// </summary>
    public void SaveAchievements()
    {
        achievementsLogic.SaveAchievements();

        // Writes file with saved JSON
        if (fileManager.WriteToFile("RUNPROGRESSFILE.d4s", SaveData.ToJson()))
            Debug.Log("Achievements Saved");
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

            // Loads current achievements
            achievementsLogic.LoadRunAchievements(this);

            Debug.Log("Player Progress + Achievements Loaded");
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

    private void OnValidate()
    {
        if (achievementsLogic == null)
        {
            Debug.LogError($"Achievement logic on {name} not set.");
        }
    }
}
