using UnityEngine;

/// <summary>
/// Scriptable object responsible for disabling projectile immediatly.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile Immediatly", 
    fileName = "Spell Behaviour Disable Projectile Immediatly")]
sealed public class SpellBehaviourDisableProjectileImmediatlySO : SpellBehaviourAbstractSO
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
        if (parent.DisableImmediatly)
        {
            parent.DisableSpell();
        }
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
