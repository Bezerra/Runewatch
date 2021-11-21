using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for triggering enemy spawn in generated rooms.
/// </summary>
public class PlayerTriggerRoomEnemySpawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.RoomProgressLayerNum)
        {
            if (other.TryGetComponentInParent(out LevelPieceGameProgressControl progressControl))
            {
                progressControl.SpawnEnemies();
            }
        }
    }
}
