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
                if (parent.WhoCast.SpeedStatusEffectTime == 0)
                {
                    parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
                    parent.WhoCast.UpdateSpeed();
                    parent.WhoCast.SpeedStatusEffectTime = Time.time;
                    parent.CurrentlyActive = true;
                }
                else
                {
                    parent.WhoCast.SpeedStatusEffectTime = Time.time;
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
                parent.WhoCast.SpeedStatusEffectTime = Time.time;
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (parent.CurrentlyActive)
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
}
