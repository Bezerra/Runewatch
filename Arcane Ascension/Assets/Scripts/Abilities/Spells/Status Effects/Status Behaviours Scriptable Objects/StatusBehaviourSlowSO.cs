using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating slow status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Slow",
    fileName = "Status Behaviour Slow")]
[InlineEditor]
public class StatusBehaviourSlowSO : StatusBehaviourAbstractSO
{
    [Header("Movement speed is multiplied by this value")]
    [Range(0.1f, 1f)] [SerializeField] private float speedMultiplier = 1f;

    [Header("-1 is enemy default attacking speed. Lower values mean less attack speed")]
    [Range(-4f, -1f)] [SerializeField] private float attackingSpeedMultiplier = -1f;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        // If the character is NOT suffering from the effect yet
        if (parent.CharacterHit.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.CharacterHit.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
                parent.CharacterHit.UpdateSpeed();

                // If character is an enemy
                if (parent.CharacterHit.CommonAttributes.Type != CharacterType.Player)
                {
                    (parent.CharacterHit as EnemyStats).
                        EnemyAttributes.AttackingSpeedReductionMultiplier = -(attackingSpeedMultiplier);
                }

                parent.CharacterHit.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.PrefabVFX = playerVFX;
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
        // This will happen to the active effect
        // In order for this to happen, the effect is active, so the stats Dictionary will
        // have this key for sure
        if (Time.time - parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
        {
            parent.CharacterHit.CommonAttributes.MovementStatusEffectMultiplier = 1f;
            parent.CharacterHit.UpdateSpeed();

            // If character is an enemy
            if (parent.CharacterHit.CommonAttributes.Type != CharacterType.Player)
            {
                (parent.CharacterHit as EnemyStats).
                    EnemyAttributes.AttackingSpeedReductionMultiplier = attackingSpeedMultiplier;
            }

            parent.CharacterHit.StatusEffectList.RemoveItem(statusEffectType);
            parent.DisableStatusGameObject();
        }
    }
}
