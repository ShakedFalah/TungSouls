using System;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationsManager : MonoBehaviour
{
    public int dailyNotificationsTimer = 24;
    
    private const string ChannelId = "Daily Triple T Notification";

    void Start()
    {
        CallAndroidChannel();
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
    
    void WhenAppOpen(bool isOpen)
    {
        if (!isOpen) //if player closed the app.
        {
            ScheduleDailyNotification();
        }
        else
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications(); // change later, removes all notification if player opened the game-----------------------------------
        }
    }

    private void ScheduleDailyNotification()
    {
        DateTime quitTime = DateTime.Now.AddHours(dailyNotificationsTimer); // setting up the timer

        AndroidNotification dailyMessage = new AndroidNotification(
            "TUNG TUNG TUNG SAHUR!", "PLAY TUNG SOULS NOW TO CLAIM UR DAILY TUNGS!!!!", quitTime); // using constructor to create a message
        
        AndroidNotificationCenter.SendNotification(dailyMessage, ChannelId);
    }
    
}
