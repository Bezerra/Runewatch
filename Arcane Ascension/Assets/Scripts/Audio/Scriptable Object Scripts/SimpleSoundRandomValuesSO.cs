using UnityEngine;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object responsible for playing a sound with random values.
/// </summary>
[CreateAssetMenu(menuName = "Audio/Simple Sound Random Values", fileName = "Simple Sound Random Values")]
sealed public class SimpleSoundRandomValuesSO : AbstractSoundSO
{
    [SerializeField] private string description = "Plays one sound with random values";

    [RangeMinMax(0f, 2f)] [SerializeField] private Vector2 volume;
    [RangeMinMax(0f, 2f)] [SerializeField] private Vector2 pitch;

    public override float Volume { get; protected set; }

    /// <summary>
    /// Plays a sound on an audiosource.
    /// </summary>
    /// <param name="audioSource">Audio source to play the sound on.</param>
    public override void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(pitch.x, pitch.y);
            Volume = Random.Range(volume.x, volume.y);
            audioSource.PlayOneShot(
                audioClips[0], Volume);
        }
    }

    /// <summary>
    /// Sets an audioclip on an audio source.
    /// </summary>
    /// <param name="audioSource">Target audiosource.</param>
    public override void SetOnAudioSource(AudioSource audioSource)
    {
        Volume = Random.Range(volume.x, volume.y);
        audioSource.pitch = Random.Range(pitch.x, pitch.y);
        audioSource.volume = Volume;

        audioSource.clip = audioClips[0];
        audioSource.Play();
    }
}
