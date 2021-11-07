using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for interaction event invokers.
/// This class is used by objects that have interactions and trigger events.
/// </summary>
public abstract class AbstractEventOnInteraction : MonoBehaviour, IInterectable
{
    [TextArea]
    [SerializeField] protected string notes;

    // Events to trigger
    [SerializeField] protected List<EventAbstractSO> eventOnInteraction;

    // Player interaction class, to set last item interacted on execute
    protected PlayerInteraction playerInteraction;

    protected virtual void Awake() =>
        playerInteraction = FindObjectOfType<PlayerInteraction>();

    public abstract void Execute();
}
