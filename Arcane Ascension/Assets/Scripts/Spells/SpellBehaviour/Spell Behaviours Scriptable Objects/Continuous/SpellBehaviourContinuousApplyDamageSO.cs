using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's continuous apply damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Continuous Apply Damage",
    fileName = "Spell Behaviour Continuous Apply Damage")]
public class SpellBehaviourContinuousApplyDamageSO : SpellBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        if (parent.DamageableTarget != null)
        {
            // If the spell is in hit time (updated on parent)
            if (Time.time > parent.LastTimeHit + parent.Spell.Cooldown)
            {
                parent.Spell.DamageBehaviour.Damage(parent.DamageableTarget, parent);
            }
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }
}
