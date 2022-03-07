using UnityEngine;

/// <summary>
/// Class for breakable objects.
/// </summary>
public class Breakable : AbstractInteractionWithSpell
{
    [SerializeField] private GameObject brokenVase;
    private GameObject spawnedVase;

    private void Start()
    {
        spawnedVase = Instantiate(brokenVase, transform.position, transform.rotation);
        spawnedVase.SetActive(false);
        spawnedVase.transform.SetParent(transform.parent);
    }

    protected override void ActionToTake()
    {
        gameObject.SetActive(false);
        spawnedVase.SetActive(true);
    }
}
