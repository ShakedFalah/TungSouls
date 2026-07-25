using TMPro;
using UnityEngine;

public class ShopManager : SingletonPersistent<ShopManager>
{
    [SerializeField] private TextMeshProUGUI currencyText;
    
    public void TryPurchaseUpgrade(ShopCatalogSO catalog, string upgradeID)
    {
        UpgradeEntry entry = catalog.GetUpgradeByID(upgradeID);
        if (entry == null) return;

        string activeProfile = PlayerPrefs.GetString("LastSelectedProfile", "");
        if (string.IsNullOrEmpty(activeProfile)) return;

        SaveData data = SaveHandler.ReadFromJson(activeProfile);

        // Check if player can afford it
        if (data.totalTungs < entry.cost)
        {
            Debug.Log("Not enough Tungs!");
            return;
        }

        // Deduct currency
        data.totalTungs -= entry.cost;
        currencyText.text = $"Tungs: {data.totalTungs}$";

        // Initialize and add to purchased list
        if (data.purchasedUpgrades == null)
        {
            data.purchasedUpgrades = new System.Collections.Generic.List<string>();
        }

        if (!data.purchasedUpgrades.Contains(upgradeID))
        {
            data.purchasedUpgrades.Add(upgradeID);
        }

        // Apply stat upgrades based on ID
        ApplyStatUpgrade(upgradeID, data);

        // Save JSON file
        SaveHandler.SaveToJson(data, activeProfile);

        // Instantly refresh all button colors on screen
        UpgradeButtonUI[] allButtons = FindObjectsByType<UpgradeButtonUI>(FindObjectsSortMode.None);
        foreach (var btn in allButtons)
        {
            btn.RefreshButtonState();
        }
    }

    private void ApplyStatUpgrade(string upgradeID, SaveData data)
    {
        switch (upgradeID)
        {
            case "gem_1": data.invincibilityDuration += 2f; break;
            case "gem_2": data.invincibilityDuration += 3f; break;
            case "gem_3": data.multiplierValue += 1f; break;

            case "star_1": data.multiplierDuration += 2f; break;
            case "star_2": data.multiplierDuration += 3f; break;
            case "star_3": data.multiplierValue += 2f; break;

            case "mag_1": data.magnetDuration += 2f; break;
            case "mag_2": data.magnetDuration += 3f; break;
            case "mag_3": data.magnetDuration += 5f; break;
        }
    }
}