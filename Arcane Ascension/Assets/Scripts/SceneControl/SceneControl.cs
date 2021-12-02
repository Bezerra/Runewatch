using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controlling scenes and spawns.
/// </summary>
public abstract class SceneControl : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spellsPool;
    [SerializeField] private Image loadingBar;

    private LevelGenerator levelGenerated;
    private RunSaveData saveData;

    private void Awake()
    {
        levelGenerated = FindObjectOfType<LevelGenerator>();
    }

    public void SpawnPlayerTEST(Transform playerSpawnTransform)
    {
        Instantiate(
            playerPrefab, playerSpawnTransform.position,
            playerSpawnTransform.rotation);

        // Enables controls
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGameplay();
    }

    public void SaveCurrentData(RunSaveData saveData)
    {
        // Left blank on purpose
    }

    public IEnumerator LoadData(RunSaveData saveData)
    {
        yield return null;
        FindObjectOfType<PlayerInputCustom>().DisableAll();
        this.saveData = saveData;

        // Destruction

        // Destroy spell pool
        if (FindObjectOfType<SpellPoolCreator>() != null) Destroy(FindObjectOfType<SpellPoolCreator>().gameObject);

        // Destroy player
        if (FindObjectOfType<Player>() != null) Destroy(FindObjectOfType<Player>().transform.parent.gameObject);


        // COMMENTED STUFF TO BE ABLE TO LOAD OUTSIDE OF GENERATED DUNGEONS
        // Destroy level
        //     Destroy(FindObjectOfType<LevelGenerator>().gameObject);
        //     GameObject[] levelParents = GameObject.FindGameObjectsWithTag("LevelParent");
        //     if (levelParents.Length > 0) foreach (GameObject lvlParent in levelParents) Destroy(lvlParent?.gameObject);
        //     
        //     // Creation
        //     // Creates a new level
        //     DungeonGenerator.GenerateDungeon(true, saveData);

        // Creates a new spell pool
        Instantiate(spellsPool, Vector3.zero, Quaternion.identity);

        // DELETE THIS WHEN TESTS WITHOUT GENERATED DUNGEONS ARE OVER
        //SpawnPlayer();
        //////////////////////////////////////

        // Will spawn player after the level is loaded
        levelGenerated = FindObjectOfType<LevelGenerator>();
        //levelGenerated.EndedGeneration += SpawnPlayer;
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

        // COMMENTED STUFF TO BE ABLE TO LOAD OUTSIDE OF GENERATED DUNGEONS
        //levelGenerated.EndedGeneration -= SpawnPlayer;
    }



    /// <summary>
    /// Gets current scene.
    /// </summary>
    /// <returns>Returns scene.</returns>
    public Scene GetCurrentScene() => SceneManager.GetActiveScene();

    /// <summary>
    /// Gets current scene enum.
    /// </summary>
    /// <returns>Returns a scene enum.</returns>
    public SceneEnum CurrentSceneEnum() =>
        (SceneEnum)Enum.Parse(typeof(SceneEnum), GetCurrentScene().name);

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
    protected abstract IEnumerator LoadNewScene(SceneEnum scene);

    /// <summary>
    /// Disables all controls. Happens when scene is ending.
    /// </summary>
    protected void DisableControls()
    {
        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        if (input != null) input.SwitchActionMapToUI();
    }
}
