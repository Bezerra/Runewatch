using UnityEngine;

/// <summary>
/// Class responsible for containing logic of a portal to change scenes on final room.
/// </summary>
public class NextFloorPortalSpawnPoint : MonoBehaviour
{
    [SerializeField] private Mesh meshForGizmos;
    private LoadingScreenWithButtonPress loadingScreen;

    private void Awake() =>
        loadingScreen = FindObjectOfType<LoadingScreenWithButtonPress>();

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 1;
        loadingScreen.LoadScene(SceneEnum.LoadingScreenToNewGame);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(meshForGizmos, transform.position, transform.rotation, new Vector3(1f, 1f, 1f));
    }
}
