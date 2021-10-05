using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling scenes and spawns.
/// </summary>
public class SceneControl : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spellsPool;

    private LevelGenerator levelGenerated;
    private SaveData saveData;

    public void SaveCurrentData(SaveData saveData)
    {
        // Left blank on purpose
    }

    public IEnumerator LoadData(SaveData saveData)
    {
        yield return null;
        FindObjectOfType<PlayerInputCustom>().DisableAll();
        this.saveData = saveData;

        // Destruction

        // Destroy spell pool
        if (FindObjectOfType<SpellPoolCreator>() != null) Destroy(FindObjectOfType<SpellPoolCreator>().gameObject); 

        // Destroy player
        if (FindObjectOfType<Player>() != null) Destroy(FindObjectOfType<Player>().transform.parent.gameObject);

        // Destroy level
        Destroy(FindObjectOfType<LevelGenerator>().gameObject);
        GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        if (levelParents.Length > 0) foreach (GameObject lvlParent in levelParents) Destroy(lvlParent?.gameObject);



        // Creation

        // Creates a new level
        DungeonGenerator.GenerateDungeon(true, saveData);

        // Creates a new spell pool
        Instantiate(spellsPool, Vector3.zero, Quaternion.identity);

        // Will spawn player after the level is loaded
        levelGenerated = FindObjectOfType<LevelGenerator>();
        levelGenerated.EndedGeneration += SpawnPlayer;
    }

    /// <summary>
    /// Spawns player.
    /// </summary>
    private void SpawnPlayer()
    {
        // Spawn player
        GameObject player = Instantiate(
            playerPrefab, saveData.PlayerSavedData.Position,
            saveData.PlayerSavedData.Rotation);

        // Loads player variables
        foreach (ISaveable iSaveable in player.GetComponentsInChildren<ISaveable>())
            StartCoroutine(iSaveable.LoadData(saveData));

        // Enables controls
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGameplay();

        levelGenerated.EndedGeneration -= SpawnPlayer;
    }
}
