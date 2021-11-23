using UnityEngine;

/// <summary>
/// Scriptable object responsible for disabling projectile immediatly.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Trigger Disable On Projectile Hit", 
    fileName = "Spell Behaviour Trigger Disable On Projectile Hit")]
sealed public class SpellBehaviourTriggerDisableOnProjectileHitSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
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
        parent.DisableImmediatly = true;
    }
}
