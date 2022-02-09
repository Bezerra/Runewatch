using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Updates a slider option.
/// </summary>
public class VolumeSliderOption : PlayerPreferencesOption
{
    [SerializeField] private AudioMixerGroup audioMixer;
    [SerializeField] private PPrefsOptions optionToUpdate;
    [SerializeField] private Slider slider;

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        slider.value = PlayerPrefs.GetFloat(optionToUpdate.ToString(), -5f);

        switch(optionToUpdate)
        {
            case PPrefsOptions.MasterVolume:
                audioMixer.audioMixer.SetFloat("MasterVolume", slider.value);
                break;
            case PPrefsOptions.AmbienceVolume:
                audioMixer.audioMixer.SetFloat("AmbienceVolume", slider.value);
                break;
            case PPrefsOptions.SFXVolume:
                audioMixer.audioMixer.SetFloat("SFXVolume", slider.value);
                break;
            case PPrefsOptions.MusicVolume:
                audioMixer.audioMixer.SetFloat("MusicVolume", slider.value);
                break;
        }
    }

    public override void UpdateValue(float value)
    {
        PlayerPrefs.SetFloat(optionToUpdate.ToString(), value);

        switch (optionToUpdate)
        {
            case PPrefsOptions.MasterVolume:
                audioMixer.audioMixer.SetFloat("MasterVolume", value);

                if (value <= -29.7f)
                    audioMixer.audioMixer.SetFloat("MasterVolume", -70f);

                break;
            case PPrefsOptions.AmbienceVolume:
                audioMixer.audioMixer.SetFloat("AmbienceVolume", value);

                if (value <= -29.7f)
                    audioMixer.audioMixer.SetFloat("AmbienceVolume", -70f);

                break;
            case PPrefsOptions.SFXVolume:
                audioMixer.audioMixer.SetFloat("SFXVolume", value);

                if (value <= -29.7f)
                    audioMixer.audioMixer.SetFloat("SFXVolume", -70f);

                break;
            case PPrefsOptions.MusicVolume:
                audioMixer.audioMixer.SetFloat("MusicVolume", value);

                if (value <= -29.7f)
                    audioMixer.audioMixer.SetFloat("MusicVolume", -70f);

                break;
        }
    }
}
