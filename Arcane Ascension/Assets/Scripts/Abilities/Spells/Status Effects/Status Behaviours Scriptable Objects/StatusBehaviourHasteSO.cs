using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating haste status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Haste",
    fileName = "Status Behaviour Haste")]
[InlineEditor]
public class StatusBehaviourHasteSO : StatusBehaviourAbstractSO
{
    [Range(1f, 4f)][SerializeField] private float speedMultiplier;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        // If the character is NOT suffering from the effect yet
        if (parent.WhoCast.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
                parent.WhoCast.UpdateSpeed();

                parent.WhoCast.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.PrefabVFX = playerVFX;
                parent.EffectActive = true;
            }
            // If it's already taking effect
            else
            {
                parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

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
        // This will happen to the active effect
        // In order for this to happen, the effect is active, so the stats Dictionary will
        // have this key for sure
        if (Time.time - parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
        {
            parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = 1f;
            parent.WhoCast.UpdateSpeed();
            parent.WhoCast.StatusEffectList.RemoveItem(statusEffectType);
            parent.DisableStatusGameObject();
        }
    }
}
