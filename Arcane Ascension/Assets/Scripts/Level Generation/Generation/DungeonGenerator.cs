using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for creating a level generator.
/// </summary>
public class DungeonGenerator: MonoBehaviour
{
    [SerializeField] private List<GameObject> levelGenerators;

    private void Start()
    {
        // TEMP
        //StartCoroutine(GenerateDungeon(false));
    }

    /// <summary>
    /// Generates a dungeon.
    /// If it's not a loaded game, it will generate values, else it will load the values saved.
    /// </summary>
    /// <param name="loadedGame">True if this game was loaded, else false.</param>
    public IEnumerator GenerateDungeon(bool loadedGame = false)
    {
        RunSaveDataController runSaveDataController = FindObjectOfType<RunSaveDataController>();

        ElementType element;

        if (loadedGame == false)
        {
            // Gets random element in possible prefabs
            int randomElement = UnityEngine.Random.Range(0, levelGenerators.Count);
            element = levelGenerators[randomElement].GetComponent<LevelGenerator>().Element;
        }
        else
        {

            element = runSaveDataController.SaveData.DungeonSavedData.Element;
        }

        CleantExistingDungeonElements();

        // Gets the dungeon of that element
        foreach(GameObject level in levelGenerators)
        {
            if (level.GetComponent<LevelGenerator>().Element == element)
            {
                GameObject levelGenerated = Instantiate(level.gameObject);
                LevelGenerator levelGeneratedScript = levelGenerated.GetComponent<LevelGenerator>();

                if (loadedGame == false)
                {
                    // Gets random (or pre-defined) values and starts generation
                    levelGeneratedScript.GetValues();
                    yield return levelGeneratedScript.StartGeneration();
                }
                // Else loads saved data (after loading data it will start generation)
                else
                {
                    yield return levelGeneratedScript.LoadGeneration(runSaveDataController.SaveData);
                }
                break;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Deactivates chests and shopkeepers.
    /// </summary>
    private void CleantExistingDungeonElements()
    {
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        Chest[] chests = FindObjectsOfType<Chest>();
        Shopkeeper[] shopkeepers = FindObjectsOfType<Shopkeeper>();
        if (chests.Length > 0)
        {
            for (int i = 0; i < chests.Length; i++)
            {
                chests[i].gameObject.SetActive(false);
            }
        }
        if (shopkeepers.Length > 0)
        {
            for (int i = 0; i < shopkeepers.Length; i++)
            {
                shopkeepers[i].gameObject.SetActive(false);
            }
        }
        if (levelGenerator != null)
        {
            Destroy(levelGenerator);
        }
    }
}
