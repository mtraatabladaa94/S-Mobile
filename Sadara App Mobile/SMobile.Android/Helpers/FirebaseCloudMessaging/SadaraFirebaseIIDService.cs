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
    class SadaraFirebaseIIDService : FirebaseInstanceIdService
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
            Models.FirebaseModel.UserTokenModel userTokenModel = new Models.FirebaseModel.UserTokenModel();

            try
            {

                userTokenModel.Add(new Models.Entities.UserTokenEntity()
                {

                    uid = Configuration.FirebaseConfig.Auth.CurrentUser.Uid,

                    token = token,

                });

            }
            catch (Java.Lang.Exception ex)
            {
                


            }
            
        }

    }

}