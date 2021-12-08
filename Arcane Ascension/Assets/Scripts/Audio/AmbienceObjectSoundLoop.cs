using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for playing the sound of an ambience object.
/// </summary>
public class AmbienceObjectSoundLoop : MonoBehaviour
{
    [SerializeField] private AbstractSoundSO soundToPlay;

    private AudioSource audioSource;

    private void Awake() =>
        audioSource = GetComponent<AudioSource>();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));

        audioSource.loop = true;
        soundToPlay.SetOnAudioSource(audioSource);

        if (TryGetComponent(out AudioEmitterWithOcclusion audioEmitter))
            audioEmitter.InitialValue = audioSource.volume;
    }
}
