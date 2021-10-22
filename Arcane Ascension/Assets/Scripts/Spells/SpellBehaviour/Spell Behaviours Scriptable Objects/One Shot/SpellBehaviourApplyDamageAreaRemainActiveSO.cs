using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying normal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Area Remain Active",
    fileName = "Spell Behaviour Apply Damage Area Remain Active")]
public class SpellBehaviourApplyDamageAreaRemainActiveSO : SpellBehaviourAbstractOneShotSO
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
        if (Time.time - parent.LastTimeDamaged > parent.Spell.TimeInterval)
        {
            parent.Spell.DamageBehaviour.Damage(parent);
            parent.LastTimeDamaged = Time.time;
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
