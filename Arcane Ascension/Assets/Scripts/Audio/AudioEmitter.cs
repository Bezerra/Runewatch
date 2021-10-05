using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioEmitter : MonoBehaviour
{
    [SerializeField] private List<AbstractSoundScriptableObject> sound;

    [SerializeField] private bool playOnAwake;
    [SerializeField] private int playOnAwakeClipIndex;
    [SerializeField] private bool loop;

    [Header("Should be equal to max distance on audio source")]
    [SerializeField] private float soundMaxDistance = 15f;

    [Header("Fade volume")]
    [Range(0,1f)][SerializeField] private float fadeUntilThisValue = 0.3f;

    private AudioSource audioSource;
    private AudioListener listener;
    private float currentDistance;
    private bool currentlyOccluded;

    private IEnumerator soundFadeCoroutine;
    private YieldInstruction wffu;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        listener = FindObjectOfType<AudioListener>();

        audioSource.spatialBlend = 1;
        if (audioSource.maxDistance > soundMaxDistance) audioSource.maxDistance = soundMaxDistance;

        wffu = new WaitForFixedUpdate();

        // Is this object occluded from player
        if (Physics.Raycast(transform.position, (listener.transform.position - transform.position).normalized,
            currentDistance, Layers.AllExceptPlayerAndEnemy))
        {
            currentlyOccluded = true;
        }
        else
        {
            currentlyOccluded = false;
        }
    }

    /// <summary>
    /// Plays a sound for a list of sound assets.
    /// </summary>
    /// <param name="index">Index of sounds asset list.</param>
    public void PlaySound(int index)
    {
        sound[index].PlaySound(audioSource);
    }

    private void Start()
    {
        audioSource.loop = loop ? audioSource.loop = true : audioSource.loop = false;
        if (playOnAwake) PlaySound(playOnAwakeClipIndex);
    }

    /// <summary>
    /// If object is in a minimum distance from audio listener, it starts calls CheckSoundOcclusion method.
    /// </summary>
    private void Update()
    {
        if (listener != null)
        {
            if ((listener.transform.position - transform.position).magnitude <= soundMaxDistance)
            {
                currentDistance = (listener.transform.position - transform.position).magnitude;
                CheckSoundOcclusion();
            }
        }
    }

    /// <summary>
    /// Checks if object is occluded. Starts coroutines to fade volume.
    /// </summary>
    private void CheckSoundOcclusion()
    {
        // Is this object occluded from player
        if (Physics.Raycast(transform.position, (listener.transform.position - transform.position).normalized,
            currentDistance, Layers.AllExceptPlayerAndEnemy))
        {
            // Happens once
            if (currentlyOccluded == false)
            {
                currentlyOccluded = true;
                if (soundFadeCoroutine != null) StopCoroutine(soundFadeCoroutine);
                soundFadeCoroutine = FadeOutCoroutine();
                StartCoroutine(soundFadeCoroutine);
            }
        }
        // Else
        else
        {
            // Happens once
            if (currentlyOccluded == true)
            {
                currentlyOccluded = false;
                if (soundFadeCoroutine != null) StopCoroutine(soundFadeCoroutine);
                soundFadeCoroutine = FadeInCoroutine();
                StartCoroutine(soundFadeCoroutine);
            }
        }
    }

    /// <summary>
    /// Fades out volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator FadeOutCoroutine()
    {
        while (audioSource.volume > fadeUntilThisValue)
        {
            audioSource.volume -= Time.deltaTime * 2f;

            if (audioSource.volume < fadeUntilThisValue)
                audioSource.volume = fadeUntilThisValue;

            yield return wffu;
        }
    }

    /// <summary>
    /// Fades out volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator FadeInCoroutine()
    {
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime * 2f;

            if (audioSource.volume > 1)
                audioSource.volume = 1;

            yield return wffu;
        }
    }
}
