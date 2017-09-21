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

    
    [Activity(Label = "SignupActivity")]
    public class SignupActivity : AppCompatActivity
    {

        //declarate button stages
        Button nextSignupButton, nextSignupGenderButton, nextSignupEmailOrPhoneButton;

        //object user
        private Models.Entities.UserEntity userEntity = new Models.Entities.UserEntity();

        //counter stage
        private byte nextStage = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.Signup);

            this.nextSignupButton = FindViewById<Button>(Resource.Id.nextSignupButton);

            this.nextSignupButton.Click += new System.EventHandler(this.nextButton_Click);
            
        }

        private void nextButton_Click()
        {

            //Evaluate stages
            switch (this.nextStage)
            {
                case 1:

                    this.nextStageOne();

                    break;

                case 2:

                    this.nextStageTwo();

                    break;
                case 3:

                    this.nextStageThree();

                    break;
            }

            //Increase stage
            this.nextStage++;

        }

        //First view - Receipt firstname and lastname
        private void nextStageOne()
        {

            EditText firstName, lastName;

            firstName = this.FindViewById<EditText>(Resource.Id.firstnameEditText);

            lastName = this.FindViewById<EditText>(Resource.Id.lastnameEditText);

            if (string.IsNullOrWhiteSpace(firstName.Text))
            {

                Toast.MakeText(this, "Ingresar nombre", ToastLength.Short).Show();

            }
            else
            {

                if (string.IsNullOrWhiteSpace(lastName.Text))
                {

                    Toast.MakeText(this, "Ingresar apellido", ToastLength.Short).Show();

                }
                else
                {

                    //Asignando campos al objeto
                    this.userEntity.firstName = firstName.Text;
                    this.userEntity.lastName = lastName.Text;

                    

                    //cambiar de layout
                    this.SetContentView(Resource.Layout.SignupGender);

                }

            }

        }

        //Second view - Receipt gender
        private void nextStageTwo()
        {

            RadioButton male, female;

            male = this.FindViewById<RadioButton>(Resource.Id.maleRadioButton);

            female = this.FindViewById<RadioButton>(Resource.Id.femaleRadioButton);

            //Evaluate if there is selected
            if (!male.Checked && !female.Checked)
            {

                Toast.MakeText(this, "Seleccionar su género", ToastLength.Short).Show();

            }
            else
            {

                //Asignando campos al objeto
                this.userEntity.gender = male.Checked ? "M" : "F";

                //cambiar de layout
                this.SetContentView(Resource.Layout.SignupEmailOrPhone);

            }

        }

        //Third view - Receipt email and phone
        private void nextStageThree()
        {

            EditText email, phone;

            email = this.FindViewById<EditText>(Resource.Id.emailEditText);

            phone = this.FindViewById<EditText>(Resource.Id.phoneEditText);

            //if (string.IsNullOrWhiteSpace(email.Text) && string.IsNullOrWhiteSpace(phone.Text))
            //{
            //}

            this.registerUser();

        }

        private void registerUser()
        {

            Models.FirebaseModel.UserModel userModel = new Models.FirebaseModel.UserModel();

            userModel.Add(this.userEntity);

        }




        //códigos de prueba
        private void testFirebaseModel()
        {

            try
            {

                Models.FirebaseModel.UserModel userModel = new Models.FirebaseModel.UserModel();

                userModel.Add(
                    new Models.Entities.UserEntity()
                    {
                        firstName = "Michel Roberto",
                        lastName = "Traña Tablada",
                        gender = "h",
                        birthDate = "02/09/1994",
                        email = "mtraatabladaa94@gmail.com",
                        phone = "8367 - 1719"
                    }
                );

            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
            }

        }

        

    }

}