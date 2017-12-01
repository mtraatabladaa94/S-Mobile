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

using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Firebase.Auth;
using Android.Gms.Tasks;
using Java.Lang;

namespace SMobile.Android.Activities
{
    [Activity(Label = "SignupAccountActivity")]
    public class SignupAccountActivity :
        AppCompatActivity,
        IOnSuccessListener,
        IOnFailureListener
    {

        #region Components for view
        V7Toolbar toolbar;
        EditText emailEditText, passwordEditText;
        FloatingActionButton nextSignupButton;

        private Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity> firebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity>();
        private ProgressDialog progress;
        #endregion

        private void InitialComponents()
        {

            //Init auth
            //this.StartFirebaseAuth();

            //Toolbar
            this.toolbar = FindViewById<V7Toolbar>(Resource.Id.signup_toolbar);
            toolbar.Title = "Datos de usuario";
            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetHomeButtonEnabled(true);

            //EditText Email
            this.emailEditText = this.FindViewById<EditText>(Resource.Id.emailEditText);
            this.emailEditText.TextChanged += EmailEditText_TextChanged;

            //EditText Password
            this.passwordEditText = this.FindViewById<EditText>(Resource.Id.passwordEditText);
            this.passwordEditText.TextChanged += PasswordEditText_TextChanged;

            //FloatingActionButton Next
            this.nextSignupButton = FindViewById<FloatingActionButton>(Resource.Id.nextSignupButton);
            this.nextSignupButton.Click += NextButton_Click;

        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.SignupAccount);

            this.InitialComponents();

        }

        #region Events for components

        private void EmailEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            if (emailEditText.Text.Trim() != "")
            {

                this.emailEditText.Error = null;

                Configuration.UserConfig.currentUserEntity.email = this.emailEditText.Text;

            }
            else
            {

                this.emailEditText.Error = "Ingresar email";

            }

        }

        private void PasswordEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            if (passwordEditText.Text.Trim() != "")
            {

                if (passwordEditText.Text.Length >= 8)
                {

                    this.passwordEditText.Error = null;

                    Configuration.UserConfig.currentUserEntity.password = this.passwordEditText.Text;

                }
                else
                {

                    this.passwordEditText.Error = "Debe contener mas de 8 carácteres";

                }

            }
            else
            {

                this.passwordEditText.Error = "Ingresar contraseña";

            }

        }

        private void NextButton_Click(object sender, EventArgs e)
        {

            if(this.ValidateEntity())
                this.SignupUser(this.emailEditText.Text, this.passwordEditText.Text);

        }

        #endregion

        #region Private functions

        private bool ValidateEntity()
        {

            bool temp = true;

            if (this.emailEditText.Text.Trim().Length == 0)
            {

                this.emailEditText.Error = "Ingresar email";

                temp = false;

            }

            if (this.passwordEditText.Text.Trim().Length == 0)
            {

                this.passwordEditText.Error = "Ingresar contraseña";

                temp = false;

            }

            return temp;
        }

        private void FindUserByEmail(string email)
        {

            FirebaseUser firebaseUser;
            

        }

        private void SignupUser(string email, string password)
        {

            this.nextSignupButton.Enabled = false;

            this.ShowProgressDialog();

            try
            {

                Configuration
                    .FirebaseConfig
                    .Auth
                    .CreateUserWithEmailAndPassword(email, password)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);

            }
            catch (Firebase.FirebaseException ex)
            {

                Toast.MakeText(this, $"Error al intentar registrarse. Error: {ex.Message}", ToastLength.Short).Show();

                this.nextSignupButton.Enabled = true;

            }
            catch (Java.Lang.Exception ex)
            {

                Toast.MakeText(this, $"Error al intentar registrarse. Error: {ex.Message}", ToastLength.Short).Show();

                this.nextSignupButton.Enabled = true;

            }

        }

        private void StartMainActivity()
        {

            Intent intent = new Intent(this, typeof(MainActivity));

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

        #endregion

        #region Async functions

        private async void Save()
        {

            try
            {
                
                await this.firebaseModels.Add(
                    Configuration.UserConfig.currentUserEntity
                );

            }
            catch (Java.Lang.Exception ex)
            {

                Toast.MakeText(this, $"No se ha podido guardar. Error: {ex.Message}", ToastLength.Short).Show();

                this.nextSignupButton.Enabled = true;

            }

        }
        
        #endregion

        #region Implementation of Interfaces

        void IOnFailureListener.OnFailure(Java.Lang.Exception e)
        {

            Toast.MakeText(this, $"Lo sentimos ha ocurrido un error. Descripción: {e.Message}", ToastLength.Short).Show();

            this.nextSignupButton.Enabled = true;

            this.progress.Dismiss();

        }

        void IOnSuccessListener.OnSuccess(Java.Lang.Object result)
        {

            this.Save();

            this.StartMainActivity();

            this.nextSignupButton.Enabled = true;

            this.progress.Dismiss();

            this.FinishAffinity();

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

    }

}