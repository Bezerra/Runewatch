using UnityEngine;
using System;

/// <summary>
/// Serializable class for loot items.
/// </summary>
[Serializable]
public class LootSoundPool : BasePool
{
    [Header("Loot Name substitutes name field in this class. Name can be blank.")]
    [SerializeField] private LootAndInteractionSoundType lootName;
    public LootAndInteractionSoundType LootName => lootName;

    /// <summary>
    /// Constructor for spell pool.
    /// </summary>
    /// <param name="prefab">Spell prefab.</param>
    /// <param name="size">Size of pool.</param>
    public LootSoundPool(string name, GameObject prefab, byte size) : base(name, prefab, size)
    { }
}
