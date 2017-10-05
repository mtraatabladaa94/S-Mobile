using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System.Threading.Tasks;

namespace SMobile.Android.Models.FirebaseModel
{
    public class UserTokenModel
    {

        public const string USER_TOKEN_NAME = "Users";

        FirebaseClient firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

        public async void Add(Models.Entities.UserTokenEntity userToken)
        {

            await firebaseClient

                .Child(UserTokenModel.USER_TOKEN_NAME)

                .PostAsync<Models.Entities.UserTokenEntity>

                (userToken);

        }

        public async Task<List<Models.Entities.UserTokenEntity>> List()
        {

            var users = await this.firebaseClient.Child(UserTokenModel.USER_TOKEN_NAME).OnceAsync<Models.Entities.UserTokenEntity>();

            List<Models.Entities.UserTokenEntity> usersList = new List<Entities.UserTokenEntity>();
            
            foreach (var user in users)
            {

                usersList.Add(

                    new Models.Entities.UserTokenEntity()
                    {

                        uid = user.Key,

                        token = user.Object.token,
                        
                    }

                );

            }

            return usersList;

        }

    }
}