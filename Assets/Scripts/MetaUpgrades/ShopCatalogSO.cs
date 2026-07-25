using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeEntry
{
    public string upgradeID;     
    public int cost = 1000;
    public string prerequisiteID; // Previous item that unlocks the current one, by ID
}

[CreateAssetMenu(fileName = "ShopCatalog", menuName = "Shop/Shop Catalog")]
public class ShopCatalogSO : ScriptableObject
{
    public List<UpgradeEntry> allUpgrades = new List<UpgradeEntry>();

    public UpgradeEntry GetUpgradeByID(string id)
    {
        return allUpgrades.Find(u => u.upgradeID == id);
    }

    public bool IsPurchased(string upgradeID, SaveData saveData)
    {
        if (saveData.purchasedUpgrades == null) return false;
        return saveData.purchasedUpgrades.Contains(upgradeID);
    }

    public bool IsLocked(string upgradeID, SaveData saveData)
    {
        UpgradeEntry entry = GetUpgradeByID(upgradeID);
        if (entry == null) return true;

        // Unlocks if it's the first upgrade in queue 
        if (string.IsNullOrEmpty(entry.prerequisiteID)) return false;

        // Locked if the player hasn't bought the previous upgrade yet
        return !IsPurchased(entry.prerequisiteID, saveData);
    }
}