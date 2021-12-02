using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for creating a level generator.
/// </summary>
public class DungeonGenerator: MonoBehaviour
{
    [SerializeField] private List<LevelGenerator> levelGenerators;
    private static DungeonGenerator instance;

    private void Awake() =>
        instance = this;

    private void Start()
    {
        // TEMP
        //GenerateDungeon(false);
    }

    /// <summary>
    /// Generates a dungeon.
    /// If it's not a loaded game, it will generate values, else it will load the values saved.
    /// </summary>
    /// <param name="loadedGame">True if this game was loaded, else false.</param>
    /// <param name="saveData">Saved data.</param>
    public static IEnumerator GenerateDungeon(bool loadedGame = false, RunSaveData saveData = null)
    {
        ElementType element;

        if (saveData == null)
        {
            // Gets random element in possible prefabs
            int randomElement = UnityEngine.Random.Range(0, instance.levelGenerators.Count);
            element = instance.levelGenerators[randomElement].Element;
        }
        else
            element = saveData.DungeonSavedData.Element;

        // Gets the dungeon of that element
        foreach(LevelGenerator level in instance.levelGenerators)
        {
            if (level.Element == element)
            {
                GameObject levelGenerated = Instantiate(level.gameObject);
                LevelGenerator levelGeneratedScript = levelGenerated.GetComponent<LevelGenerator>();

                if (saveData == null)
                {
                    // Gets random (or pre-defined) values and starts generation
                    levelGeneratedScript.GetValues();
                    yield return levelGeneratedScript.StartGeneration();
                }
                // Else loads saved data (after loading data it will start generation)
                else
                {
                    yield return levelGeneratedScript.LoadData(saveData);
                }
                break;
            }
        }

        yield return null;
    }
}
