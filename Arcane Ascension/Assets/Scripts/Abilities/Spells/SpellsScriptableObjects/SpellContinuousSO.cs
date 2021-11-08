using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptableobject for continuous spell creation.
/// </summary>
[CreateAssetMenu(menuName = "Spells/New Continuous Spell", fileName = "Spell Continuous Name")]
public class SpellContinuousSO : SpellSO
{
    [BoxGroup("Behaviours")]
    [Tooltip("What happens after the spell is spawned. Should contain at least a movement and apply damage behaviour")]
    [SerializeField] protected List<SpellBehaviourAbstractContinuousSO> spellBehaviourContinuous;

    [BoxGroup("Behaviours")]
    [Tooltip("Attack behaviour of this spell")]
    [SerializeField] protected AttackBehaviourAbstractContinuousSO attackBehaviourContinuous;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected List<SpellOnHitBehaviourAbstractContinuousSO> onHitBehaviourContinuous;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected List<SpellMuzzleBehaviourAbstractContinuousSO> muzzleBehaviourContinuous;

    public override IList<SpellBehaviourAbstractContinuousSO> SpellBehaviourContinuous => spellBehaviourContinuous;

    public override IList<SpellOnHitBehaviourAbstractContinuousSO> OnHitBehaviourContinuous => onHitBehaviourContinuous;

    public override IList<SpellMuzzleBehaviourAbstractContinuousSO> MuzzleBehaviourContinuous => muzzleBehaviourContinuous;

    public override AttackBehaviourAbstractSO AttackBehaviour => attackBehaviourContinuous;
}
