using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating vulnerable status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Vulnerable",
    fileName = "Status Behaviour Vulnerable")]
[InlineEditor]
public class StatusBehaviourVulnerableSO : StatusBehaviourAbstractSO
{
    [Header("0 = 0% plus damage, 1 = 100% plus damage")]
    [Range(0, 1f)] [SerializeField] private float damageToTakeMultiplier = 0;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        if (parent.CharacterHit == null)
        {
            parent.DisableStatusGameObject();
            return;
        }

        if (parent.CharacterHit.CommonAttributes.Type != CharacterType.Boss)
        {
            // If the character is NOT suffering from the effect yet
            if (parent.CharacterHit.StatusEffectList.Items.ContainsKey(statusEffectType) == false)
            {
                // If the parent of this effect is not taking effect yet
                if (parent.EffectActive == false)
                {
                    parent.CharacterHit.CommonAttributes.DamageResistanceStatusEffectMultiplier =
                        damageToTakeMultiplier;

                    parent.CharacterHit.StatusEffectList.AddItem(statusEffectType,
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
            // Else if the character is already suffering from the effect (meants it has dict key)
            else
            {
                parent.CharacterHit.StatusEffectList.Items[statusEffectType].TimeApplied = Time.time;

                // Will only disable the spell if it's not the one that's causing the current effect
                if (parent.EffectActive == false)
                    parent.DisableStatusGameObject();
            }
        }
        else
        {
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
        if (parent.CharacterHit.StatusEffectList.Items.ContainsKey(statusEffectType))
        {
            if (Time.time - parent.CharacterHit.StatusEffectList.Items[statusEffectType].
                TimeApplied> durationSeconds)
            {
                parent.CharacterHit.CommonAttributes.DamageResistanceStatusEffectMultiplier = 0f;
                parent.CharacterHit.StatusEffectList.RemoveItem(statusEffectType);
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
