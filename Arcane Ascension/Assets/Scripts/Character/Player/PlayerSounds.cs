using UnityEngine;

/// <summary>
/// Class responsible for playing player's sounds.
/// </summary>
public class PlayerSounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO dashSound;

    private PlayerAudioSources audioSources;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        audioSources = GetComponentInChildren<PlayerAudioSources>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerMovement.EventDash += PlayDash;
    }

    private void OnDisable()
    {
        playerMovement.EventDash -= PlayDash;
    }

    private void PlayDash() =>
        dashSound.PlaySound(audioSources.GetFreeAudioSource());
}
