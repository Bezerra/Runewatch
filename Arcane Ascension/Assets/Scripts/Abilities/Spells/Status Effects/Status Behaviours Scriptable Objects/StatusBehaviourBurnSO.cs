using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating burn status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Burn",
    fileName = "Status Behaviour Burn")]
[InlineEditor]
public class StatusBehaviourBurnSO : StatusBehaviourAbstractSO
{
    [Range(0, 4f)][SerializeField] private float damageToDo;
    [Range(0, 2f)] [SerializeField] private float damageInterval;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        // If the character is NOT suffering from the effect yet
        if (parent.WhoCast.StatusEffectList.ContainsKey(StatusEffectType.Burn) == false)
        {
            // If the parent of this effect is not taking place yet
            if (parent.EffectActive == false)
            {
                parent.WhoCast.StatusEffectList.Add(StatusEffectType.Burn,
                    new StatusEffectInformation(Time.time, durationSeconds));

                parent.CharacterHit.TakeDamageOvertime(
                    damageToDo, ElementType.Fire, damageInterval, durationSeconds);

                parent.EffectActive = true;
            }
            // If it's already taking effect
            else
            {
                parent.WhoCast.StatusEffectList[StatusEffectType.Burn].TimeApplied = Time.time;

                parent.CharacterHit.TakeDamageOvertime(
                    damageToDo, ElementType.Fire, damageInterval, durationSeconds);

                parent.DisableStatusGameObject();
            }
        }
        // Else if the character is already suffering from the effect
        else
        {
            parent.WhoCast.StatusEffectList[StatusEffectType.Burn].TimeApplied = Time.time;

            parent.CharacterHit.TakeDamageOvertime(
                damageToDo, ElementType.Fire, damageInterval, durationSeconds);

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
        if (Time.time - parent.WhoCast.StatusEffectList[StatusEffectType.Burn].TimeApplied
            > durationSeconds)
        {
            parent.WhoCast.StatusEffectList.Remove(StatusEffectType.Burn);
            parent.DisableStatusGameObject();
        }
    }
}
