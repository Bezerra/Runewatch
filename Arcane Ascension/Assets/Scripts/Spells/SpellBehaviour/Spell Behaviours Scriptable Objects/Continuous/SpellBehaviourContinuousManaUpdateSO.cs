using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's continuous apply damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Continuous Mana Update",
    fileName = "Spell Behaviour Continuous Mana Update")]
public class SpellBehaviourContinuousManaUpdateSO : SpellBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // If the spell is in hit time (updated on parent)
        if (Time.time > parent.LastTimeHit + parent.Spell.Cooldown)
        {
            parent.WhoCast.ReduceMana(parent.Spell.ManaCost);
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
