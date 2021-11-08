using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for creator damage hit pools.
/// </summary>
public class ItemLootPoolCreator : MonoBehaviour
{
    /// <summary>
    /// Prefab of the damage hit prefab.
    /// </summary>
    [SerializeField] private List<ItemLootPool> pool;

    // IList with pool for every base pool
    private IList<ItemLootPool> listOfGameObjects;

    public static ObjectPool<ItemLootPool> Pool { get; private set; }

    private void Awake()
    {
        // Creates a dictionary with gameobject name
        Pool = new ObjectPool<ItemLootPool>(new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        listOfGameObjects = new List<ItemLootPool>();

        // Foreach pool
        for (int i = 0; i < pool.Count; i++)
        {
            ItemLootPool spawnedGameObject = 
                new ItemLootPool(pool[i].LootName.ToString(), pool[i].Prefab, pool[i].Size);
            listOfGameObjects.Add(spawnedGameObject);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listOfGameObjects);
    }
}
