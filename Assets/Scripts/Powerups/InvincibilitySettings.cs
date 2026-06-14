using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilitySettings", menuName = "Scriptable Objects/Powerups/InvincibilitySettings")]
public class InvincibilitySettings : PowerUpSettings
{
    public override void Activate(PlayerController player)
    {
        player.StartInvinciblity(duration);
    }
}
