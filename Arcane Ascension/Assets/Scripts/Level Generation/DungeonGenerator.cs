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
        GenerateDungeon(false);
    }

    /// <summary>
    /// Generates a dungeon.
    /// If it's not a loaded game, it will generate values, else it will load the values saved.
    /// </summary>
    /// <param name="loadedGame">True if this game was loaded, else false.</param>
    /// <param name="saveData">Saved data.</param>
    public static void GenerateDungeon(bool loadedGame = false, SaveData saveData = null)
    {
        ElementType element;

        if (saveData == null)
        {
            // Gets random element
            int randomElement = UnityEngine.Random.Range(0, instance.levelGenerators.Count);
            element = instance.levelGenerators[randomElement].Element;
        }
        else
            element = saveData.DungeonSavedData.Element;

        foreach(LevelGenerator level in instance.levelGenerators)
        {
            if (level.Element == element)
            {
                GameObject levelGenerated = Instantiate(level.gameObject);
                LevelGenerator levelGeneratedScript = levelGenerated.GetComponent<LevelGenerator>();

                // Gets random (or pre-defined) values and starts generation
                if (loadedGame == false)
                {
                    levelGeneratedScript.GetValues();
                    levelGeneratedScript.StartGeneration();
                }
                // Else loads saved data (after loading data it will start generation)
                else
                {
                    if (saveData != null) instance.StartCoroutine(levelGeneratedScript.LoadData(saveData));
                }
            }
        }
    }
}
