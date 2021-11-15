using System;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Serializable struct with enemy spells information.
/// </summary>
[Serializable]
public struct EnemySpell
{
    [Space(20f)]
    [Tooltip("Minim and maximum range. In the begging of a spell selection, the enemy will pick" +
        "a randm value between this X and Y.")]
    [SerializeField] private SpellSO spell;
    [RangeMinMax(2.5f, 20f)] [SerializeField] private Vector2 range; // 2.5 MINIMUM BECAUSE OF THE ENEMY COLLISION AGAINST PLAYER

    [Tooltip("The enemy will only cast a spell when it reaches the range obtained on the " +
        "beggining of the attack.")]
    [SerializeField] private bool needsToBeInRangeToAttack;

    [Space(25f)]
    [Tooltip("As soon as the enemy starts casting any spell, it will stop.")]
    [InfoBox("If spell is one shot cast with release 'Enemy Stops On Attack' variable MUST BE TRUE.")]
    [SerializeField] private bool enemyStopsOnAttack;

    [EnableIf("enemyStopsOnAttack")]
    [Tooltip("As soon as the enemy starts casting any spell, it will stop for this time")]
    [Range(0.1f, 4f)] [SerializeField] private float stoppingTime;

    [EnableIf("@this.spell != null && this.spell.CastType == SpellCastType.OneShotCastWithRelease && enemyStopsOnAttack")]
    [Tooltip("Percentage of stopping time to start showing AoE spell area. For example, if stopping time" +
        "is 1 second and this float is 0.5f, at 0.5 seconds the area will start being shown." +
        "Set to 1 to show no area. Set to 0 to show the area the whole time of the spell cast.")]
    [Range(0, 1f)][SerializeField] private float percentageStoppingTimeTriggerAoESpell;
    
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

    /// <summary>
    /// Percentage time of stopping time to release one shot with release spell.
    /// </summary>
    public float PercentageStoppingTimeTriggerAoESpell => percentageStoppingTimeTriggerAoESpell;
}
