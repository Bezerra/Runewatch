using UnityEngine;

/// <summary>
/// Abstract class responsible for on hit monobehaviours.
/// </summary>
public abstract class SpellOnHitBehaviourAbstract : MonoBehaviour
{
    public abstract ISpell Spell { get; set; }

    public abstract float TimeSpawned { get; set; }

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableHitSpell()
    {
        gameObject.SetActive(false);
    }
}
