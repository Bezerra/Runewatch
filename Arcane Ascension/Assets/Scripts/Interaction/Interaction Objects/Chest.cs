using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for handling chest behaviours.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;
    

    [Header("Is this chest unlocked by default")]
    [SerializeField] private bool chestUnlocked;

    // Components
    [SerializeField] private GameObject canvasText;
    [SerializeField] private InteractionCanvasText interectableCanvas;
    [SerializeField] private AbstractEventOnInteraction eventOnInteraction;

    private bool canOpen;
    public bool CanOpen
    {
        get => canOpen;
        set
        {
            canOpen = value;
            if (canOpen)
            {
                BoxCollider childBoxCol = GetComponentInChildren<BoxCollider>();
                if (childBoxCol != null)
                {
                    childBoxCol.gameObject.layer = Layers.InterectableLayerNum;
                }

                eventOnInteraction.enabled = true;
                interectableCanvas.enabled = true;
                canvasText.SetActive(true);
            }
            else
            {
                BoxCollider childBoxCol = GetComponentInChildren<BoxCollider>();
                if (childBoxCol != null)
                {
                    try
                    {
                        childBoxCol.gameObject.layer = Layers.DefaultNum;
                    }
                    // In case it's null (Happens on proc.gen.demo scene)
                    catch
                    {
                        childBoxCol.gameObject.layer = 0;
                    }
                }

                eventOnInteraction.enabled = false;
                interectableCanvas.enabled = false;
                canvasText.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (chestUnlocked)
        {
            CanOpen = true;
        }
        else
        {
            CanOpen = false;
        }

        StartCoroutine(ResetChest());
    }

    private IEnumerator ResetChest()
    {
        Animator anim = GetComponent<Animator>();
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Reset");
        yield return new WaitForSeconds(1);
        anim.ResetTrigger("Reset");
        anim.ResetTrigger("Execute");
    }

    private void Update()
    {
        if (interectableCanvas.CurrentlyActive)
        {
            interectableCanvas.UpdateInformation();
        }
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
