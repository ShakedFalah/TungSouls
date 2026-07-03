using UnityEngine;
using System.IO;

public class ProfileSelectionMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform slotsContainer; 
    [SerializeField] private ProfileSlotUI slotPrefab; 

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

        if (saveDirectories == null || saveDirectories.Length == 0)
        {
            Debug.Log("No profile saves found yet. Spawning creation slot.");
            
            ProfileSlotUI creationSlot = Instantiate(slotPrefab, slotsContainer);
            
            creationSlot.SetupSlot("Create New Profile", null, HandleCreateNewProfile);
            return;
        }
        
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
        
        SaveData loadedSession = SaveHandler.ReadFromJson(chosenDirectory);
        
        MainMenuHUD mainHUD = Object.FindFirstObjectByType<MainMenuHUD>();
        if (mainHUD != null)
        {
            mainHUD.StartLoadedGame(loadedSession);
        }
    }

    private void HandleCreateNewProfile(string placeholderName)
    {
        Debug.Log("User clicked to create a brand new save profile slot.");
        
        string newProfileName = "Profile_1";

        SaveData newGameSession = new SaveData(); 
        
        SaveHandler.SaveToJson(newGameSession, newProfileName);
        
        PopulateProfileList();
    }
}