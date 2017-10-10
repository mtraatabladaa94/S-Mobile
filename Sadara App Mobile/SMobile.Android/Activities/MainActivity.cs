using Android.App;
using Android;
using Android.Widget;
using Android.OS;

using Android.Support.V7.App;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Views;

using Realms;

namespace SMobile.Android
{
    //[Activity(Label = "Sadara Mobile", Theme = "@style/Main.Theme.SadaraTheme")]
    [Activity(Label = "Sadara", MainLauncher = false, Icon = "@drawable/ic_isotipo_sadara")]
    public class MainActivity : AppCompatActivity
    {
        //Drawable menú
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        //Text input
        EditText productEditText;
        ImageButton productSentButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);

            //
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);

            //
            this.SetSupportActionBar(toolbar);

            //
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);

            //
            this.drawerLayout.SetDrawerListener(drawerToggle);

            //
            drawerToggle.SyncState();

            //
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //
            this.setupDrawerContent(navigationView); //Calling Function

            //Input text Real Test
            this.productEditText = FindViewById<EditText>(Resource.Id.productEditText);
            this.productSentButton = FindViewById<ImageButton>(Resource.Id.productSentButton);
            this.productSentButton.Click += delegate
            {

                this.saveProduct();

            };
        }
        
        Realm realm = Realm.GetInstance(Models.RealmModel.RealmConfig.realmConfiguration);

        private void saveProduct()
        {
            
            this.realm.Write(() => {

                realm.Add(new Models.RealmModel.ProductEntity() {

                    ProductId = System.Guid.NewGuid().ToString(),

                    ProductName = this.productEditText.Text

                });

            });

            Toast.MakeText(this, "Guardado correctamente", ToastLength.Long).Show();

        }

        private void setupDrawerContent(NavigationView navigationView)
        {

            navigationView.NavigationItemSelected += (sender, e) =>
            {

                e.MenuItem.SetChecked(true);

                drawerLayout.CloseDrawers();

            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.NavMenu); //Navigation Drawer Layout Menu Creation
            return base.OnCreateOptionsMenu(menu);
        }

    }
}

