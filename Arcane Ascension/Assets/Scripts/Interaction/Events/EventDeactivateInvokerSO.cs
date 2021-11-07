using UnityEngine;

/// <summary>
/// Scritable object responsible for deactivating the interection gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Deactivate Invoker",
    fileName = "Event Deactivate Invoker")]
public class EventDeactivateInvokerSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        invoker.gameObject.SetActive(false);
    }
}
