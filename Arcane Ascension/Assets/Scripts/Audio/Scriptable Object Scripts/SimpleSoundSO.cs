using UnityEngine;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object responsible for playing a sound from a list.
/// </summary>
[CreateAssetMenu(menuName = "Audio/Simple Sound", fileName = "Simple Sound")]
sealed public class SimpleSoundSO : AbstractSoundSO
{
    [SerializeField] private string description = "Plays one sound with defined values";

    [Range(0f, 2f)] [SerializeField] private float volume;
    [Range(0f, 2f)] [SerializeField] private float pitch;

    /// <summary>
    /// Plays a sound on an audiosource.
    /// </summary>
    /// <param name="audioSource">Audio source to play the sound on.</param>
    public override void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClips[0], volume);
        }
    }

    /// <summary>
    /// Sets an audioclip on an audio source.
    /// </summary>
    /// <param name="audioSource">Target audiosource.</param>
    public override void SetOnAudioSource(AudioSource audioSource)
    {
        audioSource.pitch = pitch;
        audioSource.volume = volume;

        audioSource.clip = audioClips[0];
        audioSource.Play();
    }
}