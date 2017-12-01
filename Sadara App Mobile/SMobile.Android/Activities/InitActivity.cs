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
using Android.Support.V4.View;
using Firebase;
using Firebase.Auth;

namespace SMobile.Android.Activities
{
    [Activity(Label = "Sadara", MainLauncher = true, Icon = "@drawable/ic_isotipo_sadara")]
    public class InitActivity : AppCompatActivity
    {

        #region Declarations of components for view

        LinearLayout actionLayout;

        Button signin_button, register_button; //Buttons for call view respectives

        ViewPager sliderViewPager;

        #endregion 

        private void InitialComponents()
        {

            //Start Firebase
            this.StartFirebase();

            //Layout of actions
            this.actionLayout = this.FindViewById<LinearLayout>(Resource.Id.splash_action_layout);
            var widthAction = Resources.DisplayMetrics.WidthPixels;

            //Call view for auth user
            this.signin_button = this.FindViewById<Button>(Resource.Id.signin_button);
            this.signin_button.SetWidth((widthAction - 1) / 2);
            this.signin_button.Click += Signin_Click;
            

            //Call view for resgiter user
            this.register_button = this.FindViewById<Button>(Resource.Id.register_button);
            this.register_button.SetWidth((widthAction - 1) / 2);
            this.register_button.Click += Register_button_Click;

            //Slider show
            this.sliderViewPager = this.FindViewById<ViewPager>(Resource.Id.sliderViewPager);
            Helpers.SliderImages.SliderAdapter adapter = new Helpers.SliderImages.SliderAdapter(this);
            this.sliderViewPager.Adapter = adapter;

        }

        private void Signin_Click(object sender, EventArgs e)
        {

            Intent intent = new Intent(this, typeof(SigninActivity));

            this.StartActivity(intent);

        }

        private void Register_button_Click(object sender, EventArgs e)
        {

            Configuration.UserConfig.currentUserEntity = new Models.Entities.UserEntity();

            Intent intent = new Intent(this, typeof(SignupActivity));

            this.StartActivity(intent);

        }

        private Boolean IsUserSigned()
        {

            return false;

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            if (!this.IsUserSigned()) //Eval if not a user is started
            {

                this.SetContentView(Resource.Layout.splash_layout);//Set content view for this activity

                this.InitialComponents();

                //Window.DecorView.SetFitsSystemWindows(true);

                //Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;

            }
            else
            {

                this.StartMainActivity();//Show main activity

                this.Finish();//Close activity

            }

        }

        private void StartMainActivity()
        {

            Intent intent = new Intent(this, typeof(MainActivity));

            this.StartActivity(intent);

        }

        #region Private functions

        private void StartFirebase()
        {

            if (Configuration.FirebaseConfig.App == null)
                Configuration.FirebaseConfig.App = FirebaseApp.InitializeApp(this, Configuration.FirebaseConfig.FirebaseOptions, "Sadara App Mobile");

            if (Configuration.FirebaseConfig.Auth == null)
                Configuration.FirebaseConfig.Auth = FirebaseAuth.GetInstance(Configuration.FirebaseConfig.App);

        }

        #endregion

    }

}