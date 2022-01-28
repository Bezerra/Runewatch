using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for looping gameplay music.
/// </summary>
public class LoopGameplayMusic : MonoBehaviour, IFindPlayer
{
    private AudioSource audioSource;

    [SerializeField] private AudioClipWithVolume[] audioClips;
    private readonly string CLIPINDEX = "CLIPINDEX";

    private bool hasPlayerSpawned;

    private IEnumerator pickNextAudioClipCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        hasPlayerSpawned = false;
    }

    private void Update()
    {
        if (hasPlayerSpawned == false) 
            return;

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
        while (nextClipIndex == PlayerPrefs.GetInt(CLIPINDEX))
        {
            nextClipIndex = Random.Range(0, audioClips.Length);
            yield return null;
        }
        PlayerPrefs.SetInt(CLIPINDEX, nextClipIndex);
        audioClips[PlayerPrefs.GetInt(CLIPINDEX)].PlayOnAudioSource(audioSource);
        pickNextAudioClipCoroutine = null;
    }

    public void FindPlayer()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        PlayerPrefs.SetInt(CLIPINDEX, Random.Range(0, audioClips.Length));
        audioClips[PlayerPrefs.GetInt(CLIPINDEX)].PlayOnAudioSource(audioSource);
        hasPlayerSpawned = true;
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
