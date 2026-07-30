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

        // reduce currency
        data.totalTungs -= entry.cost;
        currencyText.text = $"Tungs: {data.totalTungs}$";

        // if there is no purchased list, create one
        if (data.purchasedUpgrades == null)
        {
            data.purchasedUpgrades = new System.Collections.Generic.List<string>();
        }

        // add to purchased list
        if (!data.purchasedUpgrades.Contains(upgradeID))
        {
            data.purchasedUpgrades.Add(upgradeID);
        }

        
        //ApplyStatUpgrade(upgradeID, data);

        // save JSON file
        SaveHandler.SaveToJson(data, activeProfile);

        // refresh all button colors on screen
        UpgradeButtonUI[] allButtons = FindObjectsByType<UpgradeButtonUI>(FindObjectsSortMode.None);
        foreach (var btn in allButtons)
        {
            btn.RefreshButtonState();
        }
    }

    // we ended up not using this
    /*
    private void ApplyStatUpgrade(string upgradeID, SaveData data)
    {
        switch (upgradeID)
        {
            case "gem_1": data.invincibilityDuration += 0f; break;
            case "gem_2": data.invincibilityDuration += 0f; break;
            case "gem_3": data.multiplierValue += 0f; break;

            case "star_1": data.multiplierDuration += 0f; break;
            case "star_2": data.multiplierDuration += 0f; break;
            case "star_3": data.multiplierValue += 0f; break;

            case "mag_1": data.magnetDuration += 0f; break;
            case "mag_2": data.magnetDuration += 0f; break;
            case "mag_3": data.magnetDuration += 0f; break;
        }
    }
    */
}