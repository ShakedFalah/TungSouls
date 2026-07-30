using System;
using UnityEngine;
using Unity.Notifications.Android;
using System.Collections;
using System.Globalization;

public class NotificationsManager : MonoBehaviour
{
    [SerializeField] private float dailyNotificationsTimer = 24; // daily timer, changeable in the inspector
    
    public float DailyNotificationsTimer => dailyNotificationsTimer; // allows other scripts to read it
    
    private const string ChannelId = "Daily Triple T Notification";
    private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

    void Start()
    {
        StartCoroutine(RequestNotificationPermission()); // request permission for notification
        CallAndroidChannel();
    }
    
    private IEnumerator RequestNotificationPermission() // handles Android notification permission
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
        {
            yield return null;
        }
    }
    
    private void CallAndroidChannel() // android channel required for it to work
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = ChannelId,
            Name = "Daily Reward",
            Importance = Importance.High,
            Description = "Log in to claim" 
        };
        
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    void OnApplicationPause(bool isPaused)
    {
        if (isPaused) //if player closed the app.
        {
            ScheduleDailyNotification();
        }
        else
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications(); // cancel notification upon opening the game
        }
    }

    private void ScheduleDailyNotification()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        
        DateTime targetNotificationTime = DateTime.Now.AddHours(dailyNotificationsTimer); // setting up the timer

        string currentProfile = PlayerPrefs.GetString("SelectedProfile", "");
        if (!string.IsNullOrEmpty(currentProfile))
        {
            SaveData data = SaveHandler.ReadFromJson(currentProfile);
            if (data != null && !string.IsNullOrEmpty(data.lastClaimedTimeStr))
            {
                if (DateTime.TryParseExact(data.lastClaimedTimeStr, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastClaimed))
                {
                    targetNotificationTime = lastClaimed.AddHours(dailyNotificationsTimer);
                }
            }
        }
        
        if (targetNotificationTime > DateTime.Now)
        {
            AndroidNotification dailyMessage = new AndroidNotification(
                "TUNG TUNG TUNG SAHUR!",
                "PLAY TUNG SOULS NOW TO CLAIM UR DAILY TUNGS!!!!",
                targetNotificationTime
            );

            AndroidNotificationCenter.SendNotification(dailyMessage, ChannelId);
        }
    }
    
}
