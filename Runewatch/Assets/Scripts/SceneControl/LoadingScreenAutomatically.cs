using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for loading a scene automatically.
/// </summary>
public class LoadingScreenAutomatically : SceneControl
{
    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    protected override IEnumerator LoadNewScene(SceneEnum scene, bool isAdditive = true)
    {
        Time.timeScale = 1;

        yield return null;

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
        GetComponent<Animator>().SetTrigger(backgroundAnimationTrigger);
    }
}
