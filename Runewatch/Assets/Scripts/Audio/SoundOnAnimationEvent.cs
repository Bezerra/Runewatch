using UnityEngine;

/// <summary>
/// Class responsible for triggering a sound with ambience sound pool name 
/// with an animation event.
/// </summary>
public class SoundOnAnimationEvent : MonoBehaviour
{
    /// <summary>
    /// Instantiates a sound from ambience sound pool.
    /// </summary>
    /// <param name="nameOfSound">Name of the sound.</param>
    public void PlaySoundAmbienceSoundPool(string nameOfSound)
    {
        AmbienceSoundPoolCreator.Pool.InstantiateFromPool(
            nameOfSound, transform.position, Quaternion.identity);
    }
}
