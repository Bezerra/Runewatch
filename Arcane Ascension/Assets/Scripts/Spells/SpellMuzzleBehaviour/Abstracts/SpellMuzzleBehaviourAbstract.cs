using UnityEngine;

/// <summary>
/// Abstract Monobehaviour for spell muzzles.
/// </summary>
public abstract class SpellMuzzleBehaviourAbstract : MonoBehaviour
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public abstract ISpell Spell { get; set; }

    public abstract float TimeSpawned { get; set; }

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableMuzzleSpell()
    {
        gameObject.SetActive(false);
    }
}
