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
    [Range(0, 400f)] [SerializeField] private float durationSeconds;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        if (parent.WhoCast != null)
        {
            parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = speedMultiplier;
            parent.WhoCast.UpdateSpeed();
        }
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (Time.time - parent.TimeSpawned > durationSeconds)
        {
            parent.WhoCast.CommonAttributes.MovementStatusEffectMultiplier = 1f;
            parent.WhoCast.UpdateSpeed();
            parent.DisableStatusGameObject();
        }
    }
}
