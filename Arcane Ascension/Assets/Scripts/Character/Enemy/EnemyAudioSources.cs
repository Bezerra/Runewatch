using UnityEngine;

/// <summary>
/// Class responsible for controlling an enemy audio sources.
/// </summary>
public class EnemyAudioSources : CharacterAudioSources
{
    [SerializeField] private AudioSource voiceAudioSource;

    public AudioSource VoiceAudioSource => voiceAudioSource;

    [SerializeField] private AudioSource enemyHitAudioSource;

    public AudioSource EnemyHitAudioSource => enemyHitAudioSource;

    [SerializeField] private AudioSource movementAudioSource;
    
    public AudioSource MovementAudioSource => movementAudioSource;
}
