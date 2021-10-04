using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Forward", fileName = "Spell Behaviour Forward")]
sealed public class SpellBehaviourForwardSO : SpellBehaviourAbstractOneShotSO
{
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

        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;

        // Starts cooldown
        if (parent.WhoCast.TryGetComponent<PlayerSpells>(out PlayerSpells playerSpells))
            playerSpells.StartSpellCooldown(playerSpells.ActiveSpell);

        // Takes mana from player
        parent.WhoCast.ReduceMana(parent.Spell.ManaCost);
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.ApplyingDamageOvertime == false)
        {
            if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                parent.gameObject.SetActive(false);
        }
        else
        {
            // Disables spell after it reached max time
            if (Time.time - parent.TimeSpawned > parent.Spell.AreaSpellMaxTime)
                parent.gameObject.SetActive(false);
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        IDamageable character;

        float damageMultiplier = parent.WhoCast.Attributes.BaseDamage / 100f;

        switch (parent.Spell.DamageType)
        {
            case SpellDamageType.SingleTarget:
                if (other.gameObject.TryGetComponentInParent<IDamageable>(out character))
                    character.TakeDamage(damageMultiplier * parent.Spell.Damage, parent.Spell.Element);

                DisableSpell(parent);
                break;

            case SpellDamageType.Overtime:
                if (other.gameObject.TryGetComponentInParent<IDamageable>(out character))
                    character.TakeDamageOvertime(
                        damageMultiplier * parent.Spell.Damage, 
                        parent.Spell.Element, 
                        parent.Spell.TimeInterval, 
                        parent.Spell.MaxTime);

                DisableSpell(parent);
                break;

            case SpellDamageType.AreaDamage:
                if (parent.Spell.AppliesDamageOvertime == false)
                {
                    Collider[] collisions = Physics.OverlapSphere(
                    other.ClosestPoint(parent.transform.position), parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

                    // Creates a new list with IDamageable characters
                    IList<IDamageable> charactersToDoDamage = new List<IDamageable>();

                    // Adds all IDamageable characters found to a list
                    for (int i = 0; i < collisions.Length; i++)
                    {
                        // Creates a ray from spell to hit
                        Ray dir = new Ray(
                                    parent.transform.position,
                                    (collisions[i].transform.position - parent.transform.position).normalized);

                        if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f, Layers.EnemyWithWalls))
                        {
                            // If the collider is an IDamageable (meaning there wasn't a wall in the ray path)
                            if (characterHit.collider.TryGetComponentInParent<IDamageable>(out character) &&
                                characterHit.collider.TryGetComponentInParent<Stats>(out Stats stats))
                            {
                                // If the target is different than who cast the spell
                                if (stats != parent.WhoCast)
                                {
                                    if (charactersToDoDamage.Contains(character) == false)
                                    {
                                        charactersToDoDamage.Add(character);
                                    }
                                }
                            }
                        }
                    }

                    // Damages all IDamageable characters depending on the number of characters hit
                    if (charactersToDoDamage.Count > 0)
                    {
                        for (int i = 0; i < charactersToDoDamage.Count; i++)
                        {
                            charactersToDoDamage[i].TakeDamage(
                                ((damageMultiplier * parent.Spell.Damage) / charactersToDoDamage.Count),
                                parent.Spell.Element);
                        }
                    }

                    DisableSpell(parent);
                }
                else
                {
                    Collider[] collisions = Physics.OverlapSphere(
                    other.ClosestPoint(parent.transform.position), parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

                    // Adds all IDamageable characters found to a list
                    for (int i = 0; i < collisions.Length; i++)
                    {
                        // Creates a ray from spell to hit
                        Ray dir = new Ray(
                                    parent.transform.position,
                                    (collisions[i].transform.position - parent.transform.position).normalized);

                        if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f, Layers.EnemyWithWalls))
                        {
                            // If the collider is an IDamageable (meaning there wasn't a wall in the ray path)
                            if (characterHit.collider.TryGetComponentInParent<IDamageable>(out character) &&
                                characterHit.collider.TryGetComponentInParent<Stats>(out Stats stats))
                            {
                                // If the target is different than who cast the spell
                                if (stats != parent.WhoCast)
                                {
                                    Debug.Log("AH2");
                                    character.TakeDamageOvertime(
                                        damageMultiplier * parent.Spell.Damage,
                                        parent.Spell.Element,
                                        parent.Spell.TimeInterval,
                                        parent.Spell.MaxTime);
                                }
                            }
                        }      
                    }
                    
                    parent.ApplyingDamageOvertime = true;
                }
                

                break;
        }

        
    }
}
