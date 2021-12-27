using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating sacrifice status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Sacrifice",
    fileName = "Status Behaviour Sacrifice")]
[InlineEditor]
public class StatusBehaviourSacrificeSO : StatusBehaviourAbstractSO
{
    [Range(0, 10f)][SerializeField] private float damageToDo;
    [Range(0, 2f)] [SerializeField] private float damageInterval;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        // If the character is NOT suffering from the effect yet
        if (parent.CharacterHit.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.CharacterHit.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.PrefabVFX = prefabVFX;
                parent.EffectActive = true;
            }
            // If it's already taking effect
            else
            {
                parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

                parent.DisableStatusGameObject();
            }
        }
        // Else if the character is already suffering from the effect
        else
        {
            parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

            // Will only disable the spell if it's not the one that's causing the current effect
            if (parent.EffectActive == false)
                parent.DisableStatusGameObject();
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (Time.time - parent.LastTimeHit > damageInterval)
        {
            // Checks which layer should it damage
            LayerMask layerToHit = parent.WhoCast.gameObject.layer ==
                Layers.PlayerLayer ? Layers.EnemyLayer : Layers.PlayerLayer;

            // Checks if a for is in range
            Collider[] characterInRange = Physics.OverlapSphere(parent.transform.position,
                parent.Spell.AreaOfEffect, layerToHit);

            // If it finds foes in range, it triggers damage behaviour
            if (characterInRange.Length > 0)
            {
                for (int i = 0; i < characterInRange.Length; i++)
                {
                    if (characterInRange[i].TryGetComponent(out Stats stats))
                    {
                        stats.TakeDamage(
                            parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type),
                            parent.Spell.Element,
                            parent.transform.position);
                    }
                }
            }

            // Reduces spell caster health
            if (parent.WhoCast != null)
            {
                parent.WhoCast.TakeDamage(
                    parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type) * 0.33f,
                    parent.Spell.Element,
                    Vector3.zero);
            }
        }

        // This will happen to the active effect
        // In order for this to happen, the effect is active, so the stats Dictionary will
        // have this key for sure
        if (Time.time - parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
        {
            parent.CharacterHit.StatusEffectList.Items.Remove(statusEffectType);
            parent.DisableStatusGameObject();
        }
    }
}
