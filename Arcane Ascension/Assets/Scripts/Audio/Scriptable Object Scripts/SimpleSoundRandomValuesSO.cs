using UnityEngine;

/// <summary>
/// Scriptable object responsible for playing a sound with random values.
/// </summary>
[CreateAssetMenu(menuName = "Audio/Simple Sound Random Values", fileName = "Simple Sound Random Values")]
sealed public class SimpleSoundRandomValuesSO : AbstractSoundScriptableObject
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
        audioSource.pitch = Random.Range(pitch.x, pitch.y);
        audioSource.PlayOneShot(
            audioClips[0],
            Random.Range(volume.x, volume.y));
    }
}
