using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierSettings", menuName = "Scriptable Objects/Powerups/MultiplierSettings")]
public class MultiplierSettings : PowerUpSettings
{
    public float multiplierValue;

    public override void Activate(PlayerController player)
    {
        player.StartMultiplier(duration, multiplierValue);
    }
}
