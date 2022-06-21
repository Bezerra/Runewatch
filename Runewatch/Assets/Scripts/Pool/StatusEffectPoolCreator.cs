using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for creating loot status effects pools.
/// </summary>
public class StatusEffectPoolCreator : AbstractPoolCreator
{
    /// <summary>
    /// Prefab of the status effect prefab.
    /// </summary>
    [SerializeField] private List<StatusEffectPool> pool;

    // IList with pool for every base pool
    private IList<StatusEffectPool> listOfGameObjects;

    public static ObjectPool<StatusEffectPool> Pool { get; private set; }

    private void Awake()
    {
        // Creates a dictionary with gameobject name
        Pool = new ObjectPool<StatusEffectPool>(
            new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        listOfGameObjects = new List<StatusEffectPool>();

        // Foreach pool
        for (int i = 0; i < pool.Count; i++)
        {
            AbstractPoolCreator.AllPrefabsToInstantiate += pool[i].Size;

            StatusEffectPool spawnedGameObject = 
                new StatusEffectPool(pool[i].StatusName.ToString(), pool[i].Prefab, 
                pool[i].Size);
            listOfGameObjects.Add(spawnedGameObject);
        }

        Pool.CreatePool(this.gameObject, listOfGameObjects);
    }
}
