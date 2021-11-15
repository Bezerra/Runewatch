using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with stats for an enemy.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStatsSO : StatsSO
{
    [BoxGroup("Damage Stats")]
    [Header("Character list of spells")]
    [SerializeField] private List<EnemySpell> availableSpells;

    public List<EnemySpell> AllEnemySpells => availableSpells;
}
