using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for looping gameplay music.
/// </summary>
public class LoopGameplayMusic : MonoBehaviour, IFindPlayer
{
    private AudioSource audioSource;
    private WaitForSecondsRealtime wfs;
    private float initialVolume;

    [SerializeField] private AudioClipWithVolume[] audioClips;
    private readonly string CLIPINDEX = "CLIPINDEX";

    public bool HasPlayerSpawned { get; private set; }

    private IEnumerator pickNextAudioClipCoroutine;
    private IEnumerator fadeVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        wfs = new WaitForSecondsRealtime(0.1f);
        HasPlayerSpawned = false;
    }

    private void Update()
    {
        if (HasPlayerSpawned == false) 
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
        while (nextClipIndex == PlayerPrefs.GetInt(CLIPINDEX, 999))
        {
            nextClipIndex = Random.Range(0, audioClips.Length);
            yield return null;
        }
        PlayerPrefs.SetInt(CLIPINDEX, nextClipIndex);
        audioClips[PlayerPrefs.GetInt(CLIPINDEX)].PlayOnAudioSource(audioSource);
        initialVolume = audioClips[PlayerPrefs.GetInt(CLIPINDEX)].Volume;
        pickNextAudioClipCoroutine = null;
    }

    public void FindPlayer()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        PlayerPrefs.SetInt(CLIPINDEX, Random.Range(0, audioClips.Length));
        audioClips[PlayerPrefs.GetInt(CLIPINDEX)].PlayOnAudioSource(audioSource);
        initialVolume = audioClips[PlayerPrefs.GetInt(CLIPINDEX)].Volume;
        HasPlayerSpawned = true;
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }

    public void FadeInVolume() => 
        this.StartCoroutineWithReset(ref fadeVolume, FadeInVolumeCoroutine());
    public void FadeOutVolume() => 
        this.StartCoroutineWithReset(ref fadeVolume, FadeOutVolumeCoroutine());

    private IEnumerator FadeInVolumeCoroutine()
    {
        while (audioSource.volume < initialVolume)
        {
            audioSource.volume += 0.05f;
            yield return wfs;
        }
    }

    private IEnumerator FadeOutVolumeCoroutine()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.05f;
            yield return wfs;
        }
    }
}
