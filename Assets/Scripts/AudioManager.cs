using UnityEngine;

public class AudioManager : SingletonPersistent<AudioManager>
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip bgmClip;
    [Header("SFX")]
    [SerializeField] private string sfxTag = "SFXSource";
    private float sfxVolume;

    private void Start()
    {
        SettingsManager.Instance.onMusicVolumeChanged += UpdateMusicVolume;
        SettingsManager.Instance.onSFXVolumeChanged += UpdateSFXVolume;
        PlayMusic();
    }

    public void PlaySound(AudioClip audioClip, Transform parent = null)
    {
        PlaySFXPool sfxPlayer = TaggedObjectPooler.Instance.GetPooledObjectWithAutoReturn(sfxTag).GetComponent<PlaySFXPool>();
        if (parent != null)
        {
            sfxPlayer.transform.SetParent(transform);
        }
        sfxPlayer.PlaySound(audioClip, sfxVolume, sfxPlayer.TriggerReturn);
    }

    public void PlayMusic()
    {
        if (bgmAudioSource == null || bgmClip == null)
        {
            Debug.LogWarning("No audio source or clip for bgm");
            return;
        }

        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.Play();
    }

    public void UpdateMusicVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void UpdateSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
