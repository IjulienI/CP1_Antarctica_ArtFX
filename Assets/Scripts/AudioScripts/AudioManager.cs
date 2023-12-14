using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public const string MASTER_KEY = "MasterVolume";
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        LoadVolume();
    }

    void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        audioMixer.SetFloat(VolumeSettings.AUDIOMIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.AUDIOMIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.AUDIOMIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
