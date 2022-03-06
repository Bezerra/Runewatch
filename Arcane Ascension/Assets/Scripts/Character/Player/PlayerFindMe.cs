using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for making every IFindPlayer interface find player.
/// </summary>
public class PlayerFindMe : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// What happens when the player is spawned.
    /// </summary>
    private void OnEnable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindPlayer[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindPlayer>(true);

            foreach (IFindPlayer childInterface in childrenInterfaces)
            {
                childInterface.FindPlayer(player);
            }
        }
    }

    /// <summary>
    /// What happens when player is lost.
    /// </summary>
    private void OnDisable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindPlayer[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindPlayer>(true);

            foreach (IFindPlayer childInterface in childrenInterfaces)
            {
                childInterface.PlayerLost(player);
            }
        }
    }
}
