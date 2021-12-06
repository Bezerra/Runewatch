using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour SpellMuzzlePoolCreator, creates a SpellMuzzlePool of spell muzzle type.
/// </summary>
public class SpellMuzzlePoolCreator : MonoBehaviour
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
        IList<SpellPool> listSpellMuzzlePools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.SpellList.Count; i++)
        {
            if (allSpells.SpellList[i].Prefab.Item3 != null)
            {
                SpellPool spawnedSpellMuzzlePool = 
                    new SpellPool(allSpells.SpellList[i].Prefab.Item3, 
                    allSpells.SpellList[i].Prefab.Item1, poolSize);
                listSpellMuzzlePools.Add(spawnedSpellMuzzlePool);
            }
        }

        // Adds default spell
        if (allSpells.DefaultSpell.Prefab.Item3 != null)
        {
            SpellPool spawnedSpellMuzzlePool =
                    new SpellPool(allSpells.DefaultSpell.Prefab.Item3,
                    allSpells.DefaultSpell.Prefab.Item1, poolSize);
            listSpellMuzzlePools.Add(spawnedSpellMuzzlePool);
        }

        // Adds melee attack spell
        if (allSpells.MeleeAttack.Prefab.Item3 != null)
        {
            SpellPool spawnedSpellMuzzlePool =
                    new SpellPool(allSpells.MeleeAttack.Prefab.Item3,
                    allSpells.MeleeAttack.Prefab.Item1, poolSize);
            listSpellMuzzlePools.Add(spawnedSpellMuzzlePool);
        }

        // After the spell pool was created, it will create queues for all spells or hits/muzzles
        Pool.CreatePool(this.gameObject, listSpellMuzzlePools);
    }

}
