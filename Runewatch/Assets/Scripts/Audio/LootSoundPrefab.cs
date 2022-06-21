using UnityEngine;
using System.Collections;

/// <summary>
/// Class that holds a sound to play.
/// </summary>
public class LootSoundPrefab : AbstractSoundPrefab
{
    [SerializeField] private LootSoundsSO lootSounds;
    [SerializeField] private LootAndInteractionSoundType lootName;

    protected override IEnumerator PlaySound()
    {
        yield return null;
        lootSounds.PlaySound(lootName, audioSource);
    }
}
