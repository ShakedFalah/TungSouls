using UnityEngine;

[CreateAssetMenu(fileName = "MagnetSettings", menuName = "Scriptable Objects/Powerups/MagnetSettings")]
public class MagnetSettings : PowerUpSettings
{
    public float pullSpeed;
    public float pullDistance;

    public override void Activate(PlayerController player)
    {
        player.StartMagnet(duration, pullDistance, pullSpeed);
    }
}
