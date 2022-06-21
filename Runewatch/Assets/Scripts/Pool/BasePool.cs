using UnityEngine;
using System;

/// <summary>
/// Base class for pools.
/// </summary>
[Serializable]
public class BasePool
{
    [SerializeField] protected string name;
    [SerializeField] protected GameObject prefab;
    [Range(1, 255)][SerializeField] protected byte size;

    public virtual string Name => name;
    public virtual GameObject Prefab => prefab;
    public virtual byte Size => size;

    /// <summary>
    /// Constructor for prefab and size.
    /// </summary>
    /// <param name="prefab">GameObject prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public BasePool(GameObject prefab, byte size)
    {
        this.prefab = prefab;
        this.size = size;
    }

    /// <summary>
    /// Constructor for name, prefab and size.
    /// </summary>
    /// <param name="name">GameObject name.</param>
    /// <param name="prefab">GameObject prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public BasePool(string name, GameObject prefab, byte size)
    {
        this.name = name;
        this.prefab = prefab;
        this.size = size;
    }
}
