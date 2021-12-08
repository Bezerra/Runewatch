using System.Collections;
using UnityEngine;

public abstract class AbstractSoundPrefab : MonoBehaviour
{
    protected AudioSource audioSource;

    private void Awake() =>
        audioSource = GetComponent<AudioSource>();

    /// <summary>
    /// This sound is played when an item is enabled from a pool.
    /// </summary>
    private void OnEnable() =>
        StartCoroutine(PlaySound());

    protected abstract IEnumerator PlaySound();
}
