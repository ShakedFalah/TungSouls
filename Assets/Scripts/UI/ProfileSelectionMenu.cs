using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ProfileSelectionMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform slotsContainer; 
    [SerializeField] private ProfileSlotUI slotPrefab;
    
    [SerializeField] private DailyRewardManager dailyRewardManager;
    [SerializeField] private ProfileSO _ProfileSO;
    private string nextSave = "";
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
        if (saveDirectories != null && saveDirectories.Length > 0)
        {

            Array.Sort(saveDirectories);

            int maxNumber = saveDirectories.Select(s =>
            {
                Match match = Regex.Match(s, @"^Save(\d+)$");
                return match.Success ? int.Parse(match.Groups[1].Value) : 0;
            }).Max();

            nextSave = $"Save{maxNumber + 1}";

            foreach (string dirName in saveDirectories)
            {
                ProfileSlotUI newSlot = Instantiate(slotPrefab, slotsContainer);

                Texture2D thumbnail = SaveHandler.LoadThumbnail(dirName);

                newSlot.SetupSlot(dirName, thumbnail, OnProfileSelected);
            }
        } else
        {
            nextSave = "Save1";
        }

        ProfileSlotUI creationSlot = Instantiate(slotPrefab, slotsContainer);

        creationSlot.SetupSlot("New Profile", null, HandleCreateNewProfile);
    }

    private void OnProfileSelected(string chosenDirectory)
    {
        Debug.Log($"Profile selected: {chosenDirectory}!");
        
        _ProfileSO.profileName = chosenDirectory;
        _ProfileSO.iSProfileLoaded = true;

        if (dailyRewardManager != null)
        {
            dailyRewardManager.CheckDailyRewardOnLogin(chosenDirectory);
        }
        else
        {
            Debug.Log("daily reward refrence is missing!");
            
        }
    }

    private void HandleCreateNewProfile(string placeholderName)
    {
        _ProfileSO.profileName = nextSave;
        _ProfileSO.iSProfileLoaded = false;

        if (dailyRewardManager != null)
        {
            dailyRewardManager.CheckDailyRewardOnLogin(nextSave);
        }
        
        //Debug.Log("User clicked to create a brand new save profile slot.");
    }

    public void StartTheGame()
    {
        MainMenuHUD mainHUD = UnityEngine.Object.FindFirstObjectByType<MainMenuHUD>();
        if (mainHUD != null)
        {
            mainHUD.StartLoadedGame();
        }
    }
}