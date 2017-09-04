using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SMobile.Android.Configuration;

using Firebase.Xamarin.Database;

namespace SMobile.Android.Models.Entities
{

    internal sealed class UserEntity
    {

        public string uid { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public async void UserEntityD()
        {

            FirebaseClient db = new FirebaseClient("");

        }
        
    }

}