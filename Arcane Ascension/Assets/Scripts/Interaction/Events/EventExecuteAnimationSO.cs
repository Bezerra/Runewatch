using UnityEngine;

/// <summary>
/// Scritable object responsible for opening one spells canvas.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Execute Animation",
    fileName = "Event Execute Animation")]
public class EventExecuteAnimationSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        invoker.GetComponent<Animator>().SetTrigger("Execute");
    }
}
