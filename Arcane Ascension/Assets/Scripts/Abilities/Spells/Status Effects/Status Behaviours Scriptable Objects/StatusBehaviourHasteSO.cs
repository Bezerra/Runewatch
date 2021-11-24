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
        base.StartBehaviour(parent);

        if (parent.CurrentlyActive == false)
        {
            if (parent.WhoCast != null)
            {
                if (parent.WhoCast.StatusEffectList.ContainsKey(StatusEffectType.Haste) == false)
                {
                    parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
                    parent.WhoCast.UpdateSpeed();

                    parent.WhoCast.StatusEffectList.Add(
                        StatusEffectType.Haste, 
                        new StatusEffectInformation(Time.time, durationSeconds));

                    parent.CurrentlyActive = true;
                }
                else
                {
                    parent.WhoCast.StatusEffectList[StatusEffectType.Haste].TimeApplied = Time.time;
                    parent.DisableStatusGameObject();
                }
            }
            else
            {
                parent.DisableStatusGameObject();
            }
        }
        else
        {
            if (parent.WhoCast != null)
            {
                if (parent.WhoCast.StatusEffectList.ContainsKey(StatusEffectType.Haste))
                {
                    parent.WhoCast.StatusEffectList[StatusEffectType.Haste].TimeApplied = Time.time;
                }
            }
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (parent.CurrentlyActive)
        {
            if (Time.time - parent.WhoCast.StatusEffectList[StatusEffectType.Haste].TimeApplied >
                durationSeconds)
            {
                parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = 1f;
                parent.WhoCast.UpdateSpeed();
                parent.WhoCast.StatusEffectList.Remove(StatusEffectType.Haste);
                parent.DisableStatusGameObject();
            }
        }
    }
}
