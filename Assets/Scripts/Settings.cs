using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject consentPopupPanel;

    public void OpenConsentSettings()
    {
        consentPopupPanel.SetActive(true);
    }
}
