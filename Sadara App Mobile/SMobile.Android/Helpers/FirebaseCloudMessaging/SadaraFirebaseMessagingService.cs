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
using Android.Gms.Tasks;
using Java.Lang;
using Android.Runtime;
using Firebase;

namespace SMobile.Android.Helpers
{
    [Service]
    [IntentFilter(new string[] { "com.google.firebase.MESSAGING_EVENT" })]
    class SadaraFirebaseMessagingService : FirebaseMessagingService, IOnSuccessListener, IOnFailureListener
    {

        private string Title { get; set; }

        private string Body { get; set; }

        private string ImageUrl { get; set; }

        public override void OnMessageReceived(RemoteMessage message)
        {

            base.OnMessageReceived(message);

            //var App = FirebaseApp.InitializeApp(this, Configuration.FirebaseConfig.FirebaseOptions, "Sadara Mobile");

            this.ImageUrl = message.Data["main_picture"];

            this.Body = message.GetNotification().Body;

            this.Title = message.GetNotification().Title;

            InitNotification();
            
        }

        private void InitNotification()
        {

            if (string.IsNullOrWhiteSpace(this.ImageUrl) || string.IsNullOrEmpty(this.ImageUrl))
            {
                this.SendNotification(this.Title, this.Body);
            }
            else
            {
                this.DownloadImage(this.ImageUrl);
            }

        }

        private void SendNotification(string title, string body, Bitmap image = null)
        {

            var intent = new Intent(this, typeof(MainActivity));

            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationManager = NotificationManager.FromContext(this);

            notificationManager
                .Notify(0, this.BuilderNotification(title, body, RingtoneType.Notification, pendingIntent, image));

        }

        private Notification BuilderNotification(
            string title,
            string body,
            RingtoneType defaultSoundUri,
            PendingIntent pendingIntent,
            Bitmap image)
        {
            
            var builder = new NotificationCompat.Builder(this)

                .SetSmallIcon(Resource.Drawable.ic_isotipo_sadara_ico)

                .SetContentTitle(title)

                .SetContentText(body)
                
                .SetAutoCancel(true)

                .SetSound(RingtoneManager.GetDefaultUri(defaultSoundUri))

                .SetContentIntent(pendingIntent);

            if (image != null)
            {

                builder

                    .SetStyle(this.SetImageToNotification(image));

            }

            return builder.Build();

        }

        private NotificationCompat.BigPictureStyle SetImageToNotification(Bitmap image)
        {

            NotificationCompat.BigPictureStyle bigPictureStyle = new NotificationCompat.BigPictureStyle();

            bigPictureStyle.BigPicture(image);

            bigPictureStyle.SetSummaryText("Imagen de la promoción");

            return bigPictureStyle;

        }

        private void DownloadImage(string ImageUrl)
        {

            StorageReference storageReference = FirebaseStorage
                .Instance
                .GetReferenceFromUrl(ImageUrl);

            storageReference.GetBytes(Configuration.FirebaseConfig.ONE_MEGABYTE)
                .AddOnSuccessListener(this)
                .AddOnFailureListener(this);
            
        }
        
        void IOnSuccessListener.OnSuccess(Java.Lang.Object result)
        {

            var data = result.ToArray<byte>();

            BitmapFactory.Options options = new BitmapFactory.Options();

            options.InSampleSize = 2;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length, options);

            this.SendNotification(this.Title, this.Body, bitmap);

        }

        void IOnFailureListener.OnFailure(Java.Lang.Exception e)
        {
            
        }
        
    }
    
}