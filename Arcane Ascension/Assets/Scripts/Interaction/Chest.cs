using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handling chest behaviours.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;

    private GameObject canvasText;
    private InterectableCanvasText interectableCanvas;
    private EventOnInteraction eventOnInteraction;

    private void Awake()
    {
        canvasText = GetComponentInChildren<Canvas>().gameObject;
        interectableCanvas = GetComponent<InterectableCanvasText>();
        eventOnInteraction = GetComponent<EventOnInteraction>();
    }

    public void ChestOpeningStartAnimationEvent()
    {
        interectableCanvas.enabled = false;
        canvasText.SetActive(false);
        eventOnInteraction.enabled = false;
    }

    public void ChestOpenedEndAnimationEvent()
    {
        foreach(EventAbstractSO eve in eventsToRunOnChestOpen)
        {
            eve.Execute(eventOnInteraction);
        }
    }
}
