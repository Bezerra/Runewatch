using UnityEngine;

/// <summary>
/// Class responsible for playing the sound of an ambience object with interaction.
/// </summary>
public class AmbienceObjectSoundInteraction : MonoBehaviour
{
    [SerializeField] private AbstractSoundSO soundToPlay;

    private AudioSource audioSource;

    private void Awake() =>
        audioSource = GetComponent<AudioSource>();

    private void OnEnable()
    {
        soundToPlay.PlaySound(audioSource);
    }
}
