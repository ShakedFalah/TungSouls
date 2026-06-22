using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierSettings", menuName = "Scriptable Objects/Collectable/MultiplierSettings")]
public class MultiplierSettings : PowerUpSettings
{
    public float multiplierValue;

    public override void OnCollect(PlayerController player)
    {
        player.StartMultiplier(duration);
    }
}
