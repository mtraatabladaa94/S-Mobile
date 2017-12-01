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
using Android.Content.PM;
using Android.Support.V7.App;

using FragmentSupport = Android.Support.V4.App.Fragment;

namespace SMobile.Android.Helpers.Permissions
{

    public class ManagePermission
    {

        public static bool CheckPermission(Activity activity, string permission)
        {

            return activity.CheckSelfPermission(permission) == Permission.Granted;

        }

        public static void RequestPermission(Activity activity, string[] permissions)
        {

            activity.RequestPermissions(permissions, 0);

        }

        public static bool CheckPermission(AppCompatActivity compatActivity, string permission)
        {

            return compatActivity.CheckSelfPermission(permission) == Permission.Granted;

        }

        public static void RequestPermission(AppCompatActivity compatActivity, string[] permissions)
        {

            compatActivity.RequestPermissions(permissions, 0);

        }

        public static void RequestPermission(FragmentSupport fragmentSupport, string[] permissions)
        {

            fragmentSupport.RequestPermissions(permissions, 0);

        }

    }

}