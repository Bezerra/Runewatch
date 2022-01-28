using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopGameplayMusic : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClipWithVolume[] audioClips;
    private int clipIndex;

    private IEnumerator pickNextAudioClipCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        clipIndex = Random.Range(0, audioClips.Length);
        audioClips[clipIndex].PlayOnAudioSource(audioSource);
    }

    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            if (pickNextAudioClipCoroutine == null)
            {
                if (audioClips.Length == 1)
                {
                    audioClips[0].PlayOnAudioSource(audioSource);
                    return;
                }

                pickNextAudioClipCoroutine = PickNextAudioClipCoroutine();
                StartCoroutine(pickNextAudioClipCoroutine);
            }
        }
    }

    private IEnumerator PickNextAudioClipCoroutine()
    {
        int nextClipIndex = Random.Range(0, audioClips.Length);
        while (nextClipIndex == clipIndex)
        {
            nextClipIndex = Random.Range(0, audioClips.Length);
            yield return null;
        }
        clipIndex = nextClipIndex;
        audioClips[clipIndex].PlayOnAudioSource(audioSource);
        pickNextAudioClipCoroutine = null;
    }
}
