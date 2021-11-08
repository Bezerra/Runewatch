using UnityEngine;

/// <summary>
/// Class that holds a sound to play.
/// </summary>
public class LootSound : MonoBehaviour
{
    [SerializeField] private LootAndInteractionSoundType lootName;
    [SerializeField] private LootSoundsSO lootSounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// This sound is played when an item is enabled from a pool.
    /// </summary>
    public void OnEnable() =>
        lootSounds.PlaySound(lootName, audioSource);
}
