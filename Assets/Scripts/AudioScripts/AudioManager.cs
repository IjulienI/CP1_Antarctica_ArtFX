using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public const string MASTER_KEY = "MasterVolume";
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource musicSource2;
    [SerializeField] private AudioSource musicSource3;
    [SerializeField] private AudioClip musicChaseBeginningClip;
    [SerializeField] private AudioClip musicChaseMiddleClip;
    [SerializeField] private AudioClip musicChaseEndClip;
    public static AudioManager instance;

    private bool isTransitioning = false;
    public bool isLooping = true;
    bool i = false;
    bool l = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        LoadVolume();
        ChaseMusic();
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

    void ChaseMusic()
    {
        musicSource.Play();
        l = true;
    }
    private void Update()
    {
        if (!isTransitioning && isLooping && l)
        {
            isTransitioning = true;
            // Démarrer la lecture du morceau 2 en avance
            musicSource2.PlayScheduled(AudioSettings.dspTime + musicSource.clip.length);
        }
        if (!isLooping && !i && isTransitioning)
        {
            i= true;
            print("oui");
            musicSource2.loop = false;
            float a = musicSource2.clip.length - musicSource2.time; ;
            musicSource3.PlayScheduled(AudioSettings.dspTime + a); // 1.0 est le décalage temporel, ajustez selon vos besoins
        }
    }
}
