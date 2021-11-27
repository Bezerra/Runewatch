using UnityEngine;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object responsible for playing a random sound from a list with random values.
/// </summary>
[CreateAssetMenu(menuName = "Audio/Random Sound Random Values", fileName = "Random Sound Random Values")]
sealed public class RandomSoundRandomValuesSO : AbstractSoundSO
{
    [SerializeField] private string description = "Plays a random sound with random values";

    [RangeMinMax(0f, 2f)] [SerializeField] private Vector2 volume;
    [RangeMinMax(0f, 2f)] [SerializeField] private Vector2 pitch;

    /// <summary>
    /// Plays a sound on an audiosource.
    /// </summary>
    /// <param name="audioSource">Audio source to play the sound on.</param>
    public override void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(pitch.x, pitch.y);
            audioSource.PlayOneShot(
                audioClips[Random.Range(0, audioClips.Count)],
                Random.Range(volume.x, volume.y));
        }
    }

    /// <summary>
    /// Sets an audioclip on an audio source.
    /// </summary>
    /// <param name="audioSource">Target audiosource.</param>
    public override void SetOnAudioSource(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(pitch.x, pitch.y);
        audioSource.volume = Random.Range(volume.x, volume.y);

        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.Play();
    }
}