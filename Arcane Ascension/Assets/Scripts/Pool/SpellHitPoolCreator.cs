using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellHitPoolCreator, creates a SpellHitPool of spell muzzle type.
/// </summary>
public class SpellHitPoolCreator : MonoBehaviour
{
    private IList<SpellSO> allSpells;

    private IList<SpellPool> listSpellHitPools;

    [SerializeField] private byte poolSize;

    public static ObjectPool<SpellPool> Pool { get; private set; }

    private void Awake()
    {
        // Automatic pool creation
        // Finds all spells
        allSpells = FindObjectOfType<AllSpells>().SpellList;

        // Creates a dictionaries with the spell name and the spell / spell hit / spell muzzle game object
        Pool =
            new ObjectPool<SpellPool>(new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        listSpellHitPools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.Count; i++)
        {
            SpellPool spawnedSpellHitPool = new SpellPool();
            if (allSpells[i].Prefab.Item3 != null)
            {
                spawnedSpellHitPool.Initialize(allSpells[i].Prefab.Item2, allSpells[i].Prefab.Item1, poolSize);
                listSpellHitPools.Add(spawnedSpellHitPool);
            }
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listSpellHitPools);
    }

}
