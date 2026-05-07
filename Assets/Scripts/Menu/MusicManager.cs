using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    private SettingsManager settings;
    public AudioClip worldBGMusic;
    public AudioClip menuMusic;
    

    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindFirstObjectByType<MusicManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<MusicManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        LoadSettings(); // Apply mute and volume on startup

        SceneManager.sceneLoaded += OnSceneLoaded;
    }





    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        SetMute(!musicEnabled);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        audioSource.volume = musicVolume;

        if (scene.name == "MainMenu")
            PlayMusic(menuMusic);
        else if (scene.name == "World")
            PlayMusic(worldBGMusic);
    }





    public void PlayMusic(AudioClip clip)
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        if (!musicEnabled)
            return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }


    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume; // Apply only
    }


    public void SetMute(bool isMuted)
    {
        audioSource.mute = isMuted;
    }

    private void LoadSettings()
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        audioSource.mute = !musicEnabled;
        audioSource.volume = musicVolume;
    }

    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }

    public bool IsMusicEnabled()
    {
        return !audioSource.mute;
    }

}
