using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UpgradesKeyObject
{
    public string key;
    public List<PowerupUpgradeSO> upgrades;
}


[CreateAssetMenu(fileName = "MetaUpgradesSO", menuName = "Scriptable Objects/MetaUpgradesSO")]
public class MetaUpgradesSO : ScriptableObject
{
    public List<UpgradesKeyObject> upgradesList = new List<UpgradesKeyObject>();

    public PowerupUpgradeSO GetActivePowerupUpgrade(string name, int index)
    {
        // Find the matching upgrades list
        UpgradesKeyObject upgradeEntry = upgradesList.Find(x => x.key == name);

        // Check if it was found
        if (upgradeEntry.upgrades == null)
        {
            Debug.LogWarning($"No upgrade list found for '{name}'.");
            return null;
        }

        // Check index bounds
        if (index < 0 || index >= upgradeEntry.upgrades.Count)
        {
            Debug.LogWarning($"Upgrade index {index} is out of range for '{name}'.");
            return null;
        }

        return upgradeEntry.upgrades[index];
    }
}
