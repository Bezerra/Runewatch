using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// Scriptable object with stats for an enemy.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStatsSO : StatsSO
{
    // List with all possible loots in enemy
    [BoxGroup("General Stats")]
    [SerializeField] private LootRates lootRates;
    public LootRates Rates => lootRates;

    [BoxGroup("Damage Stats")]
    [Header("Character list of spells")]
    [SerializeField] private List<EnemySpell> availableSpells;

    public List<EnemySpell> AllEnemySpells => availableSpells;

    protected override void OnEnable()
    {
        base.OnEnable();
        lootRates.Init();
    }

    /// <summary>
    /// Struct with loot pieces and a list with all loot the enemy dropped.
    /// </summary>
    [Serializable]
    public struct LootRates
    {
        [SerializeField] private List<LootPiece> lootPieces;
        private float randomChance;
        public IList<(LootType, Vector3)> DroppedLoot { get; private set; }

        public void Init()
        {
            DroppedLoot = new List<(LootType, Vector3)>();
            randomChance = UnityEngine.Random.Range(0, 100f);
        }

        public void GetDrop(Vector3 position)
        {
            for (int i = 0; i < lootPieces.Count; i++)
            {
                if (randomChance < lootPieces[i].LootRate)
                {
                    Vector3 newPosition = position + new Vector3(
                        UnityEngine.Random.Range(-1f, 1f), 0, 
                        UnityEngine.Random.Range(-1f, 1f));

                    DroppedLoot.Add((lootPieces[i].LootType, newPosition));
                }
            }
        }

        /// <summary>
        /// Struct with a type of loot and its chance of being dropped.
        /// </summary>
        [Serializable]
        private struct LootPiece
        {
            [SerializeField] private LootType lootType;
            [Range(0f, 100f)][SerializeField] private float lootRate;

            public LootType LootType => lootType;
            public float LootRate => lootRate;
        }
    }
}
