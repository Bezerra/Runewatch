using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a status behaviour.
/// </summary>
[InlineEditor]
public abstract class StatusBehaviourAbstractSO : ScriptableObject
{
    [Range(0, 400f)] [SerializeField] protected float durationSeconds;

    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public virtual void StartBehaviour(StatusBehaviour parent)
    {
        parent.StatusDuration = durationSeconds;
    }

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(StatusBehaviour parent);
}
