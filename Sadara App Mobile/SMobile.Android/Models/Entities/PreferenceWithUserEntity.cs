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

    public class PreferenceWithUserEntity
    {

        public const string PREFERENCE_WITH_USER_NAME = "preferencesWithUsers";

        public override string ToString()
        {

            return PreferenceWithUserEntity.PREFERENCE_WITH_USER_NAME;

        }

        public string uid { get; set; }

        public string uidPreference { get; set; }
        
    }

}