using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;
using Android.Content;
using SMobile.Android.Configuration;
using Android.Views;

using Xamarin.Facebook;
using Java.Lang;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;

using Android.Gms.Plus;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Util;
using System;
using Android.Runtime;
using Android;
using Xamarin.Facebook.AppEvents;
using Org.Json;
using Newtonsoft.Json;

namespace SMobile.Android.Activities
{
    [Activity(Label = "Iniciar Sesión", MainLauncher = true)]
    public class SigninActivity : AppCompatActivity, IOnCompleteListener, IFacebookCallback, GoogleApiClient.IOnConnectionFailedListener, GoogleApiClient.IConnectionCallbacks, GraphRequest.IGraphJSONObjectCallback
    {

        FirebaseAuth auth;

        EditText edtUsername, edtPassword;

        Button btnSignin;

        RelativeLayout lytSignin, lytSignup;

        TextView txtRegister1, txtRegister2;

        //------------SAIRA
        private GoogleApiClient mGoogleApiClient;
        private SignInButton mGoogleSignIn;

        const string TAG = "SigninActivity";
        const string KEY_resuelto = "resuelto";
        const string KEY_poresolver = "por_resolver";
        const int RC_SIGN_IN = 9001;
        TextView mStatus;
        bool resuelto = false;
        bool poresolver = false;
        //____________________


        /*Facebook Objects*/
        Button facebookButton;
        ICallbackManager facebookCallbackManager;
        Models.FacebookModel.MyProfileTracker myProfile;
        /*Fin Facebook Objects*/

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);            

            RequestPermissions(

                new string[] {

                    Manifest.Permission.GetAccounts,

                    Manifest.Permission.UseCredentials,

                    Manifest.Permission.Internet,

                    Manifest.Permission.ReadContacts,

                    Manifest.Permission.Camera

                },

                0

            );

            /*Initialize SDK Facebook*/
            FacebookSdk.SdkInitialize(this);
            AppEventsLogger.ActivateApp(this);
            this.myProfile = new Models.FacebookModel.MyProfileTracker();
            this.myProfile.onProfileChanged += MyProfile_onProfileChanged;
            this.myProfile.StartTracking();
            /*Fin SDK Facebook*/

            this.SetContentView(Resource.Layout.Signin);

            //----------------------------//
            if (savedInstanceState != null)
            {
                resuelto = savedInstanceState.GetBoolean(KEY_resuelto);
                poresolver = savedInstanceState.GetBoolean(KEY_poresolver);
            }
            //___________________________________________________
            //_____________________________________________________//
            this.mStatus = FindViewById<TextView>(Resource.Id.forgotPasswordTextView);

            mGoogleSignIn = FindViewById<SignInButton>(Resource.Id.signinButtongoogle);
            //mGoogleSignIn.SetOnClickListener(this);
            mGoogleSignIn.Click += delegate {
                mStatus.Text = "Sign in..";
                poresolver = true;
                mGoogleApiClient.Connect();
            };
            //FindViewById<TextView>(Resource.Id.SignOut).SetOnClickListener(this);
            TextView txtSignout = FindViewById<TextView>(Resource.Id.SignOut);
            mStatus.Click += delegate {
                if (mGoogleApiClient.IsConnected)
                {
                    PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
                    mGoogleApiClient.Disconnect();
                    Toast.MakeText(this, "Ya habia iniciado sesión", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "No ha iniciado sesión", ToastLength.Long).Show();
                }
                UpdateUI(false);
            };

            mGoogleApiClient = new GoogleApiClient.Builder(this)
             .AddConnectionCallbacks(this)
             .AddOnConnectionFailedListener(this)
             .AddApi(PlusClass.API)
             .AddScope(new Scope(Scopes.Profile))
             .Build();


            // GoogleSignInOptions gso= new GoogleSignInOptions.B
            // GoogleApiClientBuilder builder = new GoogleApiClientBuilder(this);

            //_____________________________________________________///

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

            
            /*Login de Facebook*/
            this.facebookButton = FindViewById<Button>(Resource.Id.facebookButton);//Receipt button from view
            
            this.facebookCallbackManager = CallbackManagerFactory.Create();//Create interface 'ICallbackManager'

            LoginButton loginButton = FindViewById<LoginButton>(Resource.Id.login_button);
            loginButton.RegisterCallback(this.facebookCallbackManager, this);

            LoginManager.Instance.RegisterCallback(facebookCallbackManager, this);//Register instance 'ICallbackManager' in LoginManager

