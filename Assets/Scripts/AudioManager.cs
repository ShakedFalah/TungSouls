using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonPersistent<AudioManager>
{
    [SerializeField] private AudioMixer mixer;
    [Header("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip bgmClip;
    [Header("SFX")]
    [SerializeField] private string sfxTag = "SFXSource";

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
        sfxPlayer.PlaySound(audioClip, sfxPlayer.TriggerReturn);
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
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void UpdateSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
