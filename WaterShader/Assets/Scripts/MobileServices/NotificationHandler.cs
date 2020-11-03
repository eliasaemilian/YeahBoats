using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitAndroid();

        CheckIfOpenedThroughNotification();

        ScheduleNotification("Test", "blablablabla", .1f);
    }


    private void InitAndroid()
    {
        // Setup Channel
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    /// <summary>
    /// Schedules a new notification to be delivered at a given Time
    /// </summary>
    /// <param name="title">Notification Title</param>
    /// <param name="content">Notification Description Text</param>
    /// <param name="minUntilScheduled">Minutes from now until notification should be send</param>
    public static void ScheduleNotification(string title, string content, float minUntilScheduled)
    {
        if (SettingsHandler.RequestSetting(SettingsHandler.Notif, out bool notifOnOff))
        {
            if (!notifOnOff) return;
            //TODO: ALSO CANCEL NOTIFICATIONS THAT HAVE BEEN SCHEDULED IF SETTING GETS CHANGED
        }

        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.SmallIcon = "notif_icon_small";
        notification.LargeIcon = "notif_icon_large";
        notification.FireTime = System.DateTime.Now.AddMinutes(minUntilScheduled);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

        Debug.Log("Notification scheduled");
    }

    /// <summary>
    /// If the App was opened through clicking on a notification
    /// DO x
    /// </summary>
    private void CheckIfOpenedThroughNotification()
    {
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            var id = notificationIntentData.Id;
            var channel = notificationIntentData.Channel;
            var notification = notificationIntentData.Notification;

            Debug.Log("Application got opened through Notification");
        }
    }
}