            this.facebookButton.Click += (e, handler) => {

                if (AccessToken.CurrentAccessToken == null)
                {
                    
                    LoginManager.Instance.LogInWithReadPermissions(

                        this,

                        new System.Collections.Generic.List<string>() {

                            "public_profile",

                            "email",

                            "user_about_me",

                            "user_birthday",
                            
                        }

                    );

                    

                }

            };
            /*Fin Login de Facebook*/
            
        }

        private void MyProfile_onProfileChanged(object sender, Models.FacebookModel.OnProfileChangedEventArgs e)
        {
            if(e.profile != null)
                Toast.MakeText(this, $"Nombre: {e.profile.FirstName} {e.profile.LastName} {e.profile.Name} {e.profile.GetProfilePictureUri(512, 512).ToString()}", ToastLength.Long).Show();
        }

        protected override void OnDestroy()
        {
            
            myProfile.StopTracking();

            base.OnDestroy();

        }

        private void StartFirebaseAuth()
        {
            
            if (FirebaseConfig.app == null)
                FirebaseConfig.app = FirebaseApp.InitializeApp(this, FirebaseConfig.firebaseOptions, "Sadara Mobile");
            

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

        //Facebook Callback
        public void OnCancel()
        {

        }

        public void OnError(FacebookException error)
        {

        }

        public void OnSuccess(Java.Lang.Object result)
        {
            if (AccessToken.CurrentAccessToken != null)
            {

                GraphRequest graphRequest = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);

                Bundle bundle = new Bundle();

                bundle.PutString("fields", "id,email,first_name,last_name,birthday,gender");

                graphRequest.Parameters = bundle;

                graphRequest.ExecuteAsync();

            }

        }
        //End Facebook Callback

        //Google API Client
        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "onConnectionFailed:" + result);
            
            if (!resuelto && poresolver)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult(this, RC_SIGN_IN);
                        resuelto = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        Log.Error(TAG, "No se puede resolver la conexión.", e);
                        resuelto = false;
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    ShowErrorDialog(result);
                }
            }
            else
            {
                UpdateUI(false);
            }
        }

        class DialogInterfaceOnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            public Action<IDialogInterface> OnCancelImpl { get; set; }

            public void OnCancel(IDialogInterface dialog)
            {
                OnCancelImpl(dialog);
            }
        }

        void ShowErrorDialog(ConnectionResult connectionResult)
        {
            int errorCode = connectionResult.ErrorCode;

            if (GooglePlayServicesUtil.IsUserRecoverableError(errorCode))
            {
                var listener = new DialogInterfaceOnCancelListener();
                listener.OnCancelImpl = (dialog) =>
                {
                    poresolver = false;
                    UpdateUI(false);
                };
                GooglePlayServicesUtil.GetErrorDialog(errorCode, this, RC_SIGN_IN, listener).Show();
            }
            else
            {
                //var errorstring = string.Format(GetString(Resource.String.common_google_play_services_update_title), errorCode);
                var errorstring = string.Format("ERROR Google Play Service", errorCode);
                Toast.MakeText(this, errorstring, ToastLength.Short).Show();

                poresolver = false;
                UpdateUI(false);
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            Log.Debug(TAG, "onConnected:" + connectionHint);

            UpdateUI(true);
        }

        public void OnConnectionSuspended(int cause)
        {
            Log.Warn(TAG, "onConnectionSuspended:" + cause);
        }

        void UpdateUI(bool isSignedIn)
        {
            if (isSignedIn)
            {
                var person = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
                var name = string.Empty;
                if (person != null)
                    name = person.DisplayName;
                var genero = person.Gender;
                var fechaNac = person.Birthday;
                var personPhoto = person.Image;
                    
            
                var email = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);
                //mStatus.Text = string.Format(GetString(Resource.String.signed_in_fmt), name);
                mStatus.Text = name.ToString();


                FindViewById(Resource.Id.signinButtongoogle).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.SignOut).Visibility = ViewStates.Visible;
            }
            else
            {
                mStatus.Text = "Sign OUT";

                FindViewById(Resource.Id.signinButtongoogle).Enabled = true;
                //  FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.SignOut).Visibility = ViewStates.Gone;
            }
        }

        //End Google API Client

        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Log.Debug(TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);
            if (requestCode == RC_SIGN_IN)
            {
                if (resultCode != Result.Ok)
                {
                    poresolver = false;
                }
                resuelto = false;
                mGoogleApiClient.Connect();
            }

            /*Facebook SDK Result*/
            this.facebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        public void OnCompleted(JSONObject @object, GraphResponse response)
        {
            Models.FacebookModel.FacebookResult result = JsonConvert.DeserializeObject<Models.FacebookModel.FacebookResult>(@object.ToString());
        }
    }

}