using UnityEngine;

/// <summary>
/// Class responsible for controlling a player audio sources.
/// </summary>
public class PlayerAudioSources : CharacterAudioSources
{
    [SerializeField] private AudioSource playerStepsAudioSource;

    public AudioSource PlayerStepsAudioSource => playerStepsAudioSource;
}
