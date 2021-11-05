using UnityEngine;

/// <summary>
/// Class responsible for informing this gameobject is a spell.
/// </summary>
public class SpellScroll : MonoBehaviour, IDroppedSpell
{
    [Range(10, 60)][SerializeField] private float timeToDestroy;

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }

    public ISpell DroppedSpell { get; set; }
}
