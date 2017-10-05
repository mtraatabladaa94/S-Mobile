using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SMobile.Android.Models.FirebaseModel
{

    public class PreferenceWithUserModel
    {

        public const string PREFERENCE_WITH_USER_NAME = "PreferencesWithUsers";

        FirebaseClient firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

        public async void Add(Models.Entities.PreferenceWithUserEntity preference)
        {

            await firebaseClient

                .Child(PreferenceWithUserModel.PREFERENCE_WITH_USER_NAME)

                .PostAsync<Models.Entities.PreferenceWithUserEntity>

                (preference);

        }

    }

}