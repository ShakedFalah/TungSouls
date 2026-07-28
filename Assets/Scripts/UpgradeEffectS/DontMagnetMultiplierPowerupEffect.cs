using UnityEngine;

[CreateAssetMenu(fileName = "DontMagnetMultiplierPowerupEffect", menuName = "Scriptable Objects/Upgrade Effects/DontMagnetMultiplierPowerupEffect")]
public class DontMagnetMultiplierPowerupEffect : UpgradeEffectSO
{
    public override void Apply(GameManager game)
    {
        game.SetMultiplierMagnetable(false);
    }
}

