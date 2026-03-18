using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Tooltip("Assign your background music clip here")]
    public AudioClip musicClip;

    private AudioSource audioSource;

    void Awake()
    {
        // Add or get AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        // Start playing when the scene loads
        if (musicClip != null)
        {
            audioSource.Play();
        }
    }
}