using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for informing this gameobject is a spell.
/// </summary>
public class SpellScroll : MonoBehaviour, IDroppedSpell
{
    [Range(10, 60)][SerializeField] private float timeToDeactivate;

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
