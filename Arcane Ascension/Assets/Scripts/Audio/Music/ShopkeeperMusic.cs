using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for playing shopkeeper music and fading out gameplay loop music.
/// </summary>
public class ShopkeeperMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private float initialVolume;
    private WaitForSecondsRealtime wfs;
    private IEnumerator fadeVolume;

    public float CurrentVolume { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        initialVolume = audioSource.volume; // Should be on awake to start before enabling
        wfs = new WaitForSecondsRealtime(0.1f);
    }

    public void FadeInVolume() => 
        this.StartCoroutineWithReset(ref fadeVolume, FadeInVolumeCoroutine());
    public void FadeOutVolume() => 
        this.StartCoroutineWithReset(ref fadeVolume, FadeOutVolumeCoroutine());

    private IEnumerator FadeInVolumeCoroutine()
    {
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < initialVolume)
        {
            audioSource.volume += 0.05f;
            yield return wfs;
        }
        CurrentVolume = audioSource.volume;
    }

    private IEnumerator FadeOutVolumeCoroutine()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.05f;
            yield return wfs;
        }
        audioSource.Stop();
        CurrentVolume = audioSource.volume;
    }
}
