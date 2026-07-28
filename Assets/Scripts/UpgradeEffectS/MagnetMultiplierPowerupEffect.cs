using UnityEngine;

[CreateAssetMenu(fileName = "MagnetMultiplierPowerupEffect", menuName = "Scriptable Objects/Upgrade Effects/MagnetMultiplierPowerupEffect")]
public class MagnetMultiplierPowerupEffect : UpgradeEffectSO
{
    public override void Apply(GameManager game)
    {
        game.SetMultiplierMagnetable(true);
    }
}

