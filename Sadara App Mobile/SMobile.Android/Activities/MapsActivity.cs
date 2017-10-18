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

namespace SMobile.Android.Activities
{
    [Activity(Label = "Sadara Mobile", MainLauncher = false, Icon = "@drawable/ic_isotipo_sadara")]
    public class MapsActivity : Activity, IOnMapReadyCallback
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.maps_fragment_layout);

            MapFragment mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.maps_fragment_layout);

            mapFragment.GetMapAsync(this);

        }

        void IOnMapReadyCallback.OnMapReady(GoogleMap googleMap)
        {
            


        }

    }
}