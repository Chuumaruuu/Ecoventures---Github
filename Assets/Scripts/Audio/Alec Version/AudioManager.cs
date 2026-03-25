using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip _sfx)
    {
        if (_sfx == null) Debug.LogError("NoSFX Assigned");
        _sfxSource.PlayOneShot(_sfx);
    }

    public AudioSource PlayLoopedSFX(AudioClip _sfx)
    {
        if (_sfx == null)
        {
            Debug.LogError("No SFX Assigned");
            return null;
        }

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = _sfx;
        source.loop = true;
        source.Play();

        return source;
    }

    public void PlayMusic(AudioClip _music)
    {
        if (_music == null) Debug.LogError("NoMusic Assigned");

        _musicSource.clip = _music;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void StopLoopedSFX(AudioSource source)
    {
        if (source == null) return;

        source.Stop();
        Destroy(source); 
    }
}
