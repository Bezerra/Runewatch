using UnityEngine;

/// <summary>
/// Class responsible for containing logic of a portal to change scenes on final room.
/// </summary>
public class NextFloorPortalSpawnPoint : MonoBehaviour
{
    [SerializeField] private Mesh meshForGizmos;
    private LoadingScreenWithTrigger[] loadingScreen;

    private void Awake() =>
        loadingScreen = FindObjectsOfType<LoadingScreenWithTrigger>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            foreach(LoadingScreenWithTrigger load in loadingScreen)
            {
                if (load.IsFinalFloor == false)
                {
                    load.LoadSceneOnSerializeField();
                    break;
                }
            }
            
            Destroy(this); // destroys script
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(meshForGizmos, transform.position, transform.rotation, new Vector3(1f, 1f, 1f));
    }
}
