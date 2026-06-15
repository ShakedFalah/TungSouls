using UnityEngine;

[CreateAssetMenu(fileName = "MagnetSettings", menuName = "Scriptable Objects/Collectable/MagnetSettings")]
public class MagnetSettings : PowerUpSettings
{
    public float pullSpeed;
    public float pullDistance;

    public override void OnCollect(PlayerController player)
    {
        player.StartMagnet(duration, pullDistance, pullSpeed);
    }
}
