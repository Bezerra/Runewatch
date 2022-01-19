using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating haste status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Fortify",
    fileName = "Status Behaviour Fortify")]
[InlineEditor]
public class StatusBehaviourFortifySO : StatusBehaviourAbstractSO
{
    [Header("-1 = 100% plus resistance, 0 = 0% plus resistance")]
    [Range(-1f, 0f)] [SerializeField] private float damageToTakeMultiplier = 0;

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
                parent.WhoCast.CommonAttributes.DamageResistanceStatusEffectMultiplier = 
                    damageToTakeMultiplier;

                parent.WhoCast.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.EffectType = statusEffectType;
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

        // This will happen to the active effect
        if (parent.WhoCast.StatusEffectList.Items.ContainsKey(statusEffectType))
        {
            // This will happen to the active effect
            if (Time.time - parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied
                > durationSeconds)
            {
                parent.WhoCast.CommonAttributes.DamageResistanceStatusEffectMultiplier = 0f;
                parent.WhoCast.StatusEffectList.RemoveItem(statusEffectType);
                parent.DisableStatusGameObject();
            }
        }
        else
        {
            parent.CharacterHit.CommonAttributes.DamageResistanceStatusEffectMultiplier = 0f;
            parent.DisableStatusGameObject();
        }
    }
}
