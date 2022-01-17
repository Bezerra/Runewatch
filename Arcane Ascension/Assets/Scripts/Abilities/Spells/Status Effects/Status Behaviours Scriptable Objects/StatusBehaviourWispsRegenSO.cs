using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating wisps healing status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Wisps Healing",
    fileName = "Status Behaviour Wisps Healing")]
[InlineEditor]
public class StatusBehaviourWispsRegenSO : StatusBehaviourAbstractSO
{
    [Range(0, 10f)][SerializeField] private float healingToDo;
    [Range(0, 2f)] [SerializeField] private float healingInterval;

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
        if (parent.WhoCast == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        if (Time.time - parent.LastTimeHit > healingInterval)
        {
            parent.LastTimeHit = Time.time;
            parent.WhoCast.Heal(healingToDo, StatsType.Armor);
        }

        // This will happen to the active effect
        // In order for this to happen, the effect is active, so the stats Dictionary will
        // have this key for sure
        if (Time.time - parent.WhoCast.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
        {
            parent.WhoCast.StatusEffectList.RemoveItem(statusEffectType);
            parent.DisableStatusGameObject();
        } 
    }
}
