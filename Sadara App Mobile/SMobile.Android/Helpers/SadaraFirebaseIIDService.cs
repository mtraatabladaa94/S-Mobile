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
using Firebase.Iid;
using Android.Util;

namespace SMobile.Android.Helpers
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class SadaraFirebaseIIDService : FirebaseInstanceIdService
    {

        const string TAG = "SadaraFirebaseIIDService";

        public override void OnTokenRefresh()
        {

            var refreshedToken = FirebaseInstanceId.Instance.Token;

            Log.Debug(TAG, "Refreshed token: " + refreshedToken);

            SendRegistrationToServer(refreshedToken);

        }

        void SendRegistrationToServer(string token)
        {

            // Add custom implementation, as needed.


        }

    }

}