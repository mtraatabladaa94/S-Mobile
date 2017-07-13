using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;
using Android.Content;
using SMobile.Android.Configuration;

namespace SMobile.Android.Activities
{
    [Activity(Label = "Iniciar Sesión", MainLauncher = true)]
    public class SigninActivity : AppCompatActivity, IOnCompleteListener
    {

        FirebaseAuth auth;

        EditText edtUsername, edtPassword;

        Button btnSignin;

        RelativeLayout lytSignin, lytSignup;

        TextView txtRegister1, txtRegister2;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.Signin);

            this.StartFirebaseAuth();

            this.edtUsername = FindViewById<EditText>(Resource.Id.usernameEditText);

            this.edtPassword = FindViewById<EditText>(Resource.Id.passwordEditText);

            this.lytSignin = FindViewById<RelativeLayout>(Resource.Id.Signin);

            this.btnSignin = FindViewById<Button>(Resource.Id.signinButton);

            this.btnSignin.Click += delegate
            {

                this.SigninClick(edtUsername.Text, edtPassword.Text);

            };

            this.lytSignup = FindViewById<RelativeLayout>(Resource.Id.registerLayout);

            this.lytSignup.Click += delegate
            {

                this.SignupClick();

            };

            this.txtRegister1 = FindViewById<TextView>(Resource.Id.register1TextView);

            this.txtRegister1.Click += delegate
            {

                this.SignupClick();

            };

            this.txtRegister2 = FindViewById<TextView>(Resource.Id.register2TextView);

            this.txtRegister2.Click += delegate
            {

                this.SignupClick();

            };

        }
        
        private void StartFirebaseAuth()
        {

            var options = new FirebaseOptions
                .Builder()
                .SetApplicationId(FirebaseConfig.FIREBASE_APP_KEY)
                .SetApiKey(FirebaseConfig.FIREBASE_API_KEY)
                .Build();

            if (FirebaseConfig.app == null)
                FirebaseConfig.app = FirebaseApp.InitializeApp(this, options);

            this.auth = FirebaseAuth.GetInstance(FirebaseConfig.app);

        }

        private void SigninClick(string UserName, string Password)
        {

            this.auth
                .SignInWithEmailAndPassword(UserName, Password)
                .AddOnCompleteListener(this)
                ;

        }

        private void SignupClick()
        {

            this.StartActivity(new Intent(this, typeof(SignupActivity)));

        }

        public void OnComplete(Task task)
        {

            if (task.IsSuccessful)
            {

                this.StartActivity(new Intent(this, typeof(MainActivity)));

                this.Finish();

            }
            else
            {

                if (task.Exception != null)
                {

                    Toast.MakeText(this, task.Exception.Message, ToastLength.Long).Show();

                }
                else
                {

                    Toast.MakeText(this, "No ha podido iniciar sesión", ToastLength.Long).Show();

                }

            }

        }

    }

}