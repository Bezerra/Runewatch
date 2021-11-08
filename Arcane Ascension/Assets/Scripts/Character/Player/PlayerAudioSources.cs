using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling player's audio sources.
/// </summary>
public class PlayerAudioSources : MonoBehaviour
{
    private IList<AudioSource> audioSources;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

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
