using System;
using UnityEngine;

/// <summary>
/// Struct responsible for holding an enemy spawn point information.
/// </summary>
[Serializable]
public struct EnemySpawnPointInformation
{
    [SerializeField] private DifficultyType difficulty;

    [SerializeField] private EnemySpawnType enemySpawnType;

    public DifficultyType Difficulty => difficulty;

    public EnemySpawnType EnemySpawnType => enemySpawnType;
}
