using UnityEngine;
using System.Collections.Generic;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object base class for all sounds.
/// </summary>
public abstract class AbstractSoundSO : ScriptableObject
{
    [SerializeField] protected List<AudioClip> audioClips;

    /// <summary>
    /// Method responsible for playing sounds.
    /// </summary>
    /// <param name="audioSource">Audio source to play sounds.</param>
    public abstract void PlaySound(AudioSource audioSource);

    /// <summary>
    /// Sets an audioclip on an audio source.
    /// </summary>
    /// <param name="audioClip">Audioclip to insert on audiosource.</param>
    /// <param name="audioSource">Target audiosource.</param>
    public abstract void SetOnAudioSource(AudioSource audioSource);
}