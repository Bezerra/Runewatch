using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for holding loot sounds.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Loot Sounds", fileName = "Loot Sounds")]
public class LootSoundsSO : ScriptableObject
{
    // All loot sounds serialize field
    [SerializeField] private List<LootNameSound> lootSoundsList;

    private IDictionary<LootAndInteractionType, AbstractSoundSO> data;

    /// <summary>
    /// Plays a sound fro a dictionary.
    /// </summary>
    /// <param name="lootName">Sound to paly.</param>
    /// <param name="audioSource">Audio source to play the sound in.</param>
    public void PlaySound(LootAndInteractionType lootName, AudioSource audioSource)
    {
        if (data.ContainsKey(lootName))
            data?[lootName]?.PlaySound(audioSource);
    }

    private void OnEnable()
    {
        data = new Dictionary<LootAndInteractionType, AbstractSoundSO>();

        if (lootSoundsList.Count > 0)
        {
            foreach (LootNameSound loot in lootSoundsList)
            {
                data.Add(loot.LootName, loot.LootSound);
            }
        }
    }

    [Serializable]
    private struct LootNameSound
    {
        [SerializeField] private LootAndInteractionType lootName;
        [SerializeField] private AbstractSoundSO sound;

        public LootAndInteractionType LootName => lootName;
        public AbstractSoundSO LootSound => sound;
    }
}
