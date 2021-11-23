using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying aoe damage on hit.
/// Uses spell Speed as timer to deal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage AoE After Time On Impact",
    fileName = "Spell Behaviour Apply Damage AoE After Time On Impact")]
public class SpellBehaviourApplyDamageAoEAfterTimeOnImpactSO : SpellBehaviourAbstractOneShotSO
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
        if (Time.time - parent.TimeOfImpact > parent.Spell.DelayToDoDamage)
        {
            if (parent.DisableImmediatly == false)
            {
                parent.Spell.DamageBehaviour.Damage(parent);
                parent.DisableImmediatly = true;
            }   
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
