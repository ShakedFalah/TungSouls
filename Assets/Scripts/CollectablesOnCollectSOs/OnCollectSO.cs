using UnityEngine;

[CreateAssetMenu(fileName = "OnCollectSO", menuName = "Scriptable Objects/OnCollect/OnCollectSO")]
public abstract class OnCollectSO : ScriptableObject
{
    public abstract void OnCollect(PlayerController player);
    public AudioClip collectSound;
}
