using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(SDMobileXF.Droid.AndroidNotificationManager))]
namespace SDMobileXF.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        private bool channelInitialized = false;
        private int messageId = -1;
        private NotificationManager manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            this.CreateNotificationChannel();
        }

        public void Clear()
        {
            this.manager.CancelAll();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!this.channelInitialized)
            {
                this.CreateNotificationChannel();
            }

            this.messageId++;

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.IconeDNA))
                .SetSmallIcon(Resource.Drawable.IconeDNA)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            this.manager.Notify(this.messageId, notification);

            return this.messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            NotificationEventArgs args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            this.manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Java.Lang.String channelNameJava = new Java.Lang.String(channelName);
                NotificationChannel channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                this.manager.CreateNotificationChannel(channel);
            }

            this.channelInitialized = true;
        }
    }
}