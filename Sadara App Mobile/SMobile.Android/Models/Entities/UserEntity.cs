using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SMobile.Android.Configuration;

using Firebase.Xamarin.Database;

namespace SMobile.Android.Models.Entities
{

    public sealed class UserEntity
    {
        
        public const string USER_NAME = "users";

        public override string ToString()
        {

            return UserEntity.USER_NAME;

        }
        
        public string uid { get; set; } // Key del nodo

        public string firstName { get; set; } // Nombres

        public string lastName { get; set; } // Apellidos

        public string gender { get; set; } // género

        public DateTime birthDate { get; set; } = new DateTime(1980, 1, 1); // fecha de nacimiento

        public string email { get; set; } // correo electrónico

        public string password { get; set; }

        public string phone { get; set; } // número de teléfono

        public string imageUrl { get; set; } // url de imagen

    }

}