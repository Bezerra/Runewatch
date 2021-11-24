using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a status behaviour.
/// </summary>
[InlineEditor]
public abstract class StatusBehaviourAbstractSO : ScriptableObject
{
    // Status information
    [SerializeField] protected StatusEffectType statusEffectType;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected GameObject prefabVFX;
    [Range(0, 400f)] [SerializeField] protected float durationSeconds;

    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(StatusBehaviour parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(StatusBehaviour parent);
}
