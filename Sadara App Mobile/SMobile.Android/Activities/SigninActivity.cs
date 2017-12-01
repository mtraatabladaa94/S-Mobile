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

//using Xamarin.Facebook;
//using Java.Lang;
//using Xamarin.Facebook.Login.Widget;
//using Xamarin.Facebook.Login;

//using Android.Gms.Plus;
//using Android.Gms.Common.Apis;
//using Android.Gms.Common;
//using Android.Util;
using System;
using Android.Runtime;
using Android;
//using Xamarin.Facebook.AppEvents;
using Org.Json;
using Newtonsoft.Json;
using Android.Support.Design.Widget;

using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using V7Toolbar = Android.Support.V7.Widget.Toolbar;



using mTask = System.Threading.Tasks.Task;



namespace SMobile.Android.Activities
{
    [Activity(Label = "Iniciar Sesión")]
    public class SigninActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener //, IFacebookCallback, GoogleApiClient.IOnConnectionFailedListener, GoogleApiClient.IConnectionCallbacks, GraphRequest.IGraphJSONObjectCallback
    {

        #region Compnoents for View
        V7Toolbar toolbar;
        private EditText edtUsername, edtPassword;
        private Button btnSignin;
        private RelativeLayout lytSignin;
        private TextView txtRegister1, txtRegister2;

        FirebaseAuth auth;
        private ProgressDialog progress;
        #endregion

        //------------SAIRA
        //private GoogleApiClient mGoogleApiClient;
        //private SignInButton mGoogleSignIn;

        //const string TAG = "SigninActivity";
        //const string KEY_resuelto = "resuelto";
        //const string KEY_poresolver = "por_resolver";
        //const int RC_SIGN_IN = 9001;
        //TextView mStatus;
        //bool resuelto = false;
        //bool poresolver = false;
        //____________________

        /*Facebook Objects*/
        //Button facebookButton;
        //ICallbackManager facebookCallbackManager;
        //Models.FacebookModel.MyProfileTracker myProfile;
        /*Fin Facebook Objects*/

        private void InitialComponents()
        {

            //Toolbar
            this.toolbar = FindViewById<V7Toolbar>(Resource.Id.signin_toolbar);
            toolbar.Title = "Iniciar Sesión";
            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetHomeButtonEnabled(true);

            this.RequestPermissions();
            this.edtUsername = FindViewById<EditText>(Resource.Id.usernameEditText);
            this.edtPassword = FindViewById<EditText>(Resource.Id.passwordEditText);
            this.lytSignin = FindViewById<RelativeLayout>(Resource.Id.Signin);

            this.btnSignin = FindViewById<Button>(Resource.Id.signinButton);
            this.btnSignin.Click += BtnSignin_Click;

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.Signin);

            this.InitialComponents();

            /*Initialize SDK Facebook*/
            //FacebookSdk.SdkInitialize(this);
            //AppEventsLogger.ActivateApp(this);
            //this.myProfile = new Models.FacebookModel.MyProfileTracker();
            //this.myProfile.OnProfileChanged += MyProfile_onProfileChanged;
            //this.myProfile.StartTracking();
            /*Fin SDK Facebook*/
            
            //----------------------------//
            //if (savedInstanceState != null)
            //{
            //    resuelto = savedInstanceState.GetBoolean(KEY_resuelto);
            //    poresolver = savedInstanceState.GetBoolean(KEY_poresolver);
            //}
            //___________________________________________________
            //_____________________________________________________//
            //this.mStatus = FindViewById<TextView>(Resource.Id.forgotPasswordTextView);

            //mGoogleSignIn = FindViewById<SignInButton>(Resource.Id.signinButtongoogle);
            ////mGoogleSignIn.SetOnClickListener(this);
            //mGoogleSignIn.Click += delegate {
            //    mStatus.Text = "Sign in..";
            //    poresolver = true;
            //    mGoogleApiClient.Connect();
            //};
            //FindViewById<TextView>(Resource.Id.SignOut).SetOnClickListener(this);
            //TextView txtSignout = FindViewById<TextView>(Resource.Id.SignOut);
            //mStatus.Click += delegate {
            //    if (mGoogleApiClient.IsConnected)
            //    {
            //        PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
            //        mGoogleApiClient.Disconnect();
            //        Toast.MakeText(this, "Ya habia iniciado sesión", ToastLength.Long).Show();
            //    }
            //    else
            //    {
            //        Toast.MakeText(this, "No ha iniciado sesión", ToastLength.Long).Show();
            //    }
            //    UpdateUI(false);
            //};

            //mGoogleApiClient = new GoogleApiClient.Builder(this)
            // .AddConnectionCallbacks(this)
            // .AddOnConnectionFailedListener(this)
            // .AddApi(PlusClass.API)
            // .AddScope(new Scope(Scopes.Profile))
            // .Build();


            // GoogleSignInOptions gso= new GoogleSignInOptions.B
            // GoogleApiClientBuilder builder = new GoogleApiClientBuilder(this);

            //_____________________________________________________///
            
