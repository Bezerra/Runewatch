using UnityEngine;

/// <summary>
/// Scritable object responsible for destroying the interection gameobject..
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Destroy Invoker",
    fileName = "Event Destroy Invoker")]
public class EventDestroyInvokerSO : EventAbstractSO
{
    public override void Execute(EventOnInteraction invoker)
    {
        invoker.DestroyInvoker();
    }
}
