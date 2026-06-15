using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip bgmClip;
    [Header("SFX")]
    [SerializeField] private string sfxTag = "SFXSource";

    private void Start()
    {
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
}