            /*Login de Facebook*/
            //this.facebookButton = FindViewById<Button>(Resource.Id.facebookButton);//Receipt button from view
            
            //this.facebookCallbackManager = CallbackManagerFactory.Create();//Create interface 'ICallbackManager'

            //LoginButton loginButton = FindViewById<LoginButton>(Resource.Id.login_button);
            //loginButton.RegisterCallback(this.facebookCallbackManager, this);

            //LoginManager.Instance.RegisterCallback(facebookCallbackManager, this);//Register instance 'ICallbackManager' in LoginManager

            //this.facebookButton.Click += (e, handler) => {

            //    if (AccessToken.CurrentAccessToken == null)
            //    {
                    
            //        LoginManager.Instance.LogInWithReadPermissions(

            //            this,

            //            new System.Collections.Generic.List<string>() {
                            
            //                "email",
                            
            //            }

            //        );
                    
            //    }

            //};
            ///*Fin Login de Facebook*/
            
        }

        #region Eventos for components

        private void BtnSignin_Click(object sender, EventArgs e)
        {

            if (this.ValidateEntity())
            {

                this.SigninUser(this.edtUsername.Text, this.edtPassword.Text);

                this.ShowProgressDialog();

            }

        }

        #endregion

        #region Private functions

        private bool ValidateEntity()
        {

            bool temp = true;

            if (this.edtUsername.Text.Trim().Length == 0)
            {

                this.edtUsername.Error = "Ingresar email";

                temp = false;

            }

            if (this.edtPassword.Text.Trim().Length == 0)
            {

                this.edtPassword.Error = "Ingresar contraseña";

                temp = false;

            }

            return temp;

        }

        private void RequestPermissions()
        {

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

        }

        private void StartFirebaseAuth()
        {

            if (FirebaseConfig.App == null)
                FirebaseConfig.App = FirebaseApp.InitializeApp(this, FirebaseConfig.FirebaseOptions, "Sadara Mobile");

            this.auth = FirebaseAuth.GetInstance(FirebaseConfig.App);

        }

        private async void SigninUser(string UserName, string Password)
        {

            Configuration.FirebaseConfig.Auth
                .SignInWithEmailAndPassword(UserName, Password)
                .AddOnSuccessListener(this)
                .AddOnFailureListener(this);

        }

        private void StartMainActivity()
        {

            var intent = new Intent(this, typeof(MainActivity));

            this.StartActivity(intent);

        }

        private void ShowProgressDialog()
        {

            this.progress = new ProgressDialog(this);

            this.progress.SetTitle("Cargando...");

            this.progress.SetMessage("Espere un momento por favor...");

            this.progress.Window.SetType(WindowManagerTypes.SystemAlert);

            this.progress.Show();

        }

        private async mTask  FindUserById(string email)
        {

            Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity> firebaseModel =
                new Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity>();

            var users = await firebaseModel.List(Models.Entities.UserEntity.USER_NAME);

            foreach (var item in users)
            {

                if (item.Object.email.Equals(email))
                {

                    Configuration.UserConfig.currentUserEntity = new Models.Entities.UserEntity() {

                        uid = item.Key,

                        firstName = item.Object.firstName,

                        lastName = item.Object.lastName,

                        gender = item.Object.gender,

                        birthDate = item.Object.birthDate,

                        email = item.Object.email,

                        phone = item.Object.phone,

                        imageUrl = item.Object.imageUrl,

                    };

                    return;

                }

            }

        }

        #endregion

        #region Overrides functions

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                //Button back again
                case 16908332: //Home Id

                    this.Finish();

