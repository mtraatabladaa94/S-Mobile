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

        private Models.Entities.UserEntity userEntity = new Models.Entities.UserEntity();

        private byte nextState = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.SignupGender);

        }

        private void nextButton_Click()
        {

            switch (this.nextState)
            {
                case 1:

                    break;

                case 2:

                    break;
                case 3:

                    break;
            }

        }

        private void addNameAndSurnameView()
        {

            

        }

        

    }

}