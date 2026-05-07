using UnityEngine;
using System.Collections;

public class ZombieAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] hurtSounds;
    public AudioClip[] neutralSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] attackSounds;

    [Header("Neutral Sound Settings")]
    public float minNeutralDelay = 3f;
    public float maxNeutralDelay = 8f;

    private bool isAlive = true;

    private void Start()
    {
        StartCoroutine(PlayNeutralSounds());
    }

    private IEnumerator PlayNeutralSounds()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(minNeutralDelay, maxNeutralDelay));
            PlayNeutralSound();
        }
    }

    public void PlayHurtSound()
    {
        PlayRandomSound(hurtSounds);
    }

    public void PlayNeutralSound()
    {
        PlayRandomSound(neutralSounds);
    }

    public void PlayDeathSound()
    {
        PlayRandomSound(deathSounds);
        isAlive = false; // Stop neutral groans once dead
    }

    public void PlayAttackSound()
    {
        PlayRandomSound(attackSounds);
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
