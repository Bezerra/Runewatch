using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scritable object responsible for setting to true animation bool.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Execute Animation Bool",
    fileName = "Event Execute Animation Bool")]
public class EventExecuteAnimationBoolSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        if (invoker.gameObject.TryGetComponentInChildrenFirstGen(out IInteractableWithAnimation anim))
        {
            anim.ExecuteAnimation = true;
        }
    }
}
