using UnityEngine;

/// <summary>
/// Class responsible for controlling an enemy audio sources.
/// </summary>
public class EnemyAudioSources : CharacterAudioSources
{
    [SerializeField] private AudioSource enemyHitAudioSource;

    public AudioSource EnemyHitAudioSource => enemyHitAudioSource;
}
