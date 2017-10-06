using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;

namespace SMobile.Android.Activities
{

    [Activity(Label = "Sadara", MainLauncher = true, Icon = "@drawable/ic_isotipo_sadara")]
    public class UserListActivity : AppCompatActivity
    {

        FloatingActionButton AddListButton;
        Toolbar toolbar;

        private void InitialComponents()
        {

            //Load floating action button for create user list
            this.SetPropertiesAddListButton();

            //Load toolbar V7
            this.toolbar = FindViewById<Toolbar>(Resource.Id.user_list_toolbar);
            toolbar.Title = "Listas de productos";
            this.SetSupportActionBar(this.toolbar);

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            base.SetContentView(Resource.Layout.user_list_layout);

            //Initialize all components
            this.InitialComponents();

        }

        private void SetPropertiesAddListButton()
        {

            //Initialize objecto FloatingActionButton
            this.AddListButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            
        }

        

    }

}