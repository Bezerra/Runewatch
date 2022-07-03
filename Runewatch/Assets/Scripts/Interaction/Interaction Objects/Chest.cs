using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject chestBoxCollider;
    [SerializeField] private GameObject chestIcon;
    [SerializeField] private int interectableLayerNumer;

    private ChestAnimationEvents chestAnimations;


    private bool canOpen;
    public bool CanOpen
    {
        get => canOpen;
        set
        {
            canOpen = value;
            if (canOpen)
            {
                // Can't use Layers static properties because it can be null at a point
                // of proc gen
                chestBoxCollider.layer = interectableLayerNumer;

                eventOnInteraction.enabled = true;
                interectableCanvas.enabled = true;
                canvasText.SetActive(true);
                chestAnimations.PlayBreakLockVFX();
            }
            else
            {
                chestBoxCollider.layer = 0;

                eventOnInteraction.enabled = false;
                interectableCanvas.enabled = false;
                canvasText.SetActive(false);
                chestAnimations.PlayLockedVFX();
            }
        }
    }

    private void Awake() =>
        chestAnimations = GetComponentInChildren<ChestAnimationEvents>(true);

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

        if (SceneManager.GetActiveScene().name == SceneEnum.ProceduralGeneration.ToString())
            StartCoroutine(ResetChest());
    }

    private IEnumerator ResetChest()
    {
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

    /// <summary>
    /// On opening animation event.
    /// </summary>
    public void ChestOpeningStartAnimationEvent()
    {
        LootSoundPoolCreator.Pool.InstantiateFromPool(
            LootAndInteractionSoundType.InteractChest.ToString(), transform.position, Quaternion.identity);

        GetComponentInChildren<BoxCollider>().gameObject.layer = Layers.DefaultNum;
        eventOnInteraction.enabled = false;
        interectableCanvas.enabled = false;
        canvasText.SetActive(false);
    }

    /// <summary>
    /// After the opening animation is over.
    /// </summary>
    public void ChestOpenedEndAnimationEvent()
    {
        foreach(EventAbstractSO eve in eventsToRunOnChestOpen)
        {
            eve.Execute(eventOnInteraction);
        }
        chestIcon.SetActive(false);
    }
}
