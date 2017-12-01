using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;

using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;
using Android.Content;
using SMobile.Android.Configuration;

using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using FR.Ganfra.Materialspinner;
using System.Collections.Generic;
using Android.Views;

namespace SMobile.Android.Activities
{

    
    [Activity(Label = "SignupActivity")]
    public class SignupActivity : AppCompatActivity
    {

        #region Components for view
        V7Toolbar toolbar;
        EditText firstnameEditText, lastnameEditText;
        EditText dayEditText, monthEditText, yearEditText;
        RadioButton maleRadioButton, femaleRadioButton;
        EditText phoneEditText;
        FloatingActionButton nextSignupButton;

        //firebase model user
        private Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity> firebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.UserEntity>();

        #endregion
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.Signup);

            this.InitialComponents();
            
        }

        private void InitialComponents()
        {

            //Principal toolbar
            this.toolbar = FindViewById<V7Toolbar>(Resource.Id.signup_toolbar);
            toolbar.Title = "Crear Cuenta";
            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetHomeButtonEnabled(true);

            //EditText Name
            this.firstnameEditText = this.FindViewById<EditText>(Resource.Id.firstnameEditText);
            this.firstnameEditText.TextChanged += FirstnameEditText_TextChanged;

            //EditText Lastname
            this.lastnameEditText = this.FindViewById<EditText>(Resource.Id.lastnameEditText);
            this.lastnameEditText.TextChanged += LastnameEditText_TextChanged;

            //EditText's birthdate(day)
            this.dayEditText = this.FindViewById<EditText>(Resource.Id.dayEditText);
            this.dayEditText.TextChanged += DayEditText_TextChanged;

            //EditText's birthdate(month)
            this.monthEditText = this.FindViewById<EditText>(Resource.Id.monthEditText);
            this.monthEditText.TextChanged += MonthEditText_TextChanged;

            //EditText's birthdate(year)
            this.yearEditText = this.FindViewById<EditText>(Resource.Id.yearEditText);
            this.yearEditText.TextChanged += YearEditText_TextChanged;

            //RadioButton Male
            this.maleRadioButton = this.FindViewById<RadioButton>(Resource.Id.maleRadioButton);
            this.maleRadioButton.CheckedChange += MaleRadioButton_CheckedChange;

            //RadioButton Female
            this.femaleRadioButton = this.FindViewById<RadioButton>(Resource.Id.femaleRadioButton);
            this.femaleRadioButton.CheckedChange += FemaleRadioButton_CheckedChange;

            //EditText's phone
            this.phoneEditText = this.FindViewById<EditText>(Resource.Id.phoneEditText);
            this.phoneEditText.TextChanged += PhoneEditText_TextChanged;

            //FloatingActionButton Next
            this.nextSignupButton = FindViewById<FloatingActionButton>(Resource.Id.nextSignupButton);
            this.nextSignupButton.Click += NextButton_Click;
            
        }
        
        #region Events for component

        private void FirstnameEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            if (firstnameEditText.Text.Trim() != "")
            {

                this.firstnameEditText.Error = null;

                Configuration.UserConfig.currentUserEntity.firstName = this.firstnameEditText.Text;

            }
            else
            {

                this.firstnameEditText.Error = "Ingresar nombres";

            }

        }

        private void LastnameEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            if (lastnameEditText.Text.Trim() != "")
            {

                this.lastnameEditText.Error = null;

                Configuration.UserConfig.currentUserEntity.lastName = this.lastnameEditText.Text;

            }
            else
            {

                this.lastnameEditText.Error = "Ingresar apellidos";

            }

        }


        private void DayEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            try
            {
                int day = int.Parse(dayEditText.Text);
                if (day >= 1 && day <= 31)
                {

                    this.dayEditText.Error = null;

                    Configuration.UserConfig.currentUserEntity.birthDate =
                        new System.DateTime(
                            Configuration.UserConfig.currentUserEntity.birthDate.Year,
                            Configuration.UserConfig.currentUserEntity.birthDate.Month,
                            day);
                }
                else
                {
                    this.dayEditText.Error = "Día de nacimiento incorrecto";
                }
            }
            catch
            {
                //Toast.MakeText(this, "Día de nacimiento incorrecto", ToastLength.Short).Show();
                this.dayEditText.Error = "Día de nacimiento incorrecto";
            }
        }

        private void MonthEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            try
            {
                int month = int.Parse(monthEditText.Text);
                if (month >= 1 && month <= 12)
                {

                    this.monthEditText.Error = null;

                    Configuration.UserConfig.currentUserEntity.birthDate =
                        new System.DateTime(
                            Configuration.UserConfig.currentUserEntity.birthDate.Year
                            ,
                            month,
                            Configuration.UserConfig.currentUserEntity.birthDate.Day);

                }
                else
                {
                    this.monthEditText.Error = "Mes de nacimiento incorrecto";
                }
            }
            catch
            {
                this.monthEditText.Error = "Mes de nacimiento incorrecto";
            }
        }

        private void YearEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            try
            {
                int year = int.Parse(yearEditText.Text);
                if (year >= 1920 && year <= System.DateTime.Now.Year)
                {

                    this.yearEditText.Error = null;

                    Configuration.UserConfig.currentUserEntity.birthDate =
                        new System.DateTime(
                            year,
                            Configuration.UserConfig.currentUserEntity.birthDate.Month,
                            Configuration.UserConfig.currentUserEntity.birthDate.Day);

                }
                else
                {
                    this.yearEditText.Error = "Año de nacimiento incorrecto ";
                }
            }
            catch
            {
                this.yearEditText.Error = "Año de nacimiento incorrecto";
            }
        }

        private void MaleRadioButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (this.maleRadioButton.Checked)
            {
                Configuration.UserConfig.currentUserEntity.gender = "M";
            }
            else
            {
                Configuration.UserConfig.currentUserEntity.gender = "F";
            }
        }

        private void FemaleRadioButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (this.maleRadioButton.Checked)
            {
                Configuration.UserConfig.currentUserEntity.gender = "M";
            }
            else
            {
                Configuration.UserConfig.currentUserEntity.gender = "F";
            }
        }
        
        private void PhoneEditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {

            if (phoneEditText.Text.Length == 8)
            {

                this.phoneEditText.Error = null;

                Configuration.UserConfig.currentUserEntity.phone = this.phoneEditText.Text;

            }
            else
            {

                this.phoneEditText.Error = "Teléfono incorrecto";

            }

        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {

            this.ValidateEntity(Configuration.UserConfig.currentUserEntity);

            this.StartSignupImageActivity();

        }

        #endregion

        #region Private functions

        private void ValidateEntity(Models.Entities.UserEntity user)
        {

            

        }

        private void StartSignupImageActivity()
        {

            Intent intent = new Intent(this, typeof(SignupImageActivity));

            this.StartActivity(intent);

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