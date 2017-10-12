using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Firebase.Messaging;
using Android.Media;
using Android.Support.V4.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Firebase.Storage;

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
            
            var builder = new NotificationCompat.Builder(this)

                .SetSmallIcon(Resource.Drawable.ic_isotipo_sadara_ico)

                .SetContentTitle(title)

                .SetContentText(body)

                .SetStyle(this.SetImageToNotification())

                .SetAutoCancel(true)

                .SetSound(RingtoneManager.GetDefaultUri(defaultSoundUri))

                .SetContentIntent(pendingIntent)

                .Build();

            return builder;

        }

        private NotificationCompat.BigPictureStyle SetImageToNotification()
        {

            NotificationCompat.BigPictureStyle bigPictureStyle = new NotificationCompat.BigPictureStyle();

            BitmapFactory.Options options = new BitmapFactory.Options();

            options.InSampleSize = 2;

            bigPictureStyle.BigPicture(BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.ic_navheader_profile, options));

            bigPictureStyle.SetSummaryText("Imagen de la promoción");

            return bigPictureStyle;

        }

        private Bitmap DownloadImage()
        {

            StorageReference storageReference = FirebaseStorage
                .Instance
                .GetReferenceFromUrl(Configuration.FirebaseConfig.FIREBASE_STORAGE_URL);

            storageReference.GetBytes()

            return null;

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