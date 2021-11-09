using UnityEngine;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    public EnemyStatsSO EnemyAttributes => character.CommonValues.CharacterStats as EnemyStatsSO;
}
