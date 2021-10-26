using UnityEngine;

/// <summary>
/// Class used by gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class EventOnInteraction : MonoBehaviour, IInterectable
{
    [SerializeField] private EventAbstractSO eventOnInteraction;

    /// <summary>
    /// Executes an event.
    /// </summary>
    public void Execute()
    {
        if (eventOnInteraction != null)
            eventOnInteraction.Execute();
    }

    private void Start()
    {
        Execute();
    }
}
