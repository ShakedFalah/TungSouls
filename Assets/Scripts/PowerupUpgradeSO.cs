using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupUpgradeSO", menuName = "Scriptable Objects/PowerupUpgradeSO")]
public class PowerupUpgradeSO : ScriptableObject
{
    public PowerUpSettings newSettings;

    public List<UpgradeEffectSO> effects;
}
