using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    private AudioSource sfxSource; // for oneshots
    private AudioSource loopSource; // for continuous sounds

    [Header("SFX Clips")]
    public AudioClip pickupClip;
    public AudioClip dropClip;
    // public AudioClip buttonClickClip;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // Create audio sources
        sfxSource = gameObject.AddComponent<AudioSource>();
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
    }

    // Play a one-shot sound
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Start a looping sound
    public void PlayLoop(AudioClip clip)
    {
        if (clip != null)
        {
            loopSource.clip = clip;
            loopSource.Play();
        }
    }

    // Stop looping sound
    public void StopLoop()
    {
        loopSource.Stop();
    }
}