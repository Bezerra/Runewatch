using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for saving and loading game.
/// </summary>
public class SaveDataController : MonoBehaviour
{
    // Basic singleton so save and load methods can be used anywhere.
    private static SaveDataController instance;

    private SaveData saveData;
    private FileManager fileManager;

    private void Awake()
    {
        instance = this;
        saveData = new SaveData();
        fileManager = new FileManager();
    }

    /// <summary>
    /// Makes every class that implements ISaveable save their stats.
    /// </summary>
    public static void SaveGame()
    {
        IEnumerable<ISaveable> iSaveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

        foreach (ISaveable iSaveable in iSaveables)
            iSaveable.SaveCurrentData(instance.saveData);

        // Writes file with saved JSON
        if (instance.fileManager.WriteToFile("Save1.saveFile", instance.saveData.ToJson()))
            Debug.Log("Game Saved");
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public static void LoadGame()
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

        if (instance.fileManager.ReadFile("Save1.saveFile", out string json))
        {
            //instance.saveData.LoadFromJson(json);
  
            foreach (ISaveable iSaveable in iSaveables)
                instance.StartCoroutine(iSaveable.LoadData(instance.saveData));

            Debug.Log("Game Loaded");
        }
    }
}
