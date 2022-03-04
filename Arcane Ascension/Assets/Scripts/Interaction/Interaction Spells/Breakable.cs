using UnityEngine;

/// <summary>
/// Class for breakable objects.
/// </summary>
public class Breakable : AbstractInteractionWithSpell
{
    [SerializeField] private GameObject brokenVase;

    protected override void ActionToTake()
    {
        Instantiate(brokenVase, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
