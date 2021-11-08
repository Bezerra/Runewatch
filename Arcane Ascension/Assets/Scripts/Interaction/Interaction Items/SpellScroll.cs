using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for informing this gameobject is a spell.
/// </summary>
public class SpellScroll : MonoBehaviour, IDroppedSpell, IInterectableWithSound
{
    [Range(10, 60)][SerializeField] private float timeToDeactivate;

    [Header("Sound to be played when interected with")]
    [SerializeField] private LootAndInteractionSoundType lootAndInteractionSoundType;
    public LootAndInteractionSoundType LootAndInteractionSoundType => lootAndInteractionSoundType;

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(timeToDeactivate);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    public ISpell DroppedSpell { get; set; }
}
