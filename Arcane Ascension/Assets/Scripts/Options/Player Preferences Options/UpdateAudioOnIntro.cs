using UnityEngine;
using UnityEngine.Audio;

public class UpdateAudioOnIntro : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerMaster;
    [SerializeField] private AudioMixerGroup audioMixerMusic;
    [SerializeField] private AudioMixerGroup audioMixerSFX;

    public void Start()
    {
        audioMixerMaster.audioMixer.SetFloat("MasterVolume", 0);
        audioMixerMusic.audioMixer.SetFloat("MusicVolume", 0);
        audioMixerSFX.audioMixer.SetFloat("SFXVolume", 0);
    }
}