                    break;
                    
            }

            return true;

        }

        #endregion

        #region Functions of interfaces

        async void IOnSuccessListener.OnSuccess(Java.Lang.Object result)
        {

            var user = Configuration.FirebaseConfig.Auth.CurrentUser;

            if (user != null)
            {

                await this.FindUserById(user.Email);

            }

            this.StartMainActivity();

            this.progress.Dismiss();

            this.Finish();

        }

        void IOnFailureListener.OnFailure(Java.Lang.Exception e)
        {

            this.progress.Dismiss();

            Snackbar snackbar = Snackbar
                .Make(this.lytSignin, $"{e.Message}", Snackbar.LengthShort)
                .SetAction("Ok", (view) => {

                    //Insert code for action here

                });

            snackbar.Show();

        }

        #endregion

        #region Codes comments

        //private void MyProfile_onProfileChanged(object sender, Models.FacebookModel.OnProfileChangedEventArgs e)
        //{
        //    if(e.profile != null)
        //        Toast.MakeText(this, $"Nombre: {e.profile.FirstName} {e.profile.LastName} {e.profile.Name} {e.profile.GetProfilePictureUri(512, 512).ToString()}", ToastLength.Long).Show();
        //}

        //protected override void OnDestroy()
        //{

        //    myProfile.StopTracking();

        //    base.OnDestroy();

        //}
        
        //Facebook Callback
        //public void OnCancel()
        //{

        //}

        //public void OnError(FacebookException error)
        //{

        //}

        //public void OnSuccess(Java.Lang.Object result)
        //{

        //    if (AccessToken.CurrentAccessToken != null)
        //    {

        //        var permisos = AccessToken.CurrentAccessToken.Permissions;

        //        GraphRequest graphRequest = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);

        //        Bundle bundle = new Bundle();

        //        bundle.PutString("fields", "id,email,first_name,last_name,birthday,gender");

        //        graphRequest.Parameters = bundle;

        //        graphRequest.ExecuteAsync();

        //    }

        //}

        //End Facebook Callback

        //Google API Client
        //public void OnConnectionFailed(ConnectionResult result)
        //{

        //    Log.Debug(TAG, "onConnectionFailed:" + result);

        //    if (!resuelto && poresolver)
        //    {

        //        if (result.HasResolution)
        //        {

        //            try
        //            {

        //                result.StartResolutionForResult(this, RC_SIGN_IN);

        //                resuelto = true;

        //            }
        //            catch (IntentSender.SendIntentException e)
        //            {

        //                Log.Error(TAG, "No se puede resolver la conexión.", e);

        //                resuelto = false;

        //                mGoogleApiClient.Connect();

        //            }

        //        }
        //        else
        //        {

        //            ShowErrorDialog(result);

        //        }

        //    }
        //    else
        //    {

        //        UpdateUI(false);

        //    }

        //}

        //class DialogInterfaceOnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        //{
        //    public Action<IDialogInterface> OnCancelImpl { get; set; }

        //    public void OnCancel(IDialogInterface dialog)
        //    {
        //        OnCancelImpl(dialog);
        //    }
        //}

        //void ShowErrorDialog(ConnectionResult connectionResult)
        //{
        //    int errorCode = connectionResult.ErrorCode;

        //    if (GooglePlayServicesUtil.IsUserRecoverableError(errorCode))
        //    {
        //        var listener = new DialogInterfaceOnCancelListener();
        //        listener.OnCancelImpl = (dialog) =>
        //        {
        //            poresolver = false;
        //            UpdateUI(false);
        //        };
        //        GooglePlayServicesUtil.GetErrorDialog(errorCode, this, RC_SIGN_IN, listener).Show();
        //    }
        //    else
        //    {
        //        //var errorstring = string.Format(GetString(Resource.String.common_google_play_services_update_title), errorCode);
        //        var errorstring = string.Format("ERROR Google Play Service", errorCode);
        //        Toast.MakeText(this, errorstring, ToastLength.Short).Show();

        //        poresolver = false;
        //        UpdateUI(false);
        //    }

        //}

        //public void OnConnected(Bundle connectionHint)
        //{
        //    Log.Debug(TAG, "onConnected:" + connectionHint);

        //    UpdateUI(true);
        //}

        //public void OnConnectionSuspended(int cause)
        //{
        //    Log.Warn(TAG, "onConnectionSuspended:" + cause);
        //}

        //void UpdateUI(bool isSignedIn)
        //{

        //    if (isSignedIn)
        //    {

        //        var person = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
        //        var name = string.Empty;
        //        if (person != null)
        //            name = person.DisplayName;
        //        var genero = person.Gender;
        //        var fechaNac = person.Birthday;
        //        var personPhoto = person.Image;


        //        var email = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);
        //        //mStatus.Text = string.Format(GetString(Resource.String.signed_in_fmt), name);
        //        mStatus.Text = name.ToString();


        //        FindViewById(Resource.Id.signinButtongoogle).Visibility = ViewStates.Gone;
        //        FindViewById(Resource.Id.SignOut).Visibility = ViewStates.Visible;

        //    }
        //    else
        //    {

        //        mStatus.Text = "Sign OUT";

        //        FindViewById(Resource.Id.signinButtongoogle).Enabled = true;
        //        //  FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
        //        FindViewById(Resource.Id.SignOut).Visibility = ViewStates.Gone;

        //    }

        //}

        //End Google API Client

        //protected override void OnStart()
        //{

        //    base.OnStart();

        //    mGoogleApiClient.Connect();

        //}

        //protected override void OnStop()
        //{

        //    base.OnStop();

        //    mGoogleApiClient.Disconnect();

        //}

        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    Log.Debug(TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);
        //    if (requestCode == RC_SIGN_IN)
        //    {
        //        if (resultCode != Result.Ok)
        //        {
        //            poresolver = false;
        //        }
        //        resuelto = false;
        //        mGoogleApiClient.Connect();
        //    }

        //    /*Facebook SDK Result*/
        //    this.facebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        //}

        //public void OnCompleted(JSONObject @object, GraphResponse response)
        //{

        //    Models.FacebookModel.FacebookResult result = JsonConvert.DeserializeObject<Models.FacebookModel.FacebookResult>(@object.ToString());

        //    Models.FirebaseModel.UserModel userModel = new Models.FirebaseModel.UserModel();

        //    userModel.Add(
        //        new Models.Entities.UserEntity() {

        //            firstName = result.first_name,

        //            lastName = result.last_name,

        //            email = result.email,

        //            gender = result.gender,

        //            birthDate = DateTime.Now,

        //            phone = "",

        //        }
        //    );

        //}

        #endregion

    }

}