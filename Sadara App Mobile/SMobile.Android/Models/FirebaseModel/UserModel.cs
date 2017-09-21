using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMobile.Android.Models.FirebaseModel
{
    class UserModel
    {
        FirebaseClient firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

        public async void Add(Models.Entities.UserEntity user)
        {
            
            await firebaseClient

                .Child("Users")

                .PostAsync<Models.Entities.UserEntity>

                (user);

        }

        public async void Edit(Models.Entities.UserEntity user)
        {
            
            await firebaseClient

                .Child("Users")

                .Client

                .Child(user.uid)

                .Client

                .Child(nameof(user.firstName))

                .PutAsync(user.firstName);

        }

        public async void Delete(Models.Entities.UserEntity user)
        {

            await firebaseClient

                .Child("Users")

                .Client.Child(user.uid)

                .DeleteAsync();

        }

        public async Task<List<Models.Entities.UserEntity>> List()
        {

            var users = await this.firebaseClient.Child("Users").OnceAsync<Models.Entities.UserEntity>();

            List<Models.Entities.UserEntity> usersList = new List<Entities.UserEntity>();

            foreach (var user in users)
            {

                usersList.Add(

                    new Models.Entities.UserEntity() {

                        uid = user.Key,

                        firstName = user.Object.firstName,

                        lastName = user.Object.lastName,

                        gender = user.Object.gender,

                        birthDate = user.Object.birthDate,

                        email = user.Object.email,

                        phone = user.Object.phone
                    }

                );

            }

            return usersList;

        }

    }
}