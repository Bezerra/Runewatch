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
    private IList<SpellPool> pools;

    [SerializeField] private byte poolSize;

    private void Awake()
    {
        ObjectPool<SpellPool> spellPool = new ObjectPool<SpellPool>(new Dictionary<string, Queue<GameObject>>());

        // Automatic pool creation ///////////////////////////
        allSpells = FindObjectOfType<AllSpells>().SpellList;
        pools = new List<SpellPool>();

        // Foreach existent spell, creates a spellPool with its prefab and the size of the pool
        for (int i = 0; i < allSpells.Count; i++)
        {
            SpellPool pool = new SpellPool();
            pool.Initialize(allSpells[i].Prefab, poolSize);
            pools.Add(pool);
        }
        // Automatic pool creation ///////////////////////////

        // After the spell pool was created, it will create the pool for all spells
        spellPool.CreatePool(this.gameObject, pools);
    }
}
