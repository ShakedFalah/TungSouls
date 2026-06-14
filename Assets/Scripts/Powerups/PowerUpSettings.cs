using UnityEngine;

public abstract class PowerUpSettings : ScriptableObject
{
    public readonly float duration;

    public abstract void Activate(PlayerController player);
}
