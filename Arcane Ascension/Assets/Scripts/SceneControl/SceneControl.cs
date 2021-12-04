using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

/// <summary>
/// Class responsible for controlling scenes and spawns.
/// </summary>
public class SceneControl : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spellsPool;
    [SerializeField] private Image loadingBar;

    private LevelGenerator levelGenerated;
    private RunSaveData saveData;


    public void SaveCurrentData(RunSaveData saveData)
    {
        // Left blank on purpose
    }

    public IEnumerator LoadData(RunSaveData saveData)
    {
        yield return null;
        //FindObjectOfType<PlayerInputCustom>().DisableAll();
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
        IInput input = FindObjectOfType<PlayerInputCustom>();
        input.SwitchActionMapToGameplay();

        // COMMENTED STUFF TO BE ABLE TO LOAD OUTSIDE OF GENERATED DUNGEONS
        //levelGenerated.EndedGeneration -= SpawnPlayer;
    }


    [SerializeField] protected AudioMixer master;

    [Header("String variables")]
    [SerializeField] protected string masterVolumeExposed = "MasterVolume";
    [SerializeField] protected string backgroundAnimationTrigger = "FadeToUnload";

    [Header("If using loading methods without parameters")]
    [SerializeField] protected SceneEnum sceneToLoad;
    [SerializeField] protected SceneEnum thisScene;

    [Header("Triggered when loading scene transition is ending")]
    [SerializeField] protected TypeOfControl changeToTypeOfControl;

    protected virtual void Awake()
    {
        //initialMasterValue = 0;
        //master.GetFloat(masterVolumeExposed, out initialMasterValue);
    }

    /// <summary>
    /// Gets current scene.
    /// </summary>
    /// <returns>Returns scene.</returns>
    public Scene GetCurrentScene() => 
        SceneManager.GetActiveScene();

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
    public virtual void LoadScene(SceneEnum scene) =>
        StartCoroutine(LoadNewScene(scene));

    /// <summary>
    /// Loads a scene on a serialize field.
    /// Can't overload because of animation events.
    /// </summary>
    public virtual void LoadSceneOnSerializeField() =>
        StartCoroutine(LoadNewScene(sceneToLoad, true));

    /// <summary>
    /// Sets a scene as current active scene.
    /// </summary>
    /// <param name="scene"></param>
    protected void SetActiveScene(SceneEnum scene) =>
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.ToString()));

    /// <summary>
    /// Unloads a scene
    /// </summary>
    /// <param name="scene">Scene to unload.</param>
    public virtual void UnloadScene(SceneEnum scene) =>
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scene.ToString()));

    /// <summary>
    /// Unloades all other scenes that are not related with the switching.
    /// </summary>
    public virtual void UnloadScenesThatAreaNotSwitching()
    {
        int scenes = SceneManager.sceneCount;
        for (int i = 0; i < scenes; i++)
        {
            if (SceneManager.GetSceneAt(i).name != GetCurrentScene().name &&
                SceneManager.GetSceneAt(i).name != thisScene.ToString())
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
            }
        }
    }

    /// <summary>
    /// Unloades a scene.
    /// </summary>
    public virtual void UnloadCurrentScene() =>
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(thisScene.ToString()));

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <param name="isAddictive">Is loadSceneMode additive.</param>
    /// <returns></returns>
    protected virtual IEnumerator LoadNewScene(SceneEnum scene, bool isAdditive = false)
    {
        yield return null;

        master.SetFloat(masterVolumeExposed, -50f);
        DisableControls();

        // Asyc loads a scene
        AsyncOperation sceneToLoadAsync;

        if (isAdditive == false)
            sceneToLoadAsync = SceneManager.LoadSceneAsync(scene.ToString());
        else
            sceneToLoadAsync = SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoadAsync.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        // Load scene and sets it as main scene
        SetActiveScene(scene);

        // Unloades unecessary scenes
        UnloadScenesThatAreaNotSwitching();

        // Starts loading screen animation fade out
        GetComponent<Animator>().SetTrigger(backgroundAnimationTrigger);
    }

    /// <summary>
    /// Updates master volume. Updated with OnValueChanged.
    /// </summary>
    public void UpdateMasterVolume(float currentMasterValue) =>
        master.SetFloat(masterVolumeExposed, currentMasterValue);

    /// <summary>
    /// Disables all controls. Happens when scene is starting.
    /// </summary>
    protected void DisableControls()
    {
        IInput input = FindObjectOfType<PlayerInputCustom>();
        if (input != null) input.SwitchActionMapToNone();
    }

    /// <summary>
    /// Enables all controls. Happens when scene is ending.
    /// </summary>
    protected void EnableControls()
    {
        IInput input = FindObjectOfType<PlayerInputCustom>();
        if (input != null)
        {
            input.ReenableInput();

            if (changeToTypeOfControl == TypeOfControl.UI)
            {
                input.SwitchActionMapToUI();
            }
            else
            {
                input.SwitchActionMapToGameplay();
            }
        }
    }
}
