using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct with loot pieces and a list with all loot the enemy dropped.
/// </summary>
[Serializable]
public struct LootRates
{
    [SerializeField] private List<LootPiece> lootPieces;
    public List<LootPiece> LootPieces => lootPieces;

    /// <summary>
    /// Class with a type of loot and its chance of being dropped.
    /// </summary>
    [Serializable]
    public class LootPiece
    {
        [SerializeField] private LootType lootType;
        [Range(0f, 100f)] [SerializeField] private float lootRate;

        public LootType LootType => lootType;
        public float LootRate { get => lootRate; set => lootRate = value; }
    }
}
