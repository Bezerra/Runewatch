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
            if (canOpen)
            {
                GetComponentInChildren<BoxCollider>().gameObject.layer = Layers.InterectableLayerNum;
                eventOnInteraction.enabled = true;
                interectableCanvas.enabled = true;
                canvasText.SetActive(true);

            }
            else
            {
                GetComponentInChildren<BoxCollider>().gameObject.layer = Layers.DefaultNum;
                eventOnInteraction.enabled = false;
                interectableCanvas.enabled = false;
                canvasText.SetActive(false);
            }
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
        CanOpen = true;
    }

    public void ChestOpeningStartAnimationEvent()
    {
        LootSoundPoolCreator.Pool.InstantiateFromPool(
            LootAndInteractionSoundType.InteractChest.ToString(), transform.position, Quaternion.identity);

        GetComponentInChildren<BoxCollider>().gameObject.layer = Layers.DefaultNum;
        eventOnInteraction.enabled = false;
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
