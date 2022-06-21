using UnityEngine;

/// <summary>
/// Scritable object responsible for playing invoker ability interaction sound.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Play Invoker Interaction Sound",
    fileName = "Event Play Invoker Interaction Sound")]
public class EventPlayInvokerInteractionSoundSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker, PlayerInteraction interactor = null)
    {
        if (invoker.TryGetComponent<IInteractableWithSound>(out IInteractableWithSound abilityInterectable))
        {
            LootSoundPoolCreator.Pool.InstantiateFromPool(
                abilityInterectable.LootAndInteractionSoundType.ToString(), 
                invoker.transform.position, Quaternion.identity);
        }
    }
}
