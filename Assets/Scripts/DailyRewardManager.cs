using System;
using UnityEngine;
using System.Globalization;

public class DailyRewardManager : MonoBehaviour
{
    
    [SerializeField] private int rewardAmount = 50; // The currency you get (change later)-----------------------------------------------
 
    [Header("Scripts")]
    [SerializeField] private NotificationsManager notificationsManager; 
    [SerializeField] private ProfileSelectionMenu profileSelectionMenu;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject rewardPanel; // UI pop-up screen (when daily)
    [SerializeField] private GameObject mainMenu;
    
    private string currentProfile;
    private readonly string timeFormat = "yyyy-MM-dd HH:mm:ss";

    public void CheckDailyRewardOnLogin(string profileName) //  Call from Profile Menu when the player selects and opens a save profile
    {
        currentProfile = profileName;
        SaveData data = SaveHandler.ReadFromJson(profileName); // read profile savefile
        
        //Debug.Log($"[DEBUG] Data loaded. LastClaimedTimeStr: '{data.lastClaimedTimeStr}' | Path check: {profileName}");
        
        if (string.IsNullOrEmpty(data.lastClaimedTimeStr)) // check if the save is not a new save, Give daily
        {
            //Debug.Log("[REWARD] New player detected. Initializing.");
            
            data.totalTungs = rewardAmount;
            data.lastClaimedTimeStr = DateTime.Now.ToString(timeFormat, CultureInfo.InvariantCulture);
            
            SaveHandler.SaveToJson(data, currentProfile);
            
            rewardPanel.SetActive(true);
            return;
        }
        
        DateTime lastClaimedTime = DateTime.ParseExact(data.lastClaimedTimeStr, timeFormat, CultureInfo.InvariantCulture);
        // make the string of when the player got their last reward and convert to DateTime
        
        TimeSpan timePassed = DateTime.Now - lastClaimedTime; // Calculate how much time passed since last claim

        /*
        Debug.Log($"Now: {DateTime.Now}");
        Debug.Log($"Last Claimed: {lastClaimedTime}");
        Debug.Log($"Hours Passed: {timePassed.TotalHours}");
        Debug.Log($"Saved string: {data.lastClaimedTimeStr}");
        */
        
        if (timePassed.TotalHours >= notificationsManager.dailyNotificationsTimer)
        {
            GrantReward(data); 
        }
        else
        {
            //Debug.Log("Too early for reward. Panel stays closed.");
            
            rewardPanel.SetActive(false);
            profileSelectionMenu.StartTheGame();
        }
    }

    private void GrantReward(SaveData data)
    {
        data.totalTungs += rewardAmount; // update currency
        data.lastClaimedTimeStr = DateTime.Now.ToString(timeFormat, CultureInfo.InvariantCulture); // save the current time as a string
        
        SaveHandler.SaveToJson(data, currentProfile); // write the update
        
        rewardPanel.SetActive(true);
    }
    
    public void OnClaimButtonClick()
    {
        rewardPanel.SetActive(false); // Hide the popup
        profileSelectionMenu.StartTheGame();
    }
}
