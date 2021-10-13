using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Class responsible for controlling scenes and spawns.
/// </summary>
public class SceneControl : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spellsPool;

    // On tests
    [SerializeField] private Image loadingBar;

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


    // ON TESTS ------------------------------------------------------------------
    public Scene CurrentScene() => SceneManager.GetActiveScene();

    public SceneEnum CurrentSceneEnum() =>
        (SceneEnum)Enum.Parse(typeof(SceneEnum), CurrentScene().name);

    /// <summary>
    /// Loads a scene.
    /// Can't overload because of animation events.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    public void LoadScene(SceneEnum scene) =>
        StartCoroutine(LoadNewScene(scene));

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(SceneEnum scene)
    {
        YieldInstruction waitForFrame = new WaitForEndOfFrame();

        DisableControls();

        // Asyc loads a scene
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(scene.ToString());

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoad.progress <= 1)
        {
            //loadingBar.fillAmount = sceneToLoad.progress;
            yield return waitForFrame;
        }
    }

    /// <summary>
    /// Disables all controls. Happens when scene is ending.
    /// </summary>
    private void DisableControls()
    {
        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        if (input != null)
        {
            input.SwitchActionMapToUI();
        }
    }
    // ON TESTS ------------------------------------------------------------------
}
