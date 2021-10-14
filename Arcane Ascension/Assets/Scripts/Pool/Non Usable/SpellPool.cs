using UnityEngine;
using System;

/// <summary>
/// Serializable class for spell / spell hits / spell muzzles pools.
/// If it's initialized with only one prefab, it means it's the spell itself, else it means it's a muzzle or a hit.
/// </summary>
[Serializable]
sealed public class SpellPool: BasePool
{
    /// <summary>
    /// Gets name of the parent spell.
    /// Will use parentSpellPrefab if it's a hit or a muzzle (so it gets the name of their correspondent spell)
    /// </summary>
    public override string Name => 
        parentSpellPrefab == null ? 
        prefab.GetComponent<SpellBehaviourAbstract>().Spell.Name :
        parentSpellPrefab.GetComponent<SpellBehaviourAbstract>().Spell.Name;

    // Variable used if this this was initialized by a hit or a muzzle
    // This variable keeps track of the original spell
    private GameObject parentSpellPrefab;

    public override GameObject Prefab => prefab;
    public override int Size => size;

    /// <summary>
    /// Initializer for spells.
    /// </summary>
    /// <param name="prefab">Spell prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public void Initialize(GameObject prefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
    }

    /// <summary>
    /// Initializes for hits and muzzles.
    /// </summary>
    /// <param name="prefab">Hit or muzzle prefab.</param>
    /// <param name="parentSpellPrefab">Spell prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public void Initialize(GameObject prefab, GameObject parentSpellPrefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
        this.parentSpellPrefab = parentSpellPrefab;
    }
}
