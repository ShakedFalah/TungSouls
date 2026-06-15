using UnityEngine;

public abstract class CollectableSettings : ScriptableObject
{
    public AudioClip collectSound;
    public abstract void OnCollect(PlayerController player);
}
