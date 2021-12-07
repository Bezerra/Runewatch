using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for saving and loading game.
/// </summary>
public class RunSaveDataController : MonoBehaviour
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
        HashSet<ISaveable> iSaveables = new HashSet<ISaveable>();
        IEnumerable<GameObject> allGameObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            if (obj.TryGetComponent<ISaveable>(out ISaveable save) &&
                obj.TryGetComponentInParent<SelectionBase>(out SelectionBase character) == false)
            {
                iSaveables.Add(save);
            }
        }

        if (fileManager.ReadFile("RUNPROGRESSFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);

            foreach (ISaveable iSaveable in iSaveables)
                StartCoroutine(iSaveable.LoadData(SaveData));

            Debug.Log("Game Loaded");
            return SaveData;
        }
        else
        {
            if (fileManager.WriteToFile("RUNPROGRESSFILE.d4s", SaveData.ToJson()))
                Debug.Log("No run progress save found. Created new file.");
            SaveData.LoadFromJson(json);
            return SaveData;
        }
    }
}
