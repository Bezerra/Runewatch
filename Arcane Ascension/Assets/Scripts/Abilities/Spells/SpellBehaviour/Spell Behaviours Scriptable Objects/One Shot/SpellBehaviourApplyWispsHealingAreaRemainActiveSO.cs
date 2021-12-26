using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying damage while the projectile is enabled.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Wisps Healing Area Remain Active",
    fileName = "Spell Behaviour Apply Wisps Healing Area Remain Active")]
public class SpellBehaviourApplyWispsHealingAreaRemainActiveSO : SpellBehaviourAbstractOneShotSO
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
            Collider[] whoCastHit = Physics.OverlapSphere(parent.transform.position, 
                parent.Spell.AreaOfEffect, parent.LayerOfWhoCast);

            if (whoCastHit.Length > 0)
            {
                parent.WhoCast.Heal(
                    parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type), StatsType.Armor);
                parent.LastTimeDamaged = Time.time;
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
