using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Updates a slider option.
/// </summary>
public class VolumeSliderOption : PlayerPreferencesOption
{
    [SerializeField] private AudioMixerGroup audioMixer;
    [SerializeField] private AudioOption optionToUpdate;
    [SerializeField] private Slider slider;

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        slider.value = PlayerPrefs.GetFloat(optionToUpdate.ToString(), 0);

        switch(optionToUpdate)
        {
            case AudioOption.MasterVolume:
                audioMixer.audioMixer.SetFloat("MasterVolume", slider.value);
                break;
            case AudioOption.AmbienceVolume:
                audioMixer.audioMixer.SetFloat("AmbienceVolume", slider.value);
                break;
            case AudioOption.SFXVolume:
                audioMixer.audioMixer.SetFloat("SFXVolume", slider.value);
                break;
            case AudioOption.MusicVolume:
                audioMixer.audioMixer.SetFloat("MusicVolume", slider.value);
                break;
        }
    }

    public override void UpdateValue(float value)
    {
        PlayerPrefs.SetFloat(optionToUpdate.ToString(), value);

        switch (optionToUpdate)
        {
            case AudioOption.MasterVolume:
                audioMixer.audioMixer.SetFloat("MasterVolume", value);
                break;
            case AudioOption.AmbienceVolume:
                audioMixer.audioMixer.SetFloat("AmbienceVolume", value);
                break;
            case AudioOption.SFXVolume:
                audioMixer.audioMixer.SetFloat("SFXVolume", value);
                break;
            case AudioOption.MusicVolume:
                audioMixer.audioMixer.SetFloat("MusicVolume", value);
                break;
        }
    }

    private enum AudioOption
    {
        MasterVolume,
        AmbienceVolume,
        SFXVolume,
        MusicVolume,
    }
}
