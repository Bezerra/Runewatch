using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating corrupt status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Corrupt",
    fileName = "Status Behaviour Corrupt")]
[InlineEditor]
public class StatusBehaviourCorruptSO : StatusBehaviourAbstractSO
{
    [Range(0, 4f)][SerializeField] private float damageToDo;
    [Range(0, 4f)] [SerializeField] private float stackMultiplier;
    [Range(0, 2f)] [SerializeField] private float damageInterval;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        if (parent.CharacterHit == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        // If the character is NOT suffering from the effect yet
        if (parent.CharacterHit.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.CharacterHit.StatusEffectList.AddItem(statusEffectType,
                    new StatusEffectInformation(Time.time, durationSeconds, icon));

                parent.CharacterHit.TakeDamageOvertime(
                    damageToDo, ElementType.Umbra, damageInterval, durationSeconds);

                parent.EffectType = statusEffectType;
                parent.EffectActive = true;
            }
            // If it's already taking effect
            else
            {
                parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

                parent.CharacterHit.TakeDamageOvertime(
                    damageToDo, ElementType.Umbra, damageInterval, durationSeconds);

                parent.DisableStatusGameObject();
            }
        }
        // Else if the character is already suffering from the effect
        else
        {
            parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

            parent.CharacterHit.StatusEffectStack++;
            parent.CharacterHit.TakeDamageOvertime(
                damageToDo * (stackMultiplier * parent.CharacterHit.StatusEffectStack),
                ElementType.Umbra, damageInterval, durationSeconds);

            // Will only disable the spell if it's not the one that's causing the current effect
            // Do not remove this
            if (parent.EffectActive == false)
                parent.DisableStatusGameObject();
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (parent.CharacterHit == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        // This will happen to the active effect
        // In order for this to happen, the effect is active, so the stats Dictionary will
        // have this key for sure
        if (Time.time - parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied
            > durationSeconds)
        {
            parent.CharacterHit.StatusEffectList.RemoveItem(statusEffectType);
            parent.CharacterHit.StatusEffectStack = 0;
            parent.DisableStatusGameObject();
        }
    }
}
