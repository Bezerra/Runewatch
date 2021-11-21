using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling what happens when the player is inside a level piece room.
/// </summary>
public class LevelPieceGameProgressControl : MonoBehaviour
{
    [SerializeField] private BoxCollider[] exitBlockers;
    [SerializeField] private Transform[] enemySpawnTransforms;

    private int currentQuantityOfEnemies;
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;

    public void BlockExits()
    {
        if (exitBlockers.Length > 0)
        {
            foreach (BoxCollider exitBlock in exitBlockers)
            {
                exitBlock.enabled = true;
            }
        }
    }

    public void SpawnEnemies()
    {
        if (haveEnemiesSpawned == false)
        {
  
            haveEnemiesSpawned = true;
        }
    }

    private void Update()
    {
        if (haveEnemiesSpawned)
        {
            if (quantityOfEnemiesSpawned == 0)
            {
                if (exitBlockers.Length > 0)
                {
                    foreach (BoxCollider exitBlock in exitBlockers)
                    {
                        exitBlock.enabled = true;
                    }
                }
            }
        }
    }
}
