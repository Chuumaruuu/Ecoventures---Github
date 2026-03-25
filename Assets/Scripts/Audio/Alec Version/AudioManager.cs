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

        _sfxSource.clip = _sfx;
        _sfxSource.loop = true;
        _sfxSource.Play();

        return _sfxSource;
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
    }

    public AudioSource GetSFXSource()
    {
        return _sfxSource;
    }

    public AudioSource GetMusicSource()
    {
        return _musicSource;
    }
}
