using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class EndingTrigger : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup endCanvasGroup;
    public Image endImage;
    public Button quitButton;

    [Header("Audio")]
    public AudioMixer masterMixer; // Should have a "MasterVolume" exposed

    [Header("Settings")]
    public float fadeDuration = 2f;

    [Header("Gameplay")]
    public MonoBehaviour[] componentsToDisable; // Add PlayerMovement, MouseLook, etc.

    private bool hasEnded = false;
    private Transform imageTransform;

    private void Start()
    {
        if (endCanvasGroup != null)
        {
            endCanvasGroup.alpha = 0;
            endCanvasGroup.interactable = false;
            endCanvasGroup.blocksRaycasts = false;
        }

        if (quitButton != null)
            quitButton.gameObject.SetActive(false);

        if (endImage != null)
            imageTransform = endImage.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasEnded && other.CompareTag("Player"))
        {
            hasEnded = true;
            StartCoroutine(FadeAndEnd());
        }
    }

private IEnumerator FadeAndEnd()
{
    // Disable gameplay control
    foreach (var comp in componentsToDisable)
    {
        if (comp != null)
            comp.enabled = false;
    }

    // Mute master volume
    if (masterMixer != null)
        masterMixer.SetFloat("MasterVolume", -80f);

    // Unlock and show mouse
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Ensure image is visible
    if (endImage != null)
    {
        var color = endImage.color;
        color.a = 0f;
        endImage.color = color;
        imageTransform.localScale = Vector3.one;
    }

    float timer = 0f;
Vector3 startScale = Vector3.one;
Vector3 endScale = new Vector3(5f, 5f, 1f);

while (timer < fadeDuration)
{
    float t = timer / fadeDuration;

    if (endImage != null)
    {
        var color = endImage.color;
        color.a = t;
        endImage.color = color;
    }

    if (imageTransform != null)
        imageTransform.localScale = Vector3.Lerp(startScale, endScale, t);

    if (endCanvasGroup != null)
        endCanvasGroup.alpha = t;

    timer += Time.unscaledDeltaTime;
    yield return null;
}

// Final state
imageTransform.localScale = endScale;
Time.timeScale = 0f;


    // Final UI state
    if (endCanvasGroup != null)
    {
        endCanvasGroup.alpha = 1f;
        endCanvasGroup.interactable = true;
        endCanvasGroup.blocksRaycasts = true;
    }

    if (quitButton != null)
        quitButton.gameObject.SetActive(true);
}

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

}
