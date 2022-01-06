using UnityEngine;
using System;

/// <summary>
/// Serializable class for status effect items.
/// </summary>
[Serializable]
public class StatusEffectPool : BasePool
{
    [Header("Loot Name substitutes name field in this class. Name can be blank.")]
    [SerializeField] private StatusEffectType statusName;
    public StatusEffectType StatusName => statusName;

    /// <summary>
    /// Constructor for status effect pool.
    /// </summary>
    /// <param name="prefab">Status effect prefab.</param>
    /// <param name="size">Size of pool.</param>
    public StatusEffectPool(string name, GameObject prefab, byte size) : base(name, prefab, size)
    { }
}
