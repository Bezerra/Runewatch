using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for handling chest behaviours.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;
    [SerializeField] private InputActionReference interactionActionAsset;

    // Components
    private GameObject canvasText;
    private InteractionCanvasText interectableCanvas;
    private AbstractEventOnInteraction eventOnInteraction;
    private RebindingManager rebindingManager;

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
        rebindingManager = FindObjectOfType<RebindingManager>();
    }

    private void OnEnable()
    {
        rebindingManager.KeybindingsUpdated += KeybindingsUpdated;
    }

    private void OnDisable()
    {
        rebindingManager.KeybindingsUpdated -= KeybindingsUpdated;
    }

    private void Start()
    {
        CanOpen = true;
    }

    private void KeybindingsUpdated()
    {
        interectableCanvas.UpdateInformation(
            InputControlPath.ToHumanReadableString(
            interactionActionAsset.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice));
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
