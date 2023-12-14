using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    public const string AUDIOMIXER_MASTER = "MasterVolume";
    public const string AUDIOMIXER_MUSIC = "MusicVolume";
    public const string AUDIOMIXER_SFX = "SFXVolume";

    private void Awake()
    {
        globalVolumeSlider.onValueChanged.AddListener(SetGlobalVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Start()
    {
        globalVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, globalVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY , musicVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxVolumeSlider.value);
    }

    void SetGlobalVolume(float value)
    {
        audioMixer.SetFloat(AUDIOMIXER_MASTER, Mathf.Log10(value) * 20);
    }
    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(AUDIOMIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(AUDIOMIXER_SFX, Mathf.Log10(value) * 20);
    }
}
