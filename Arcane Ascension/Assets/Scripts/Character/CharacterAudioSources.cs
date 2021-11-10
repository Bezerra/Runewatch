using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling a character audio sources.
/// </summary>
public abstract class CharacterAudioSources : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;

    /// <summary>
    /// Gets a audio source that isn't currently occupied.
    /// </summary>
    /// <returns>Audio source that isn't occupied.</returns>
    public AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying == false)
            {
                return audioSources[i];
            }
        }
        return null;
    }
}
