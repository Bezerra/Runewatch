using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellHitPoolCreator, creates a SpellHitPool of spell muzzle type.
/// </summary>
public class SpellHitPoolCreator : AbstractPoolCreator
{
    [Range(1, 255)] [SerializeField] private byte poolSize;

    public static ObjectPool<SpellPool> Pool { get; private set; }

    private void Awake()
    {
        // Automatic pool creation
        // Finds all spells
        AllSpells allSpells = FindObjectOfType<AllSpells>();

        // Creates a dictionaries with the spell name and the spell / spell hit / spell muzzle game object
        Pool = new ObjectPool<SpellPool>(new Dictionary<string, Queue<GameObject>>());

        // Creates a list for prefabs or hits/muzzles
        IList<SpellPool> listSpellHitPools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.SpellList.Count; i++)
        {
            if (allSpells.SpellList[i].Prefab.Item2 != null)
            {
                AbstractPoolCreator.AllPrefabsToInstantiate += poolSize;

                SpellPool spawnedSpellHitPool = 
                    new SpellPool(allSpells.SpellList[i].Prefab.Item2,
                    allSpells.SpellList[i].Prefab.Item1, poolSize);
                listSpellHitPools.Add(spawnedSpellHitPool);
            }
        }

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.MonsterExclusiveSpellList.Count; i++)
        {
            if (allSpells.MonsterExclusiveSpellList[i].Prefab.Item2 != null)
            {
                AbstractPoolCreator.AllPrefabsToInstantiate += poolSize;

                SpellPool spawnedSpellHitPool =
                    new SpellPool(allSpells.MonsterExclusiveSpellList[i].Prefab.Item2,
                    allSpells.MonsterExclusiveSpellList[i].Prefab.Item1, poolSize);
                listSpellHitPools.Add(spawnedSpellHitPool);
            }
        }

        // Adds default spell
        if (allSpells.DefaultSpell.Prefab.Item2 != null)
        {
            AbstractPoolCreator.AllPrefabsToInstantiate += poolSize;

            SpellPool spawnedSpellHitPool =
                    new SpellPool(allSpells.DefaultSpell.Prefab.Item2,
                    allSpells.DefaultSpell.Prefab.Item1, poolSize);
            listSpellHitPools.Add(spawnedSpellHitPool);
        }

        // Adds melee attack spell
        if (allSpells.MeleeAttack.Prefab.Item2 != null)
        {
            AbstractPoolCreator.AllPrefabsToInstantiate += poolSize;

            SpellPool spawnedSpellHitPool =
                    new SpellPool(allSpells.MeleeAttack.Prefab.Item2,
                    allSpells.MeleeAttack.Prefab.Item1, poolSize);
            listSpellHitPools.Add(spawnedSpellHitPool);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listSpellHitPools);
    }
}
