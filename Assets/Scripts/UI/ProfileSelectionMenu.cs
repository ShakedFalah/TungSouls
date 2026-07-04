using UnityEngine;
using System.IO;

public class ProfileSelectionMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform slotsContainer; 
    [SerializeField] private ProfileSlotUI slotPrefab;

    [SerializeField] private ProfileSO _ProfileSO;
    void OnEnable()
    {
        PopulateProfileList();
    }

    private void PopulateProfileList()
    {
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }
        
        string[] saveDirectories = SaveHandler.GetSaveNames();
        
        Debug.Log("No profile saves found yet. Spawning creation slot.");
            
        ProfileSlotUI creationSlot = Instantiate(slotPrefab, slotsContainer);
            
        creationSlot.SetupSlot("Create New Profile", null, HandleCreateNewProfile);
        
        foreach (string dirName in saveDirectories)
        {
            ProfileSlotUI newSlot = Instantiate(slotPrefab, slotsContainer);
            
            Texture2D thumbnail = SaveHandler.LoadThumbnail(dirName);
            
            newSlot.SetupSlot(dirName, thumbnail, OnProfileSelected);
        }
    }

    private void OnProfileSelected(string chosenDirectory)
    {
        Debug.Log($"Profile selected: {chosenDirectory}!");
        
        _ProfileSO.profileName = chosenDirectory;
        _ProfileSO.iSProfileLoaded = true;
        
        MainMenuHUD mainHUD = Object.FindFirstObjectByType<MainMenuHUD>();
        if (mainHUD != null)
        {
            mainHUD.StartLoadedGame();
        }
    }

    private void HandleCreateNewProfile(string placeholderName)
    {
        _ProfileSO.profileName = placeholderName;
        _ProfileSO.iSProfileLoaded = false;
        
        Debug.Log("User clicked to create a brand new save profile slot.");
        
        string newProfileName = "Profile_1";
        
        PopulateProfileList();
    }
}