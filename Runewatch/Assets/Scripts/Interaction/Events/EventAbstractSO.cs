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
    /// <param name="invoker">Invoker gameobject.</param>
    /// <param name="interactor">Who interacted with the object that triggered the event.</param>
    public abstract void Execute(AbstractEventOnInteraction invoker = null,
        PlayerInteraction interactor = null);
}
