using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling chest behaviours.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;

    // Components
    private GameObject canvasText;
    private InteractionCanvasText interectableCanvas;
    private AbstractEventOnInteraction eventOnInteraction;

    private bool canOpen;
    public bool CanOpen
    {
        get => canOpen;
        set
        {
            canOpen = value;
            if (canOpen) gameObject.layer = Layers.InterectableLayerNum;
            else gameObject.layer = Layers.DefaultNum;
        }
    }

    private void Awake()
    {
        canvasText = GetComponentInChildren<Canvas>().gameObject;
        interectableCanvas = GetComponent<InteractionCanvasText>();
        eventOnInteraction = GetComponent<AbstractEventOnInteraction>();
    }

    private void Start()
    {
        CanOpen = false;
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
