using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a status behaviour.
/// </summary>
[InlineEditor]
public abstract class StatusBehaviourAbstractSO : ScriptableObject
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(StatusBehaviour parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(StatusBehaviour parent);
}
