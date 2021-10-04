using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic struct that creates -T- pools of gameobjects.
/// </summary>
public struct ObjectPool<T> where T : BasePool
{
    private readonly IDictionary<string, Queue<GameObject>> poolDictionary;
    private static ObjectPool<T> instance;

    public ObjectPool(IDictionary<string, Queue<GameObject>> poolDictionary)
    {
        this.poolDictionary = poolDictionary;
        instance = this;
    }

    /// <summary>
    /// Creates pools for every pool and adds them to a dictionary with all pools.
    /// </summary>
    /// <param name="parent">Gameobject that created this instance.</param>
    /// <param name="pools">List with pools</param>
    public void CreatePool(GameObject parent, IList<T> pools)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int j = 0; j < pools[i].Size; j++)
            {
                GameObject obj = MonoBehaviour.Instantiate(pools[i].Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.parent = parent.transform;
            }

            poolDictionary.Add(pools[i].Name, objectPool);
        }
    }

    /// <summary>
    /// Spawns an object from a pool.
    /// </summary>
    /// <param name="name">Name of the object.</param>
    /// <param name="position">Position of the object.</param>
    /// <param name="rotation">Rotation of the object.</param>
    /// <returns>Returns spawned gameobject.</returns>
    public static GameObject InstantiateFromPool (string name, Vector3 position, Quaternion rotation)
    {
        if (!instance.poolDictionary.ContainsKey(name))
        {
            MonoBehaviour.print($"Pool with name " + name + "doesn't exist.");
            return null;
        }

        GameObject obj = instance.poolDictionary[name].Dequeue();
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);
        instance.poolDictionary[name].Enqueue(obj);

        return obj;
    }

    /// <summary>
    /// Spawns an object from a pool.
    /// </summary>
    /// <param name="name">Name of the object.</param>
    /// <returns>Returns spawned gameobject.</returns>
    public static GameObject InstantiateFromPool(string name)
    {
        if (!instance.poolDictionary.ContainsKey(name))
        {
            MonoBehaviour.print($"Pool with name " + name + "doesn't exist.");
            return null;
        }

        GameObject obj = instance.poolDictionary[name].Dequeue();
        obj.SetActive(true);
        instance.poolDictionary[name].Enqueue(obj);

        return obj;
    }
}
