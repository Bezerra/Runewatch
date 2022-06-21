using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying aoe damage.
/// This SO is used on AoE Hover spells with one shot release for boss patterns behaviours.
/// Position of the damage is set on SpellBehaviourSpawnHitOnAoEHoverSO.
/// Uses spell Speed as timer to deal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply " +
    "Damage AoE After Time With Release Boss Pattern",
    fileName = "Spell Behaviour Apply Damage AoE After Time With Release Boss Pattern")]
public class SpellBehaviourApplyDamageAoEAfterTimeWithReleaseBossPatternSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
        parent.TimeSpawned = Time.time;
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Position of the damage is set on SpellBehaviourSpawnHitOnAoEHoverSO.
        if (Time.time - parent.TimeSpawned > parent.Spell.DelayToDoDamage)
        {
            if (parent.AreaHoverDealtDamage == false)
            {
                for (int i = 0; i < parent.SpellsInPatternsTransforms.Length; i++)
                {
                    // Applies damage on all spawned spells in the pattern
                    parent.Spell.DamageBehaviour.Damage(
                        parent, 
                        overridePosition: parent.SpellsInPatternsTransforms[i].transform);
                }
                parent.AreaHoverDealtDamage = true;
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
