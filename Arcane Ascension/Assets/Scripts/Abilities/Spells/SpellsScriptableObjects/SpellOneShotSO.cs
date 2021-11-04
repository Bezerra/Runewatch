using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptableobject for one shot spells creation.
/// </summary>
[CreateAssetMenu(menuName = "Spells/New One Shot Spell", fileName = "Spell One Shot Name")]
public class SpellOneShotSO : SpellSO
{
    [BoxGroup("Behaviours")]
    [Tooltip("What happens after the spell is spawned. Should contain at least a movement and apply damage behaviour")]
    [SerializeField] protected List<SpellBehaviourAbstractOneShotSO> spellBehaviourOneShot;

    [BoxGroup("Behaviours")]
    [Tooltip("Attack behaviour of this spell")]
    [SerializeField] protected AttackBehaviourAbstractOneShotSO attackBehaviourOneShot;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected SpellOnHitBehaviourAbstractOneShotSO onHitBehaviourOneShot;

    [BoxGroup("Behaviours Of Muzzle and Hit prefabs")]
    [Tooltip("What happens after the spell hit prefab is spawned (spawned after the spell hits something)")]
    [SerializeField] protected SpellMuzzleBehaviourAbstractOneShotSO muzzleBehaviourOneShot;

    public override IList<SpellBehaviourAbstractOneShotSO> SpellBehaviourOneShot => spellBehaviourOneShot;

    public override SpellOnHitBehaviourAbstractOneShotSO OnHitBehaviourOneShot => onHitBehaviourOneShot;

    public override SpellMuzzleBehaviourAbstractOneShotSO MuzzleBehaviourOneShot => muzzleBehaviourOneShot;

    public override AttackBehaviourAbstractSO AttackBehaviour => attackBehaviourOneShot;

    public override SpellCastType CastType => SpellCastType.OneShotCast;

}
