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
using Android.Support.V7.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android;

namespace SMobile.Android.Activities
{

    [Activity(Label = "Sadara Mobile", MainLauncher = false, Icon = "@drawable/ic_isotipo_sadara")]
    public class MapsActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {

            RequestPermissions(

                new string[] {

                    Manifest.Permission.Internet,

                    Manifest.Permission.AccessCoarseLocation,

                    Manifest.Permission.AccessFineLocation,

                    Manifest.Permission.AccessNetworkState,

                },

                0

            );

            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.maps_layout);

            

        }

    }

}