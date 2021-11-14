using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for melee attack behaviours.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Melee Attack", 
    fileName = "Spell Behaviour Apply Damage Melee Attack")]
sealed public class SpellBehaviourApplyDamageMeleeAttackSO : SpellBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Sets variables used to do damage. Applies damage behaviour.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.SpellStartedMoving = true;
        parent.transform.position = parent.Hand.position;
        parent.PositionOnHit = parent.Hand.position;
        parent.Spell.DamageBehaviour.Damage(parent);
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
