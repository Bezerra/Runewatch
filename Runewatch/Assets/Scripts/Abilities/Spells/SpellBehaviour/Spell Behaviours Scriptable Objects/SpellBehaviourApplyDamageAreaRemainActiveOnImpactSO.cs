using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying damage while the projectile is enabled.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Area " +
    "Remain After Time On Impact",
    fileName = "Spell Behaviour Apply Damage Area Remain After Time On Impact")]
public class SpellBehaviourApplyDamageAreaRemainActiveOnImpactSO : SpellBehaviourAbstractSO
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
        if (parent.Rb.velocity == Vector3.zero)
        {
            if (Time.time - parent.TimeOfImpact > parent.Spell.DelayToDoDamage)
            {
                if (Time.time - parent.LastTimeDamaged > parent.Spell.TimeInterval)
                {
                    parent.PositionOnHit = parent.transform.position;
                    parent.Spell.DamageBehaviour.Damage(parent);
                    parent.LastTimeDamaged = Time.time;
                }
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
