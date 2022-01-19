using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for triggering enemy spawn in generated rooms.
/// </summary>
public class PlayerTriggerRoomEnemySpawn : MonoBehaviour
{
    [SerializeField] private bool triggerEnemySpawnColliders = true;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerEnemySpawnColliders)
        {
            if (other.gameObject.layer == Layers.RoomProgressLayerNum)
            {
                if (other.TryGetComponentInParent(
                    out LevelPieceGameProgressControlAbstract progressControl))
                {
                    progressControl.SpawnEnemies();
                    triggerEnemySpawnColliders = false;
                }
            }
        }
    }
}
