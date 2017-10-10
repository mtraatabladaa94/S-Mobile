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

namespace SMobile.Android.Models.Entities
{

    public class UserListEntity
    {
        
        public const string USER_LIST_NAME = "usersList";

        public override string ToString()
        {

            return UserListEntity.USER_LIST_NAME;

        }

        public string uid { get; set; } // Key del nodo

        public string name { get; set; } // Nombre de la listas de usuarios
        
    }

}