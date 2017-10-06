using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMobile.Android.Models.FirebaseModel.Transaction
{
    interface IDataList<T>
    {

        Task<List<FirebaseObject<T>>> List(string UrlChild);

    }
}