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

    public override void Awake()
    {
        base.Awake();

        if (this != Instance)
        {
            return;
        }

        SettingsManager.Instance.onMusicVolumeChanged += UpdateMusicVolume;
        SettingsManager.Instance.onSFXVolumeChanged += UpdateSFXVolume;
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.onMusicVolumeChanged -= UpdateMusicVolume;
        SettingsManager.Instance.onSFXVolumeChanged -= UpdateSFXVolume;
    }
    private void Start()
    {
        UpdateMusicVolume(SettingsManager.Instance.settings.musicVolume);
        UpdateSFXVolume(SettingsManager.Instance.settings.sfxVolume);

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
        float volumeOut;
        bool success = mixer.GetFloat("Music", out volumeOut);

        Debug.Log($"{success}: the volume is {volumeOut}: before set to {volume}");
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        success = mixer.GetFloat("Music", out volumeOut);
        Debug.Log($"{success}: the volume is {volumeOut}: after set to {volume}");

    }

    public void UpdateSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
