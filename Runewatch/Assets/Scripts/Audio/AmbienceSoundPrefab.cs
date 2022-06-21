using UnityEngine;
using System.Collections;

/// <summary>
/// Class that holds a sound to play.
/// </summary>
public class AmbienceSoundPrefab : AbstractSoundPrefab
{
    [SerializeField] private AbstractSoundSO ambienceSound;

    protected override IEnumerator PlaySound()
    {
        yield return null;
        ambienceSound.PlaySound(audioSource);
    }
}
