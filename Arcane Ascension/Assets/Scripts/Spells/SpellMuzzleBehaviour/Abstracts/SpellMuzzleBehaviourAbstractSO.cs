using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a spell muzzle behaviour.
/// </summary>
[InlineEditor]
public abstract class SpellMuzzleBehaviourAbstractSO : ScriptableObject
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public virtual void StartBehaviour(SpellMuzzleBehaviourOneShot parent) { }

    /// <summary>
    /// Executes on update.
    /// </summary>
    public virtual void ContinuousUpdateBehaviour(SpellMuzzleBehaviourOneShot parent) { }

    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public virtual void StartBehaviour(SpellMuzzleBehaviourContinuous parent) { }

    /// <summary>
    /// Executes on update.
    /// </summary>
    public virtual void ContinuousUpdateBehaviour(SpellMuzzleBehaviourContinuous parent) { }
}
