using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilitySettings", menuName = "Scriptable Objects/InvincibilitySettings")]
public class InvincibilitySettings : PowerUpSettings
{
    public override void Activate(PlayerController player)
    {
        player.StartInvinciblity(duration);
    }
}
