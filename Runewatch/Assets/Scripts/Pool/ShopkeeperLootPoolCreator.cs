using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for creator damage hit pools.
/// </summary>
public class ShopkeeperLootPoolCreator : AbstractPoolCreator
{
    /// <summary>
    /// Prefab of the damage hit prefab.
    /// </summary>
    [SerializeField] private List<BasePool> pool;

    // IList with pool for every base pool
    private IList<BasePool> listOfGameObjects;

    public static ObjectPool<BasePool> Pool { get; private set; }

    private void Awake()
    {
        // Creates a dictionary with gameobject name
        Pool = new ObjectPool<BasePool>(new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        listOfGameObjects = new List<BasePool>();

        // Foreach pool
        for (int i = 0; i < pool.Count; i++)
        {
            AbstractPoolCreator.AllPrefabsToInstantiate += pool[i].Size;

            BasePool spawnedGameObject = new BasePool(pool[i].Name, pool[i].Prefab, pool[i].Size);
            listOfGameObjects.Add(spawnedGameObject);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listOfGameObjects);
    }
}
