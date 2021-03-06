using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating sacrifice status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Sacrifice",
    fileName = "Status Behaviour Sacrifice")]
[InlineEditor]
public class StatusBehaviourSacrificeSO : StatusBehaviourAbstractSO
{
    //[Header("Damage to do on caster will never be less than 1.")]
    [Tooltip("Only used if it's " +
        "the player casting this status.")]
    [Range(0, 10f)] [SerializeField] private float damageOnPlayerCaster = 1f;

    [Tooltip("This value is the base damage that will be done in the monster " +
        "without calculating damage reductions.")]
    [Range(0, 100f)] [SerializeField] private float damageOnMonsterCaster = 25f;

    [Range(0, 50f)][SerializeField] private float damageToDo;
    [Range(0, 2f)] [SerializeField] private float damageInterval;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        if (parent.WhoCast == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        // If the character is NOT suffering from the effect yet
        if (parent.WhoCast.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.WhoCast.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.EffectType = statusEffectType;
                parent.LastTimeHit = 0;
                parent.EffectActive = true;
            }
            // If it's already taking effect
            else
            {
                parent.DisableStatusGameObject();
            }
        }
        // Else if the character is already suffering from the effect
        else
        {
            parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

            // Will only disable the spell if it's not the one that's causing the current effect
            if (parent.EffectActive == false)
                parent.DisableStatusGameObject();
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (parent.WhoCast == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        if (Time.time - parent.LastTimeHit > damageInterval)
        {
            // Checks which layer should it damage
            LayerMask layerToHit = parent.WhoCast.CommonAttributes.Type ==
                CharacterType.Player ? Layers.EnemyLayer : Layers.PlayerLayer;

            // Checks if a for is in range
            Collider[] characterInRange = Physics.OverlapSphere(parent.transform.position,
                parent.Spell.AreaOfEffect, layerToHit);

            // Creates a new list with IDamageable characters
            // Must be Stats instead of IDamageable to apply status behaviour
            IList<Stats> charactersToDoDamage = new List<Stats>();

            for (int i = 0; i < characterInRange.Length; i++)
            {
                // If the collider is an IDamageable
                if (characterInRange[i].TryGetComponentInParent<IDamageable>(out IDamageable character))
                {
                    // If the target is different than who cast the spell
                    if (!character.Equals(parent.ParentSpell.ThisIDamageable))
                    {
                        if (charactersToDoDamage.Contains(character as Stats) == false)
                        {
                            charactersToDoDamage.Add(character as Stats);
                        }
                    }
                }
            }

            // Damages all foes found previously
            if (charactersToDoDamage.Count > 0)
            {
                for (int i = 0; i < charactersToDoDamage.Count; i++)
                {
                    charactersToDoDamage[i].TakeDamage(
                        damageToDo,
                        parent.Spell.Element,
                        parent.transform.position);
                }
            }

            // Reduces spell caster health
            if (parent.WhoCast != null)
            {
                if (parent.WhoCast.CommonAttributes.Type == CharacterType.Player)
                {
                    float damageToDoOnCaster = damageOnPlayerCaster;
                    if (damageToDoOnCaster < 1) damageToDoOnCaster = 1.1f;

                    parent.WhoCast.TakeDamage(
                        damageToDoOnCaster,
                        parent.Spell.Element,
                        Vector3.zero);
                }
                else if (parent.WhoCast.CommonAttributes.Type == CharacterType.Monster)
                {
                    parent.WhoCast.TakeDamage(
                        damageOnMonsterCaster * 2,
                        ElementType.Neutral,
                        Vector3.zero);
                }
                // Bosses don't take damage
            }

            parent.LastTimeHit = Time.time;
        }

        if (parent.WhoCast == null)
            return;

        // This will happen to the active effect
        if (parent.WhoCast.StatusEffectList.Items.ContainsKey(statusEffectType))
        {
            if (Time.time - parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
            {
                parent.WhoCast.StatusEffectList.RemoveItem(statusEffectType);
                parent.DisableStatusGameObject();
            }
        }
        else
        {
            parent.DisableStatusGameObject();
        }
    }
}
