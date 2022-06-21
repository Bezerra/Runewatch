using UnityEngine;
using System;

/// <summary>
/// Serializable class for spell / spell hits / spell muzzles pools.
/// If it's initialized with only one prefab, it means it's the spell itself, else it means it's a muzzle, hit or hand effect.
/// </summary>
[Serializable]
sealed public class SpellPool: BasePool
{
    /// <summary>
    /// Gets name of the parent spell.
    /// Will use parentSpellPrefab if it's a hit, muzzle or hand effect(so it gets the name of their correspondent spell).
    /// </summary>
    public override string Name => 
        parentSpellPrefab == null ? 
        prefab.GetComponent<SpellBehaviourAbstract>().Spell.Name :
        parentSpellPrefab.GetComponent<SpellBehaviourAbstract>().Spell.Name;

    // Variable used if this this was initialized by a hit or a muzzle
    // This variable keeps track of the original spell
    private GameObject parentSpellPrefab;

    /// <summary>
    /// Constructor for spell pool.
    /// </summary>
    /// <param name="prefab">Spell prefab.</param>
    /// <param name="size">Size of pool.</param>
    public SpellPool(GameObject prefab, byte size) : base(prefab, size)
    { }

    /// <summary>
    /// Constructor for hits and muzzles.
    /// </summary>
    /// <param name="prefab">Hit or muzzle prefab.</param>
    /// <param name="parentSpellPrefab">Spell prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public SpellPool(GameObject prefab, GameObject parentSpellPrefab, byte size) : base(prefab, size)
    {
        this.parentSpellPrefab = parentSpellPrefab;
    }
}
