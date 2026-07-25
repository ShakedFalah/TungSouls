using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [Header("Catalog Reference")]
    [SerializeField] private ShopCatalogSO catalog;
    [SerializeField] private string targetUpgradeID; // Type the unique ID in the Inspector

    [Header("Color States")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color redCantAfford = new Color(0.9f, 0.3f, 0.3f);
    [SerializeField] private Color grayLocked = new Color(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color whiteAvailable = Color.white;
    [SerializeField] private Color greenPurchased = new Color(0.3f, 0.9f, 0.3f);

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (buttonImage == null) buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
        RefreshButtonState();
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    public void RefreshButtonState()
    {
        if (catalog == null || string.IsNullOrEmpty(targetUpgradeID)) return;

        UpgradeEntry entry = catalog.GetUpgradeByID(targetUpgradeID);
        if (entry == null) return;

        string activeProfile = PlayerPrefs.GetString("LastSelectedProfile", "");
        SaveData data = SaveHandler.ReadFromJson(activeProfile);

        bool isPurchased = catalog.IsPurchased(targetUpgradeID, data);
        bool isLocked = catalog.IsLocked(targetUpgradeID, data);
        bool canAfford = data.totalTungs >= entry.cost;

        if (isPurchased)
        {
            buttonImage.color = greenPurchased;
            button.interactable = false;
        }
        else if (isLocked)
        {
            buttonImage.color = grayLocked;
            button.interactable = false;
        }
        else if (!canAfford)
        {
            buttonImage.color = redCantAfford;
            button.interactable = true;
        }
        else
        {
            buttonImage.color = whiteAvailable;
            button.interactable = true;
        }
    }

    private void OnButtonClicked()
    {
        if (catalog != null)
        {
            ShopManager.Instance.TryPurchaseUpgrade(catalog, targetUpgradeID);
        }
    }
}