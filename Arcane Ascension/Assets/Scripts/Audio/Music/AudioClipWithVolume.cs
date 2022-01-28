using UnityEngine;
using System;

/// <summary>
/// Serializable struct responsible for containing an audioclip and a volume.
/// </summary>
[Serializable]
public struct AudioClipWithVolume
{
    [SerializeField] private AudioClip audioClip;
    [Range(0f, 1f)] [SerializeField] private float volume;

    public void PlayOnAudioSource(AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioClip);
        audioSource.volume = volume;
    }
}
