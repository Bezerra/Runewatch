using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling chest behaviours.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;

    private GameObject canvasText;
    private InteractionCanvasText interectableCanvas;
    private AbstractEventOnInteraction eventOnInteraction;

    private void Awake()
    {
        canvasText = GetComponentInChildren<Canvas>().gameObject;
        interectableCanvas = GetComponent<InteractionCanvasText>();
        eventOnInteraction = GetComponent<AbstractEventOnInteraction>();
    }

    public void ChestOpeningStartAnimationEvent()
    {
        LootSoundPoolCreator.Pool.InstantiateFromPool(
            LootAndInteractionSoundType.InteractChest.ToString(), transform.position, Quaternion.identity);

        interectableCanvas.enabled = false;
        canvasText.SetActive(false);
    }

    public void ChestOpenedEndAnimationEvent()
    {
        foreach(EventAbstractSO eve in eventsToRunOnChestOpen)
        {
            eve.Execute(eventOnInteraction);
        }
    }
}
