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

    public virtual string Name { get; }
    public virtual GameObject Prefab { get; }
    public virtual int Size { get; }
}
