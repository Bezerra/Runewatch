using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellMuzzlePoolCreator, creates a SpellMuzzlePool of spell muzzle type.
/// </summary>
public class SpellAreaHoverPoolCreator : AbstractPoolCreator
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
        IList<SpellPool> listSpellAreaOverPools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.SpellList.Count; i++)
        {
            if (allSpells.SpellList[i].Prefab.Item5 != null)
            {
                SpellPool spawnedSpellAreaOverPool = 
                    new SpellPool(allSpells.SpellList[i].Prefab.Item5, 
                    allSpells.SpellList[i].Prefab.Item1, poolSize);
                listSpellAreaOverPools.Add(spawnedSpellAreaOverPool);
            }
        }

        // Adds default spell
        if (allSpells.DefaultSpell.Prefab.Item5 != null)
        {
            SpellPool spawnedSpellAreaOverPool =
                new SpellPool(allSpells.DefaultSpell.Prefab.Item5,
                allSpells.DefaultSpell.Prefab.Item1, poolSize);
            listSpellAreaOverPools.Add(spawnedSpellAreaOverPool);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listSpellAreaOverPools);
    }

}
