using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Struct responsible for holding information about which prefabs
/// exist in a certain type of enemy.
/// </summary>
[Serializable]
public struct EnemyTypeAndPrefabsInformation 
{
    [SerializeField] private EnemySpawnType enemyType;

    [SerializeField] private List<GameObject> enemyPrefabs;

    public EnemySpawnType EnemySpawnType => enemyType;
    public IList<GameObject> EnemyPrefabs => enemyPrefabs;
}
