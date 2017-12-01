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
using Android.Graphics;

namespace SMobile.Android.Configuration
{

    public class UserConfig
    {

        public static Models.Entities.UserEntity currentUserEntity { get; set; }

        public static Bitmap currentUserProfile { get; set; }

    }

}