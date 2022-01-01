using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying aoe damage.
/// This SO is used on AoE Hover spells with one shot release.
/// Position of the damage is set on SpellBehaviourSpawnHitOnAoEHoverSO.
/// Uses spell Speed as timer to deal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage AoE After Time With Release",
    fileName = "Spell Behaviour Apply Damage AoE After Time With Release")]
public class SpellBehaviourApplyDamageAoEAfterTimeWithReleaseSO : SpellBehaviourAbstractSO
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
                parent.Spell.DamageBehaviour.Damage(parent);
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
