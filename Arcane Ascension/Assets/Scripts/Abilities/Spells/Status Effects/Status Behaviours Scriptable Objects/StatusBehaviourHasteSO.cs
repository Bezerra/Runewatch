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

        if (parent.WhoCast != null)
        {
            if (parent.WhoCast.SpeedStatusEffectTime == 0)
            {
                parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
                parent.WhoCast.UpdateSpeed();
                parent.WhoCast.SpeedStatusEffectTime = Time.time;
            }
            else
            {
                parent.WhoCast.SpeedStatusEffectTime = Time.time;
                parent.DisableStatusGameObject();
            }
        }
        else
        {
            parent.WhoCast.SpeedStatusEffectTime = Time.time;
            parent.DisableStatusGameObject();
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (Time.time - parent.WhoCast.SpeedStatusEffectTime > durationSeconds)
        {
            parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = 1f;
            parent.WhoCast.UpdateSpeed();
            parent.WhoCast.SpeedStatusEffectTime = 0;
            parent.DisableStatusGameObject();
        }
    }
}
