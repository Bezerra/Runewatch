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
    [Range(0, 200)] [SerializeField] protected int size;

    public virtual string Name => name;
    public virtual GameObject Prefab => prefab;
    public virtual int Size => size;

    /// <summary>
    /// Constructor for prefab and size.
    /// </summary>
    /// <param name="prefab">GameObject prefab.</param>
    /// <param name="size">Size of the pool.</param>
    public BasePool(GameObject prefab, int size)
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
    public BasePool(string name, GameObject prefab, int size)
    {
        this.name = name;
        this.prefab = prefab;
        this.size = size;
    }
}
