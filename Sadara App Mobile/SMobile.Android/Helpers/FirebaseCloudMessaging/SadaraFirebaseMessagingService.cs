using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Android.Media;
using Android.Support.V4.App;

namespace SMobile.Android.Helpers
{
    [Service]
    [IntentFilter(new string[] { "com.google.firebase.MESSAGING_EVENT" })]
    class SadaraFirebaseMessagingService : FirebaseMessagingService
    {

        public override void OnMessageReceived(RemoteMessage message)
        {

            base.OnMessageReceived(message);

            this.SendNotification(message.GetNotification().Title, message.GetNotification().Body);

        }

        private Notification BuilderNotification(string title, string body, RingtoneType defaultSoundUri, PendingIntent pendingIntent)
        {

            return new NotificationCompat.Builder(this)

                .SetSmallIcon(Resource.Drawable.ic_isotipo_sadara)

                .SetContentTitle(title)

                .SetContentText(body)

                .SetSmallIcon(Resource.Drawable.ic_isotipo_sadara_ico)

                .SetAutoCancel(true)

                .SetSound(RingtoneManager.GetDefaultUri(defaultSoundUri))

                .SetContentIntent(pendingIntent)

                .Build();

        }

        private void SendNotification(string title, string body)
        {

            var intent = new Intent(this, typeof(MainActivity));

            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0 , intent, PendingIntentFlags.OneShot);
            
            var notificationManager = NotificationManager.FromContext(this);

            notificationManager
                .Notify(0, this.BuilderNotification(title, body, RingtoneType.Notification, pendingIntent));

        }
        
    }

}