using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(SDMobileXF.UWP.UWPNotificationManager))]
namespace SDMobileXF.UWP
{
    public class UWPNotificationManager : INotificationManager
    {
        int messageId = -1;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
        }

        public int ScheduleNotification(string title, string message)
        {
            messageId++;
            string TOAST = $@"<toast>
                  <visual>
                    <binding template='ToastGeneric'>
                      <text>{title}</text>
                      <text>{message}</text>
                    </binding>
                  </visual>
                  <audio src =""ms-winsoundevent:Notification.Mail"" loop=""true""/>
                </toast>";

            Windows.Data.Xml.Dom.XmlDocument xml = new Windows.Data.Xml.Dom.XmlDocument();
            xml.LoadXml(TOAST);

            try
            {
                this.Initialize();

                ScheduledToastNotification toast = new ScheduledToastNotification(xml, DateTime.Now.AddSeconds(1));
                toast.Id = "IdTostone" + messageId.ToString();
                toast.Tag = messageId.ToString();
                toast.Group = nameof(UWPNotificationManager);
                App.Manager.AddToSchedule(toast);
            }
            catch (Exception ex)
            {

            }
            return messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void Clear()
        {
        }

        ////public int ScheduleNotification(string title, string message, DateTime scheduledTime)
        //public int ScheduleNotification(string title, string message)
        //{
        //    messageId++;
        //    string TOAST = $@"<toast>
        //          <visual>
        //            <binding template='ToastGeneric'>
        //              <text>{title}</text>
        //              <text>{message}</text>
        //            </binding>
        //          </visual>
        //          <audio src =""ms-winsoundevent:Notification.Mail"" loop=""true""/>
        //        </toast>";

        //    Windows.Data.Xml.Dom.XmlDocument xml = new Windows.Data.Xml.Dom.XmlDocument();
        //    xml.LoadXml(TOAST);

        //    try
        //    {
        //        this.Initialize();

        //        ToastNotification toast = new ToastNotification(xml);
        //        toast.RemoteId = "IdTostone" + messageId.ToString();
        //        toast.Tag = "NotificationOne";
        //        toast.Activated += this.Toast_Activated;
        //        toast.Dismissed += this.Toast_Dismissed;
        //        this.manager.Show(toast);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return messageId;
        //}

        //private void Toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        //{
        //}

        //private void Toast_Activated(ToastNotification sender, object args)
        //{
        //}
    }
}
