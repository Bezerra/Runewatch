using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellPoolCreator, creates a SpellPool of SpellSO type.
/// </summary>
public class SpellPoolCreator : MonoBehaviour
{
    // Variable to know which type of prefab is being created
    [SerializeField] private SpellTypeOfPrefab spellTypeOfPrefab;

    // Manual spell pool creation
    //[SerializeField] private List<SpellPool> pools;

    private IList<SpellSO> allSpells;

    // IList with pool for every spell
    private IList<SpellPool> listSpellPools;
    private IList<SpellSecondPrefabPool> listSpellHitAndMuzzlePools;

    [SerializeField] private byte poolSize;

    private void Awake()
    {
        // Automatic pool creation
        // Finds all spells
        allSpells = FindObjectOfType<AllSpells>().SpellList;

        // Creates a dictionaries with the spell name and the spell / spell hit / spell muzzle game object
        ObjectPool<SpellPool> spellPool = 
            new ObjectPool<SpellPool>(new Dictionary<string, Queue<GameObject>>());
        ObjectPool<SpellSecondPrefabPool> spellHitOrMuzzlePool = 
            new ObjectPool<SpellSecondPrefabPool>(new Dictionary<string, Queue<GameObject>>());
        
        // Creates a list for prefabs or hits/muzzles
        switch (spellTypeOfPrefab)
        {
            case SpellTypeOfPrefab.Prefab:
                listSpellPools = new List<SpellPool>();
                break;

            case SpellTypeOfPrefab.Hit:
                listSpellHitAndMuzzlePools = new List<SpellSecondPrefabPool>();
                break;

            case SpellTypeOfPrefab.Muzzle:
                listSpellHitAndMuzzlePools = new List<SpellSecondPrefabPool>();
                break;
        }

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.Count; i++)
        {
            switch(spellTypeOfPrefab)
            {
                case SpellTypeOfPrefab.Prefab:
                    SpellPool spawnedSpellPool = new SpellPool();
                    spawnedSpellPool.Initialize(allSpells[i].Prefab.Item1, poolSize);
                    listSpellPools.Add(spawnedSpellPool);
                    break;
                case SpellTypeOfPrefab.Hit:
                    SpellSecondPrefabPool spawnedSpellHitPool = new SpellSecondPrefabPool();
                    spawnedSpellHitPool.Initialize(allSpells[i].Prefab.Item2, allSpells[i].Prefab.Item1, poolSize);
                    listSpellHitAndMuzzlePools.Add(spawnedSpellHitPool);
                    break;
                case SpellTypeOfPrefab.Muzzle:
                    SpellSecondPrefabPool spawnedSpellMuzzlePool = new SpellSecondPrefabPool();
                    spawnedSpellMuzzlePool.Initialize(allSpells[i].Prefab.Item3, allSpells[i].Prefab.Item1, poolSize);
                    listSpellHitAndMuzzlePools.Add(spawnedSpellMuzzlePool);
                    break;
            }
        }

        // // After the spell pool was created, it will create queues for all spells or hits/muzzles
        switch (spellTypeOfPrefab)
        {
            case SpellTypeOfPrefab.Prefab:
                spellPool.CreatePool(this.gameObject, listSpellPools);
                break;

            case SpellTypeOfPrefab.Hit:
                spellHitOrMuzzlePool.CreatePool(this.gameObject, listSpellHitAndMuzzlePools);
                break;

            case SpellTypeOfPrefab.Muzzle:
                spellHitOrMuzzlePool.CreatePool(this.gameObject, listSpellHitAndMuzzlePools);
                break;
        }
    }

    public enum SpellTypeOfPrefab
    {
        Prefab,
        Hit,
        Muzzle
    }
}
