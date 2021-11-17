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
    [Header("Healing Potion is only affected by CharacterSaveDataController Reaper variable. " +
        "Rate can be ignored.")]
    [SerializeField] private LootRates lootRates;
    public LootRates Rates => lootRates;

    [BoxGroup("Damage Stats")]
    [Header("Character list of spells")]
    [SerializeField] private List<EnemySpell> availableSpells;
    public List<EnemySpell> AllEnemySpells => availableSpells;

    /// <summary>
    /// Initiates lootrates struct.
    /// </summary>
    /// <param name="stpData">Save data.</param>
    public void LootRatesInit(CharacterSaveDataController stpData) =>
        lootRates.Init(stpData);

    /// <summary>
    /// Struct with loot pieces and a list with all loot the enemy dropped.
    /// </summary>
    [Serializable]
    public struct LootRates
    {
        [SerializeField] private List<LootPiece> lootPieces;
        private float randomChance;
        public IList<(LootType, Vector3)> DroppedLoot { get; private set; }
        private CharacterSaveDataController stpData;

        public void Init(CharacterSaveDataController stpData)
        {
            DroppedLoot = new List<(LootType, Vector3)>();
            randomChance = UnityEngine.Random.Range(0, 100f);
            this.stpData = stpData;
        }

        /// <summary>
        /// Gets a drop and sets random position with a received position.
        /// </summary>
        /// <param name="position">Position to set the item.</param>
        public void GetDrop(Vector3 position)
        {
            for (int i = 0; i < lootPieces.Count; i++)
            {
                // If it's a healing potion, its rate will be automatically set
                if (lootPieces[i].LootType == LootType.PotionHealing)
                {
                    lootPieces[i].LootRate = stpData.SaveData.Reaper;
                }
                
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
        /// Class with a type of loot and its chance of being dropped.
        /// </summary>
        [Serializable]
        private class LootPiece
        {
            [SerializeField] private LootType lootType;
            [Range(0f, 100f)][SerializeField] private float lootRate;

            public LootType LootType => lootType;
            public float LootRate { get => lootRate; set => lootRate = value; }
        }
    }
}
