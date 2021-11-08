using UnityEngine;

/// <summary>
/// Scriptable object responsible for disable parent spell behaviour if spell is too distant from the caster.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile At Maximum Distance", 
    fileName = "Spell Behaviour Disable Projectile At Maximum Distance")]
public class SpellBehaviourDisableProjectileAtMaximumDistanceSO : SpellBehaviourAbstractOneShotSO
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
        // If the spell hits something
        if (Vector3.Distance(parent.transform.position, parent.WhoCast.transform.position) > parent.Spell.MaximumDistance)
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
