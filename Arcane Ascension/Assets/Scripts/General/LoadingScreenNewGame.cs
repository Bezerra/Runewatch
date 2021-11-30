using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// Class responsible for loading a new generation scene and start its generation.
/// </summary>
public class LoadingScreenNewGame : MonoBehaviour
{
    [SerializeField] private AudioMixer master;
    private float initialMasterValue;
    private float currentMasterValue;

    private void Awake()
    {
        master.GetFloat("MasterVolume", out initialMasterValue);
        StartCoroutine(LoadNewScene(SceneEnum.ProceduralGeneration));
    }

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(SceneEnum scene)
    {
        yield return null;
        master.SetFloat("MasterVolume", -50f);

        // Asyc loads a scene
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoad.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        // Load scene and sets it as main scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.ToString()));

        LevelGenerator levelGenerated = DungeonGenerator.GenerateDungeon();
        levelGenerated.EndedGeneration += SwitchToNewScene;
    }

    private void SwitchToNewScene()
    {
        GetComponent<Animator>().SetTrigger("FadeToUnload");
    }

    public void FadeInMasterAudioAnimationEvent() =>
        StartCoroutine(FadeInMasterAudioCoroutine());

    private IEnumerator FadeInMasterAudioCoroutine()
    {
        master.GetFloat("MasterVolume", out currentMasterValue);

        float currentTime = 0;
        while (currentTime < 1)
        {
            currentMasterValue = Mathf.Lerp(currentMasterValue, initialMasterValue, currentTime);
            master.SetFloat("MasterVolume", currentMasterValue);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void UnloadSceneAnimationEvent() =>
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("LoadingScreenNewGame"));
}
