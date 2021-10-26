using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Abstract scriptable object parent of event scriptable objects.
/// </summary>
[InlineEditor]
public abstract class EventAbstractSO : ScriptableObject
{
    /// <summary>
    /// Executes an event.
    /// </summary>
    public abstract void Execute();
}
