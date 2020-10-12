using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using System;

public class NotificationHandler : MonoBehaviour
{
    [SerializeField] private string _notificationCategoryId;
    [SerializeField] private string _notifTitle;
    [SerializeField] private string _notifSubtitle;
    [SerializeField] private string _notifContent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ScheduleLocalNotification()
    {
        NotificationContent content = PrepareNotificationContent();
        TimeSpan delay = new TimeSpan(0, 0, 10);
        Notifications.ScheduleLocalNotification(delay, content);
        Debug.Log("Notification Scheduled with Title " + content.title);
    }


    private NotificationContent PrepareNotificationContent()
    {
        NotificationContent content = new NotificationContent();

        content.title = _notifTitle;
        content.subtitle = _notifSubtitle;
        content.title = _notifContent;

        content.categoryId = _notificationCategoryId;

        return content;
    }

}

    

