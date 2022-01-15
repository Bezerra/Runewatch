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
    [SerializeField] private List<ElementAndEnemyPrefab> enemyInformation;
    public EnemySpawnType EnemySpawnType => enemyType;
    public IList<ElementAndEnemyPrefab> EnemyInformation => enemyInformation;
}

/// <summary>
/// Struct with element and prefab.
/// </summary>
[Serializable]
public struct ElementAndEnemyPrefab
{
    [SerializeField] private ElementType element;
    [SerializeField] private GameObject prefab;
    public ElementType Element => element;
    public GameObject Prefab => prefab;
}
