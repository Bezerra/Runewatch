using UnityEngine;

/// <summary>
/// Class that holds a sound to play.
/// </summary>
public class LootSound : MonoBehaviour
{
    [SerializeField] private LootAndInteractionType lootName;
    [SerializeField] private LootSoundsSO lootSounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnEnable() =>
        lootSounds.PlaySound(lootName, audioSource);
}
