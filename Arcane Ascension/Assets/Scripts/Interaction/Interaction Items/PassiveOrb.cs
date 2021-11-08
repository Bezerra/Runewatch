using UnityEngine;

/// <summary>
/// Class responsible for passive orbs.
/// </summary>
public class PassiveOrb : MonoBehaviour, IInterectableWithSound
{
    [Header("Sound to be played when interected with")]
    [SerializeField] private LootAndInteractionSoundType lootAndInteractionSoundType;
    public LootAndInteractionSoundType LootAndInteractionSoundType => lootAndInteractionSoundType;
}
