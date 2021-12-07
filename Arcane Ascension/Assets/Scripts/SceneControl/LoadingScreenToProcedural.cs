using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for loading a new generation scene and start its generation.
/// </summary>
public class LoadingScreenToProcedural : SceneControl
{
    [SerializeField] private bool loadingSaveFile;

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    protected override IEnumerator LoadNewScene(SceneEnum scene, bool isAdditive = true)
    {
        Time.timeScale = 1;

        yield return null;

        // Unloades unecessary scenes
        UnloadScenesThatAreaNotSwitching();

        master.SetFloat("MasterVolume", -50f);
        DisableControls();

        // Asyc loads a scene
        AsyncOperation sceneToLoadAsync =
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoadAsync.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        // Load scene and sets it as main scene
        SetActiveScene(sceneToLoad);

        if (loadingSaveFile == false)
        {
            // Waits until scene is generated
            DungeonGenerator dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            yield return dungeonGenerator.GenerateDungeon();
        }
        else
        {
            // Waits until scene is generated
            DungeonGenerator dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            yield return dungeonGenerator.GenerateDungeon(true);
        }
        

        // Starts loading screen animation fade out
        GetComponent<Animator>().SetTrigger(backgroundAnimationTrigger);
    }
}
