using UnityEngine;
using System;

/// <summary>
/// Serializable class for loot items.
/// </summary>
[Serializable]
public class ItemLootPool : BasePool
{
    [Header("Loot Name substitutes name field in this class. Name can be blank.")]
    [SerializeField] private LootAndInteractionType lootName;
    public LootAndInteractionType LootName => lootName;

    /// <summary>
    /// Constructor for spell pool.
    /// </summary>
    /// <param name="prefab">Spell prefab.</param>
    /// <param name="size">Size of pool.</param>
    public ItemLootPool(string name, GameObject prefab, byte size) : base(name, prefab, size)
    { }
}
