using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for making every IFindInput interface find player.
/// </summary>
public class InputFindMe : MonoBehaviour
{
    /// <summary>
    /// What happens when the player is spawned.
    /// </summary>
    private void OnEnable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindInput[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindInput>();

            foreach (IFindInput childInterface in childrenInterfaces)
            {
                childInterface.FindInput();
            }
        }
    }

    /// <summary>
    /// What happens when input is lost.
    /// </summary>
    private void OnDisable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindInput[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindInput>();

            foreach (IFindInput childInterface in childrenInterfaces)
            {
                childInterface.LostInput();
            }
        }
    }
}
