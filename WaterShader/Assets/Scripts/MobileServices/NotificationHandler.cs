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

    // Update is called once per frame
    void Update()
    {
        
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

    public static void ScheduleNotification(string title, string content, float minUntilScheduled)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.SmallIcon = "notif_icon_small";
        notification.LargeIcon = "notif_icon_large";
        notification.FireTime = System.DateTime.Now.AddMinutes(minUntilScheduled);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

        Debug.Log("Notification scheduled");
    }

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
