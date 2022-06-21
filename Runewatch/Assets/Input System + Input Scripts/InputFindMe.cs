using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for making every IFindInput interface find player.
/// </summary>
public class InputFindMe : MonoBehaviour
{
    private PlayerInputCustom input;

    private void Awake()
    {
        input = GetComponent<PlayerInputCustom>();
    }

    /// <summary>
    /// What happens when the input is spawned.
    /// </summary>
    private void OnEnable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindInput[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindInput>(true);

            foreach (IFindInput childInterface in childrenInterfaces)
            {
                childInterface.FindInput(input);
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
                rootGameObject.GetComponentsInChildren<IFindInput>(true);

            foreach (IFindInput childInterface in childrenInterfaces)
            {
                childInterface.LostInput(input);
            }
        }
    }
}
