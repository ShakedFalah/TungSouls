using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public string sfxTag = "SFXSource";
    public void PlaySound(AudioClip audioClip, Transform parent = null)
    {
        PlaySFXPool sfxPlayer = TaggedObjectPooler.Instance.GetPooledObjectWithAutoReturn(sfxTag).GetComponent<PlaySFXPool>();
        if (parent != null)
        {
            sfxPlayer.transform.SetParent(transform);
        }
        sfxPlayer.PlaySound(audioClip, sfxPlayer.TriggerReturn);
    }
}
