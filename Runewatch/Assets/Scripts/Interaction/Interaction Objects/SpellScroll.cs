using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for informing this gameobject is a spell.
/// </summary>
public class SpellScroll : MonoBehaviour, IDroppedSpell, IInteractableWithSound, IReset
{
    //[Range(10, 60)][SerializeField] private float timeToDeactivate;

    // Components
    [SerializeField] private GameObject canvasText;
    [SerializeField] private InteractionCanvasText interectableCanvas;
    [SerializeField] private AbstractEventOnInteraction eventOnInteraction;
    [SerializeField] private Animator anim;

    [SerializeField] private List<EventAbstractSO> eventsToRunOnChestOpen;

    [Header("Sound to be played when interected with")]
    [SerializeField] private LootAndInteractionSoundType lootAndInteractionSoundType;
    public LootAndInteractionSoundType LootAndInteractionSoundType => lootAndInteractionSoundType;

    /*private IEnumerator Disable()
    {
        yield return new WaitForSeconds(timeToDeactivate);
        gameObject.SetActive(false);
    }*/

    private void OnEnable()
    {
        //StartCoroutine(Disable());
        interectableCanvas.enabled = true;
        canvasText.SetActive(true);
        anim.ResetTrigger("Reset");
    }

    private void OnDisable()
    {
        ResetAfterPoolDisable();
    }

    public ISpell DroppedSpell { get; set; }

    /// <summary>
    /// On opening animation event.
    /// </summary>
    public void BookOpeningStartAnimationEvent()
    {
        interectableCanvas.enabled = false;
        canvasText.SetActive(false);
    }

    /// <summary>
    /// After the opening animation is over.
    /// </summary>
    public void BookOpenedEndAnimationEvent()
    {
        foreach (EventAbstractSO eve in eventsToRunOnChestOpen)
        {
            eve.Execute(eventOnInteraction);
        }
    }

    public void ResetAfterPoolDisable()
    {
        anim.SetTrigger("Reset");
        interectableCanvas.enabled = true;
        canvasText.SetActive(true);
    }
}
