using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.UnityConsent; // Required for consent state

public class AnalyticsManager : MonoBehaviour
{
    [SerializeField] private GameObject consentPopupPanel;

    void Start()
    {
        // Check if the user has already made a privacy choice in the past
        if (!PlayerPrefs.HasKey("PrivacyChoiceMade"))
        {
            ShowConsentPopup();
        }
        else
        {
            // Proceed normally with saved choice
            bool totalConsent = PlayerPrefs.GetInt("DataTrackingConsent") == 1;
            InitializeUnityServices(totalConsent);
        }
    }

    void ShowConsentPopup()
    {
        consentPopupPanel.SetActive(true);
    }

    // Triggered by the "Accept" Button
    public void OnUserAccepts()
    {
        PlayerPrefs.SetInt("PrivacyChoiceMade", 1);
        PlayerPrefs.SetInt("DataTrackingConsent", 1);
        consentPopupPanel.SetActive(false);

        InitializeUnityServices(true);
    }

    // Triggered by the "Decline" Button
    public void OnUserDeclines()
    {
        PlayerPrefs.SetInt("PrivacyChoiceMade", 1);
        PlayerPrefs.SetInt("DataTrackingConsent", 0);
        consentPopupPanel.SetActive(false);

        InitializeUnityServices(false);
    }

    async void InitializeUnityServices(bool consentGranted)
    {
        ConsentState consentState = EndUserConsent.GetConsentState();

        consentState.AnalyticsIntent = consentGranted
            ? ConsentStatus.Granted
            : ConsentStatus.Denied;

        consentState.AdsIntent = consentGranted
            ? ConsentStatus.Granted
            : ConsentStatus.Denied;

        EndUserConsent.SetConsentState(consentState);

        await UnityServices.InitializeAsync();
    }
}
