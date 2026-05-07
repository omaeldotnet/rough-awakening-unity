using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;
    public Slider sfxSlider;
    public Slider sensitivitySlider;
    public AudioMixer audioMixer;

    private MusicManager musicManager;
    private bool isLoadingSettings = false;

    void Start()
    {
        musicManager = MusicManager.Instance;

        ApplyInitialSettings();

        // Add live preview listeners (no saving here)
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMuteToggled);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }

    private void ApplyInitialSettings()
    {
        isLoadingSettings = true;

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150.0f);
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        musicToggle.isOn = musicEnabled;
        volumeSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        sensitivitySlider.value = sensitivity;

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume <= 0.0001f ? 0.0001f : musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);

        if (musicManager != null)
        {
            musicManager.SetMute(!musicEnabled);
            musicManager.SetVolume(musicVolume);
        }   

        isLoadingSettings = false;
    }


    public void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();

        Debug.Log("Settings saved.");
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelSettings()
    {
        StartCoroutine(RevertAndExit());
    }

    private IEnumerator RevertAndExit()
    {
        isLoadingSettings = true;

        // Remove listeners temporarily so they don't save when values are changed
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        musicToggle.onValueChanged.RemoveListener(OnMuteToggled);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);

        // Load saved PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150.0f);
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        // Apply to UI
        musicToggle.isOn = musicEnabled;
        volumeSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        sensitivitySlider.value = sensitivity;

        // Apply to Audio
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume <= 0.0001f ? 0.0001f : musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);
        if (musicManager != null)
        {
            musicManager.SetMute(!musicEnabled);
            musicManager.SetVolume(musicVolume);
        }

        // Delay for UI/audio to update properly
        yield return new WaitForSeconds(0.1f);

        // Reattach listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMuteToggled);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        isLoadingSettings = false;

        // Now load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    void OnVolumeChanged(float volume)
    {
        if (isLoadingSettings) return;

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume <= 0.0001f ? 0.0001f : volume) * 20);
        if (musicManager != null)
            musicManager.SetVolume(volume);
    }

    void OnSFXVolumeChanged(float volume)
    {
        if (isLoadingSettings) return;

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume <= 0.0001f ? 0.0001f : volume) * 20);
    }

    void OnMuteToggled(bool isNowOn)
    {
        if (isLoadingSettings) return;

        bool shouldMute = !isNowOn;
        float volume = Mathf.Log10(volumeSlider.value <= 0.0001f ? 0.0001f : volumeSlider.value) * 20;

        audioMixer.SetFloat("MusicVolume", shouldMute ? -80f : volume);
        if (musicManager != null)
            musicManager.SetMute(shouldMute);
    }

    void OnSensitivityChanged(float value)
    {
        if (isLoadingSettings) return;
        Debug.Log("Sensitivity changed to: " + value);
        // You could apply this to a player controller if needed
    }
}
