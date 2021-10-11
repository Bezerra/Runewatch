using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Forward", fileName = "Spell Behaviour Forward")]
sealed public class SpellBehaviourForwardSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private DamageSingleTargetSO damageSingleTargetBehaviour;
    [SerializeField] private DamageOvertimeSO damageOvertimeBehaviour;
    [SerializeField] private DamageAoESO damageAoeBehaviour;
    [SerializeField] private DamageAoEOvertimeSO damageAoeOvertimeBehaviour;

    [Header("In this spell, this variable only checks the direction of the spell")]
    [Range(1, 50)] [SerializeField] private float spellDistance;

    [Header("This variables is used to disable the spell after X seconds")]
    [Range(1, 10)] [SerializeField] private float disableAfterSeconds;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Direction of the spell
        Ray forward = new Ray(parent.Eyes.position, parent.Eyes.forward);

        if (Physics.Raycast(forward, out RaycastHit objectHit, spellDistance)) // Creates a raycast to see if eyes are hiting something
        {
            parent.transform.LookAt(objectHit.point);
        }
        else
        {
            Vector3 finalDirection = parent.Eyes.position + parent.Eyes.forward * 15f;
            parent.transform.LookAt(finalDirection);
        }

        // Moves the spell forward
        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;

        // Starts cooldown of the spell
        if (parent.WhoCast.TryGetComponent<PlayerSpells>(out PlayerSpells playerSpells))
            playerSpells.StartSpellCooldown(playerSpells.ActiveSpell);

        // Takes mana from player
        parent.WhoCast.ReduceMana(parent.Spell.ManaCost);
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // If the spell is a area spell is an AoE and doesn't disappear immediatly (ex. cloud of poison)
        // it will only disable it after x seconds
        if (parent.ApplyingDamageOvertime == false)
        {
            if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                DisableSpell(parent);
        }
        else
        {
            // Disables spell after it reached max time
            if (Time.time - parent.TimeSpawned > parent.Spell.AreaSpellMaxTime)
                DisableSpell(parent);
        }

        // If the spell hits something
        if (parent.Rb.velocity == Vector3.zero)
        {
            if (Time.time - parent.TimeOfImpact > 1)
                DisableSpell(parent);
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        switch (parent.Spell.DamageType)
        {
            case SpellDamageType.SingleTarget:
                damageSingleTargetBehaviour.Damage(other, parent);
                break;

            case SpellDamageType.Overtime:
                damageOvertimeBehaviour.Damage(other, parent);
                break;

            case SpellDamageType.AreaDamage:
                if (parent.Spell.AppliesDamageOvertime == false)
                {
                    damageAoeBehaviour.Damage(other, parent);
                }
                else
                {
                    damageAoeOvertimeBehaviour.Damage(other, parent);
                }

                break;
        }

        parent.Rb.velocity = Vector3.zero;
    }
}
