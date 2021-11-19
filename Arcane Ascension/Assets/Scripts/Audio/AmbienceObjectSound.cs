using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for playing the sound of an ambience object.
/// </summary>
public class AmbienceObjectSound : MonoBehaviour
{
    [SerializeField] private AbstractSoundSO soundToPlay;

    private AudioSource audioSource;

    private void Awake() =>
        audioSource = GetComponent<AudioSource>();

    private void Start()
    {
        audioSource.loop = true;
        soundToPlay.SetOnAudioSource(audioSource);
    }
}
