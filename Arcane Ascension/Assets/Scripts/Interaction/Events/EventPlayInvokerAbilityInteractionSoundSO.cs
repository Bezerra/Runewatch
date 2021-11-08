using UnityEngine;

/// <summary>
/// Scritable object responsible for playing invoker ability interaction sound.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Play Invoker Ability Interaction Sound",
    fileName = "Event Play Invoker Ability Interaction Sound")]
public class EventPlayInvokerAbilityInteractionSoundSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        if (invoker.TryGetComponent<IAbilityInterectable>(out IAbilityInterectable abilityInterectable))
        {
            LootSoundPoolCreator.Pool.InstantiateFromPool(
                abilityInterectable.LootAndInteractionSoundType.ToString(), 
                invoker.transform.position, Quaternion.identity);
        }
    }
}
