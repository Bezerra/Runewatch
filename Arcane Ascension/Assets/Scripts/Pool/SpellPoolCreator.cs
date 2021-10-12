using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellPoolCreator, creates a SpellPool of SpellSO type.
/// </summary>
public class SpellPoolCreator : MonoBehaviour
{
    // Manual spell pool creation
    //[SerializeField] private List<SpellPool> pools;

    private IList<SpellSO> allSpells;

    // IList with pool for every spell
    private IList<SpellPool> listSpellPools;

    [SerializeField] private byte poolSize;

    public static ObjectPool<SpellPool> Pool { get; private set; }

    private void Awake()
    {
        // Automatic pool creation
        // Finds all spells
        allSpells = FindObjectOfType<AllSpells>().SpellList;

        // Creates a dictionaries with the spell name and the spell / spell hit / spell muzzle game object
        Pool = new ObjectPool<SpellPool>(new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        listSpellPools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.Count; i++)
        {
                SpellPool spawnedSpellPool = new SpellPool();
                spawnedSpellPool.Initialize(allSpells[i].Prefab.Item1, poolSize);
                listSpellPools.Add(spawnedSpellPool);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listSpellPools);
    }
}
