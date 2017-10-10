using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System.Threading.Tasks;

namespace SMobile.Android.Models.FirebaseModel
{
    class FirebaseModels<T> :
        ITransaction.IAdd<T>,
        ITransaction.IUpdate<T>,
        ITransaction.IRemove<T>,
        ITransaction.IDataList<T>
    {

        FirebaseClient firebaseClient;

        public async Task<FirebaseObject<T>> Add(T Entity)
        {

            this.firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

            return await firebaseClient

                .Child(Entity.ToString())

                .PostAsync<T>

                (Entity);

        }

        public async Task<List<FirebaseObject<T>>> List(string URL)
        {

            this.firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

            var dataList = await this.firebaseClient.Child(URL).OnceAsync<T>();
            
            return dataList.ToList();
            
        }

        public async Task Remove<Id>(T Entity, Id Uid)
        {

            this.firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

            await firebaseClient

                .Child(Entity.ToString())

                .Client.Child(Uid.ToString())

                .DeleteAsync();

        }

        public void Update(T Entity, List<Tuple<string, string, object>> DataToChanges)
        {

            this.firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);

            DataToChanges.ForEach(async item => {

                await firebaseClient

                .Child(Entity.ToString())

                .Child(item.Item1)

                .Child(item.Item2)

                .PutAsync(item.Item3);

            });
            
        }

    }

}