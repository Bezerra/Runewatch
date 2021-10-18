using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's continuous apply damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Continuous Apply Damage",
    fileName = "Spell Behaviour Continuous Apply Damage")]
public class SpellBehaviourContinuousApplyDamageSO : SpellBehaviourAbstractContinuousSO
{
    [Range(0f, 5f)][SerializeField] private float damageTimeInterval;

    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        if (parent.DamageableTarget != null)
        {
            if (Time.time > parent.LastTimeHit + damageTimeInterval)
            {
                parent.Spell.DamageBehaviour.Damage(parent.DamageableTarget, parent);
                parent.LastTimeHit = Time.time;
            }
        }
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void HitBehaviour(Collider other, SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }
}
