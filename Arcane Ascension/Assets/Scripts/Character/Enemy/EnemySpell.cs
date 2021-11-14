using System;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Serializable struct with enemy spells information.
/// </summary>
[Serializable]
public struct EnemySpell
{
    [SerializeField] private SpellSO spell;
    [RangeMinMax(1f, 20f)] [SerializeField] private Vector2 range;
    [SerializeField] private bool needsToBeInRangeToAttack;
    [SerializeField] private bool enemyStopsOnAttack;
    [EnableIf("enemyStopsOnAttack")]
    [Range(0.1f, 2f)] [SerializeField] private float stoppingTime;

    /// <summary>
    /// Spell.
    /// </summary>
    public SpellSO Spell => spell;

    /// <summary>
    /// Two values with min and max possible ranges.
    /// </summary>
    public Vector2 RandomRange => range;

    /// <summary>
    /// A random range between two pre-defined values.
    /// </summary>
    public float Range { get; set; }

    /// <summary>
    /// Property to know if the character needs to be at least in this range to attack.
    /// </summary>
    public bool NeedsToBeInRangeToAttack => needsToBeInRangeToAttack;

    /// <summary>
    /// Property to know if the enemy stops on attack.
    /// </summary>
    public bool EnemyStopsOnAttack => enemyStopsOnAttack;

    /// <summary>
    /// Property to know how much time should the enemy be stopped.
    /// </summary>
    public float StoppingTime => stoppingTime;
}
