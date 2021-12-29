using UnityEngine;

/// <summary>
/// Class responsible for containing logic of a portal to change scenes on final room.
/// </summary>
public class NextFloorPortalSpawnPoint : MonoBehaviour
{
    [SerializeField] private Mesh meshForGizmos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            RunSaveData saveData = FindObjectOfType<RunSaveDataController>().SaveData;
            LoadingScreenWithTrigger[] loadingScreen = FindObjectsOfType<LoadingScreenWithTrigger>();

            // If not last floor
            // loads next floor
            if (saveData.DungeonSavedData.Floor < 9)
            {
                foreach (LoadingScreenWithTrigger load in loadingScreen)
                {
                    if (load.IsFinalFloor == false)
                    {
                        load.LoadSceneOnSerializeField();
                        break;
                    }
                }
            }
            else // Else it will load the final scene
            {
                foreach (LoadingScreenWithTrigger load in loadingScreen)
                {
                    if (load.IsFinalFloor)
                    {
                        load.LoadSceneOnSerializeField();
                        break;
                    }
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
