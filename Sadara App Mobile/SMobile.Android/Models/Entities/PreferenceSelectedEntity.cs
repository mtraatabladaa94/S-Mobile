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
    public class PreferenceSelectedEntity
    {

        public string uid { get; set; } // Key del nodo

        public string name { get; set; } // Nombre de la preferencia

        public bool selected { get; set; } = false; // Seleccionado

    }
}