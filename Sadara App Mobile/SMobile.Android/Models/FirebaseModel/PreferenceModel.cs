using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMobile.Android.Models.FirebaseModel
{
    public class PreferenceModel
    {
        
        FirebaseClient firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

        public async void Add(Models.Entities.PreferenceEntity preference)
        {

            await firebaseClient

                .Child("Preferences")

                .PostAsync<Models.Entities.PreferenceEntity>

                (preference);

        }

        public async Task<List<Models.Entities.PreferenceEntity>> List()
        {
            try
            {
                
                var preferences = await this.firebaseClient.Child("Preferences").OnceAsync<Models.Entities.PreferenceEntity>();


                List<Models.Entities.PreferenceEntity> preferencesList = new List<Entities.PreferenceEntity>();

                foreach (var preference in preferences)
                {

                    var productos = await this.firebaseClient.Child(preference.Key).OnceAsync<Models.Entities.PreferenceEntity>();

                    preferencesList.Add(

                        new Models.Entities.PreferenceEntity()
                        {

                            uid = preference.Key,

                            name = preference.Object.name,

                        }

                    );

                }

                return preferencesList;

            }
            catch (Firebase.FirebaseException ex)
            {

                throw new Firebase.FirebaseException(ex.Message);

            }
            
        }

    }
}