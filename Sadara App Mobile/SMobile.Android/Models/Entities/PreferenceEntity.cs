using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.Entities
{
    public class PreferenceEntity
    {

        public const string PREFERENCE_NAME = "preferences";

        public override string ToString()
        {

            return PreferenceEntity.PREFERENCE_NAME;

        }
        
        public string uid { get; set; } // Key del nodo

        public string name { get; set; } // Nombre de la preferencia

        public string topic { get; set; }

    }
}