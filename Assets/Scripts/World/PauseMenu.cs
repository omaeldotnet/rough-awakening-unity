using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Video;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseMenu;

    public float fadeDuration = 0.2f;  // How long the fade lasts (in seconds)
    public AudioMixer audioMixer; // assign your MasterMixer here
    public Toggle musicToggle;
    public Slider volumeSlider;
    public MusicManager musicManager;
    private float lastMusicVolume = 1.0f; // default to full volume

    public Slider sfxSlider;      // assign the new SFX slider
    public Slider sensitivitySlider;
    public float sensitivity = 150.0f;

    private bool isPaused = false;
    private bool isFading = false;
    private bool videoFinished = false;


    void Start()
    {
        // Make sure the menu is hidden at start
        pauseMenu.alpha = 0f;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the MusicManager instance
        musicManager = MusicManager.Instance;
        VideoPlayer vp = FindObjectOfType<VideoPlayer>();
        if (vp != null)
        {
            vp.loopPointReached += OnVideoFinished;
        }


        // Add listeners to slider and toggle for real-time changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMuteToggled);
        // Load the saved settings when the scene starts
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            sfxSlider.value = sfxVolume;
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20); // dB conversion
        }

        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150.0f);
            sensitivitySlider.value = sensitivity;
        }
        else
        {
            sensitivity = 1.0f;
            sensitivitySlider.value = sensitivity;
        }      

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        ApplySettings();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isFading)
        {
            if (isPaused)
                StartCoroutine(ResumeGame());
            else
                StartCoroutine(PauseGame());
        }
    }

    IEnumerator PauseGame()
    {
        isFading = true;
        // Mute SFX

        // Pause video playback if active
        VideoPlayer vp = FindObjectOfType<VideoPlayer>();
        if (vp != null && vp.isPlaying)
        {
            vp.Pause();
        }


        PlayableDirector pd = FindObjectOfType<PlayableDirector>();
        if (pd != null && pd.state == PlayState.Playing)
        {
            pd.Pause();
        }

        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;

        float elapsed = 0f;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        pauseMenu.alpha = 1f;

        audioMixer.SetFloat("SFXVolume", -80f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            pauseMenu.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        isFading = false;
    }

    public IEnumerator ResumeGame()
    {
        
        isFading = true;

        VideoPlayer vp = FindObjectOfType<VideoPlayer>();
        if (vp != null && !vp.isPlaying && !videoFinished)
        {
            vp.Play();
        }


        PlayableDirector pd = FindObjectOfType<PlayableDirector>();
        if (pd != null && pd.state == PlayState.Paused)
        {
            pd.Resume(); // Only available in Unity 2020+
        }


        Time.timeScale = 1f;

        float elapsed = 0f;
        pauseMenu.alpha = 0f;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            pauseMenu.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        isFading = false;
        ApplySettings();
    }

    public void OnResumeButton()
    {
        StartCoroutine(ResumeGame());
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();

        ApplySettings();
    }

    public void ApplySettings()
    {

        if (musicManager != null)
        {
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

            // Apply the settings to the AudioSource using the SetVolume method
            musicManager.SetMute(!musicEnabled);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume <= 0.0001f ? 0.0001f : musicVolume) * 20);
        }
        else
        {
            Debug.LogWarning("Background Music AudioSource is not assigned!");
        }

        // Check if musicToggle is assigned
        if (musicToggle != null)
        {
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            musicToggle.isOn = musicEnabled;
        }
        else
        {
            Debug.LogWarning("Music Toggle is not assigned!");
        }

        // Check if volumeSlider is assigned
        if (volumeSlider != null)
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            volumeSlider.value = musicVolume;
        }
        else
        {
            Debug.LogWarning("Volume Slider is not assigned!");
        }

        if (sfxSlider != null)
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            sfxSlider.value = sfxVolume;
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);
        }
        else
        {
            Debug.LogWarning("SFX Slider is not assigned!");
        }

        if (sensitivitySlider != null)
        {
            sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150f);
            sensitivitySlider.value = sensitivity;

            // Apply to PlayerCam immediately
            PlayerCam cam = FindObjectOfType<PlayerCam>();
            if (cam != null)
            {
                cam.SetSensitivity(sensitivity / 100f); // Scale here
            }
        }
    }

    public void CancelSettings()
    {
        StartCoroutine(RevertPauseSettings());
    }

    private IEnumerator RevertPauseSettings()
    {
        // Temporarily detach listeners
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        musicToggle.onValueChanged.RemoveListener(OnMuteToggled);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);

        // Load saved values
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150.0f);
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        // Reset UI
        volumeSlider.value = musicVolume;
        musicToggle.isOn = musicEnabled;
        sfxSlider.value = sfxVolume;
        sensitivitySlider.value = sensitivity;

        // Reset audio
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume <= 0.0001f ? 0.0001f : musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);
        if (musicManager != null)
        {
            musicManager.SetMute(!musicEnabled);
            musicManager.SetVolume(musicVolume);
        }

        // Reapply camera sensitivity
        PlayerCam cam = FindObjectOfType<PlayerCam>();
        if (cam != null)
        {
            cam.SetSensitivity(sensitivity / 100f);
        }

        // Reattach listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMuteToggled);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        yield return new WaitForSecondsRealtime(0.1f); // give Unity time to update

        StartCoroutine(ResumeGame());
    }



    void OnVolumeChanged(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume <= 0.0001f ? 0.0001f : volume) * 20);

        if (musicManager != null)
        {
            musicManager.SetVolume(volume); // Apply volume instantly
        }
    }


void OnSFXVolumeChanged(float volume)
{
    if (isPaused) return; // Don't update during pause menu

    audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume <= 0.0001f ? 0.0001f : volume) * 20);
}


    void OnMuteToggled(bool isNowOn)
    {
        bool shouldMute = !isNowOn;

        audioMixer.SetFloat("MusicVolume", shouldMute ? -80f :
            Mathf.Log10(volumeSlider.value <= 0.0001f ? 0.0001f : volumeSlider.value) * 20);

        if (musicManager != null)
        {
            musicManager.SetMute(shouldMute);
        }
    }



    void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    void OnSensitivityChanged(float value)
    {
        sensitivity = value;

        PlayerCam cam = FindObjectOfType<PlayerCam>();
        if (cam != null)
        {
            cam.SetSensitivity(value / 100f);
        }
    }

}
