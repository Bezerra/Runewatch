using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for loading a new scene with a button press.
/// </summary>
public class LoadingScreenWithButtonPress : SceneControl
{
    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    protected override IEnumerator LoadNewScene(SceneEnum scene, bool isAdditive = true)
    {
        DisableControls();
        
        // Asyc loads a scene
        AsyncOperation sceneToLoadAsync =
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoadAsync.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        // Needs to destroy current input, so it won't mess with next scene's input
        Destroy(FindObjectOfType<PlayerInputCustom>().gameObject);

        // Load scene and sets it as main scene
        SetActiveScene(sceneToLoad);
    }

    public void QuitGame() =>
        Application.Quit();
}
