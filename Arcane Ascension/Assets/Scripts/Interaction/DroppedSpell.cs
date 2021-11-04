using UnityEngine;

/// <summary>
/// Class responsible for informing this gameobject is a dropped spell.
/// </summary>
public class DroppedSpell : MonoBehaviour
{
    public ISpell SpellDropped { get; set; }
}
