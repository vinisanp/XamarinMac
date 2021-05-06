using System;
using System.Linq;
using Foundation;
using SDMobileXF;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(SDMobileXF.iOS.iOSNotificationManager))]
namespace SDMobileXF.iOS
{
    public class iOSNotificationManager : INotificationManager
    {
        int messageId = -1;

        bool hasNotificationsPermission;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            // request the permission to use local notifications
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                hasNotificationsPermission = approved;
            });
        }

        public void Clear()
        {
            UIApplication.SharedApplication.CancelAllLocalNotifications();
            UILocalNotification[] notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
            foreach (UILocalNotification notification in notifications)
                UIApplication.SharedApplication.CancelLocalNotification(notification);
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }

        public int ScheduleNotification(string title, string message)
        {
            // EARLY OUT: app doesn't have permissions
            if (!hasNotificationsPermission)
            {
                return -1;
            }

            this.messageId++;

            UNMutableNotificationContent content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1
            };

            // Local notifications can be time or location based
            // Create a time-based trigger, interval is in seconds and must be greater than 0
            UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);

            UNNotificationRequest request = UNNotificationRequest.FromIdentifier(this.messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
            });

            return this.messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            NotificationEventArgs args = new NotificationEventArgs()
            {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke(null, args);
        }
    }
}