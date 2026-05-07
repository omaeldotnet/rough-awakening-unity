using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public VideoPlayer videoPlayer;
    public AudioSource videoAudioSource;
    public float cutsceneEndTime = 19f;
    public GameObject player;
    [SerializeField] public TextMeshProUGUI skipText;
    public GameObject PlayerStats;

    private bool cutscenePlaying = false;

    void Start()
    {
        // Always link audio output cleanly on startup
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, videoAudioSource);
        skipText.text = "Press Q to skip";
        PlayCutscene();

    }

    void Update()
    {
        if (cutscenePlaying && Input.GetKeyDown(KeyCode.Q))
        {
            SkipCutscene();

        }

        if (cutscenePlaying && videoPlayer.time >= cutsceneEndTime)
        {
            EndCutscene();
  
        }
    }

    public void PlayCutscene()
    {
        PlayerStats.gameObject.SetActive(false);
        cutscenePlaying = true;

cutsceneCamera.enabled = true;
mainCamera.enabled = false;

// Disable main camera AudioListener, enable cutscene camera AudioListener
cutsceneCamera.GetComponent<AudioListener>().enabled = true;
mainCamera.GetComponent<AudioListener>().enabled = false;


        videoPlayer.time = 0;
        videoAudioSource.time = 0;

        videoPlayer.Play();
        videoAudioSource.Play();
        

        Time.timeScale = 0f;
        player.SetActive(false);
    }

    public void SkipCutscene()
    {
        videoPlayer.time = cutsceneEndTime;
        videoAudioSource.time = cutsceneEndTime;
        skipText.gameObject.SetActive(false);
        PlayerStats.gameObject.SetActive(true);

        videoPlayer.Play();
        videoAudioSource.Play();
    }

    public void EndCutscene()
    {
        cutscenePlaying = false;
        skipText.gameObject.SetActive(false);
        PlayerStats.gameObject.SetActive(true);


cutsceneCamera.enabled = false;
mainCamera.enabled = true;

// Enable main camera AudioListener, disable cutscene camera AudioListener
cutsceneCamera.GetComponent<AudioListener>().enabled = false;
mainCamera.GetComponent<AudioListener>().enabled = true;


        Time.timeScale = 1f;
        player.SetActive(true);

        Object.Destroy(cutsceneCamera.gameObject); // Just the visual, not the audio!
    }
}
