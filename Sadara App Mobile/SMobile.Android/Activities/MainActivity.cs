using Android.App;
using Android;
using Android.Widget;
using Android.OS;

using Android.Support.V7.App;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Content;
using ImageViews.Rounded;
using Square.Picasso;

namespace SMobile.Android
{
    
    [Activity(Label = "Sadara", MainLauncher = false, Icon = "@drawable/ic_isotipo_sadara")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

    #region General Components
        //Drawable menú
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        //Text input
        EditText productEditText;
        ImageButton productSentButton;

        //toolbar
        V7Toolbar toolbar;

        //navcomponents
        ImageView profileImageView;
        TextView nameTextView, emailTextView;
        #endregion

    #region Fragments
        Fragments.MainFragment mainFragment;//Main Fragment
    #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);

            //Initialition of components
            this.InitialComponents();
            
        }
        
        private void InitialComponents()
        {

            //Toolbar
            this.toolbar = this.FindViewById<V7Toolbar>(Resource.Id.toolbar);

            //Init mainFragment
            this.mainFragment = new Fragments.MainFragment(this.SupportFragmentManager);
            this.StartMainFragment();

            //Function for init NavigationDrawable and Toolbar
            this.FuncNavigationDrawable();

            //user data
            if (Configuration.UserConfig.currentUserEntity != null)
            {

                if (this.nameTextView != null && this.emailTextView != null && this.profileImageView != null)
                {

                    this.nameTextView.Text =
                    $"{Configuration.UserConfig.currentUserEntity.firstName} {Configuration.UserConfig.currentUserEntity.lastName}";

                    this.emailTextView.Text = Configuration.UserConfig.currentUserEntity.email;

                    if (!string.IsNullOrWhiteSpace(Configuration.UserConfig.currentUserEntity.imageUrl))
                    {

                        var transformation = new RoundedTransformationBuilder()
                        .CornerRadiusDp(64)
                        .Oval(true)
                        .Build();

                        Picasso
                        .With(this)
                        .Load(Configuration.UserConfig.currentUserEntity.imageUrl)
                        .Resize(96, 96)
                        .CenterCrop()
                        .Transform(transformation)
                        .Error(
                            Configuration.UserConfig.currentUserEntity.gender.Equals("M")
                            ? Resource.Drawable.ic_user_man
                            : Resource.Drawable.ic_user_woman
                        )
                        .Into(this.profileImageView);

                    }
                    else
                    {

                        this.profileImageView.SetImageResource(
                            Configuration.UserConfig.currentUserEntity.gender.Equals("M") ?
                            Resource.Drawable.ic_user_man :
                            Resource.Drawable.ic_user_woman
                        );

                    }

                }

            }

        }

        private void Search_menu_Click(object sender, System.EventArgs e)
        {

            this.StartSearchMain();

        }

        private void StartMainFragment()
        {

            var trans = SupportFragmentManager.BeginTransaction();
            //trans.Add(Resource.Id.pon_fragmentos, this.mainFragment, "SetMainFragment");
            trans.Replace(Resource.Id.pon_fragmentos, this.mainFragment);
            trans.Commit();

        }

        private void StartUserPreferenceActivity()
        {

            Intent intent = new Intent(this, typeof(Activities.UserPreferenceActivity));

            this.StartActivity(intent);

        }

        private void FuncNavigationDrawable()
        {
            //Select DrawerLayout
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "";
            //toolbar.InflateMenu(Resource.Menu.main_toolbar_menu);

            //Asign Toolbar
            this.SetSupportActionBar(toolbar);

            //Create Toggle for Drawer
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);

            //Set Drawer listener to Toggle
            drawerLayout.SetDrawerListener(drawerToggle);

            //Sync state for Drawer Toggle
            drawerToggle.SyncState();

            //Select NavigationView
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //Set NavigationView to Drawer Content
            this.setupDrawerContent(navigationView); //Calling Function

            //Set components of NavigationView
            var navHeaderView = this.navigationView.InflateHeaderView(Resource.Layout.MainNavHeader);
            this.profileImageView = navHeaderView.FindViewById<ImageView>(Resource.Id.navheader_profile);
            this.nameTextView = navHeaderView.FindViewById<TextView>(Resource.Id.navheader_username);
            this.emailTextView = navHeaderView.FindViewById<TextView>(Resource.Id.navheader_useremail);

        }
        
        private void setupDrawerContent(NavigationView navigationView)
        {

            navigationView.NavigationItemSelected += (sender, e) =>
            {

                //e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {

                    case Resource.Id.nav_follows:
                        this.StartUserPreferenceActivity();
                        break;

                    case Resource.Id.nav_off_session:
                        this.Finish();
                        break;

                }

                drawerLayout.CloseDrawers();

            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            navigationView.InflateMenu(Resource.Menu.NavMenu); //Navigation Drawer Layout Menu Creation

            MenuInflater.Inflate(Resource.Menu.main_toolbar_menu, menu);

            IMenuItem searchMenu = menu.FindItem(Resource.Id.search_menu);

            if (searchMenu != null)
            {

                View actionView = searchMenu.ActionView;

                if (actionView != null)
                {

                    actionView.Click += Search_menu_Click;

                }
                

            }

            return true;

        }
        

        private void StartSearchMain()
        {

            Intent intent = new Intent(this, typeof(Activities.MainSearch));

            this.StartActivity(intent);

        }

        bool NavigationView.IOnNavigationItemSelectedListener.OnNavigationItemSelected(IMenuItem menuItem)
        {

            switch (menuItem.ItemId)
            {

                case Resource.Id.nav_off_session:

                    this.Finish();

                    break;

            }

            return true;

        }
    }

}