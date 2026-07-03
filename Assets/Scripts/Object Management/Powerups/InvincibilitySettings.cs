using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilitySettings", menuName = "Scriptable Objects/Collectable/InvincibilitySettings")]
public class InvincibilitySettings : PowerUpSettings
{
    public override void OnCollect(PlayerController player)
    {
        player.StartInvinciblity(duration);
    }
}
