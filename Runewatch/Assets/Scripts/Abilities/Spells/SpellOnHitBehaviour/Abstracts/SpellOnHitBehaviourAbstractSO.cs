using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a spell hit behaviour.
/// </summary>
[InlineEditor]
public abstract class SpellOnHitBehaviourAbstractSO : ScriptableObject
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(SpellOnHitBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellOnHitBehaviourOneShot parent);
}
