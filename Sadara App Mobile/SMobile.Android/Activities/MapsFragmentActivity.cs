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

using Android.Gms.Maps;
using Android;
using Android.Support.V7.App;
using Android.Gms.Maps.Model;
using Android.Locations;

namespace SMobile.Android.Activities
{
    public class MapsFragmentActivity : Fragment, IOnMapReadyCallback, GoogleMap.IOnMarkerClickListener
    {

        private LocationManager locationManager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.maps_fragment_layout, container, false);

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
            MapFragment mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.sadara_maps_fragment);

            if (mapFragment == null)
            {

                GoogleMapOptions mapOptions = new GoogleMapOptions()

                    .InvokeMapType(GoogleMap.MapTypeTerrain)

                    .InvokeZoomControlsEnabled(false)

                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();

                mapFragment = MapFragment.NewInstance(mapOptions);

                fragTx.Add(Resource.Id.sadara_maps_fragment, mapFragment, "map");

                fragTx.Commit();

            }

            TextView textView = view.FindViewById<TextView>(Resource.Id.textviewmap);

            Button button = view.FindViewById<Button>(Resource.Id.buttonmap);

            Button button2 = view.FindViewById<Button>(Resource.Id.buttonmapintro);

            button2.Text = "Soy un botón";

            mapFragment.GetMapAsync(this);
            
            return view;

        }

        void IOnMapReadyCallback.OnMapReady(GoogleMap googleMap)
        {

            MarkerOptions markerOptions = new MarkerOptions();

            this.locationManager = Context.GetSystemService(Context.LocationService) as LocationManager;

            markerOptions.SetPosition(new LatLng(12.1062900, -85.3645200));

            markerOptions.SetTitle("Michel Traña");

            googleMap.AddMarker(markerOptions);

            googleMap.UiSettings.ZoomControlsEnabled = true;

            googleMap.UiSettings.CompassEnabled = true;

            googleMap.AnimateCamera(this.BuilderPositionMap(new LatLng(12.1062900, -85.3645200)));

            googleMap.SetOnMarkerClickListener(this);

        }

        private CameraUpdate BuilderPositionMap(LatLng location)
        {

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();

            builder.Target(location);

            builder.Zoom(15);

            builder.Bearing(155);

            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            return cameraUpdate;

        }

        bool GoogleMap.IOnMarkerClickListener.OnMarkerClick(Marker marker)
        {
            
            return true;

        }

    }

}