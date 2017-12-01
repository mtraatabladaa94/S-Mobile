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
using Android.Net;

using AndroidUri = Android.Net.Uri;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using Android.Provider;
using Java.IO;
using Android.Support.Design.Widget;
using Firebase.Storage;
using Android.Gms.Tasks;
using Java.Lang;

using JavaMath = Java.Lang.Math;
using Android.Support.V4.Graphics.Drawable;
using Firebase.Auth;

namespace SMobile.Android.Activities
{

    [Activity(Label = "SignupImageActivity")]
    public class SignupImageActivity : AppCompatActivity, IOnProgressListener, IOnSuccessListener, IOnFailureListener
    {

        #region Components for view
        V7Toolbar toolbar;
        ImageView profileImageView;
        FloatingActionButton nextSignupButton;

        AndroidUri imageUri;
        const int PICK_IMAGE_REQUEST = 2994;
        Bitmap profileBitmap;
        StorageReference storageReference;
        ProgressDialog progress;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.signup_image_layout);

            this.InitialComponents();

        }

        private void InitialComponents()
        {

            //Principal toolbar
            this.toolbar = FindViewById<V7Toolbar>(Resource.Id.signup_toolbar);
            toolbar.Title = "Datos de usuario";
            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetHomeButtonEnabled(true);

            //Profile imageview
            this.profileImageView = this.FindViewById<ImageView>(Resource.Id.profileImageView);
            if (Configuration.UserConfig.currentUserEntity.gender.Equals("M"))
                this.profileImageView.SetImageResource(Resource.Drawable.ic_user_man);
            else
                this.profileImageView.SetImageResource(Resource.Drawable.ic_user_woman);
            this.profileImageView.Click += ProfileImageView_Click;

            //Next button
            this.nextSignupButton = FindViewById<FloatingActionButton>(Resource.Id.nextSignupButton);
            this.nextSignupButton.Click += NextButton_Click;

            //Progress Dialog

        }
        
        #region Events for components

        private void ProfileImageView_Click(object sender, EventArgs e)
        {

            this.GetImage();

        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            this.SendImageFirebase();
        }

        #endregion

        #region Private functions

        private void GetImage()
        {

            Intent intent = new Intent();

            intent.SetType("image/*");

            intent.SetAction(Intent.ActionGetContent);

            this.StartActivityForResult(Intent.CreateChooser(intent, "Seleccionar Imagen"), PICK_IMAGE_REQUEST);

        }

        private void ImageResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {

            if (requestCode == PICK_IMAGE_REQUEST &&
                resultCode == Result.Ok &&
                data != null &&
                data.Data != null)
            {

                this.imageUri = data.Data;

                try
                {

                    this.profileBitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, this.imageUri);

                    this
                        .profileImageView
                        .SetImageDrawable(
                            this.RoundedImage(
                                this.profileBitmap,
                                152 * Resources.DisplayMetrics.Density)
                            );

                }
                catch (IOException ex)
                {

                    ex.PrintStackTrace();

                    Toast.MakeText(this, $"Ha ocurrido un error. Descripción: {ex.Message}", ToastLength.Long).Show();

                }

            }

        }

        private RoundedBitmapDrawable RoundedImage(Bitmap image, float cornerRadius)
        {

            RoundedBitmapDrawable roundedDrawable =
                        RoundedBitmapDrawableFactory.Create(Resources, image);

            roundedDrawable.CornerRadius = cornerRadius;

            roundedDrawable.Circular = true;

            return roundedDrawable;

        }

        private void ShowProgressDialog()
        {

            this.progress = new ProgressDialog(this);

            this.progress.SetTitle("Cargando...");

            this.progress.Window.SetType(WindowManagerTypes.SystemAlert);

            this.progress.Show();

        }

        private void SendImageFirebase()
        {

            if (this.imageUri != null)
            {

                this.ShowProgressDialog();

                StorageReference storageReference = FirebaseStorage
                    .GetInstance(Configuration.FirebaseConfig.App)
                    .GetReferenceFromUrl($"{Configuration.FirebaseConfig.FIREBASE_STORAGE_URL}{Configuration.FirebaseConfig.PROFILE_IMAGES_STORAGE_URL}");

                var childStorage = storageReference.Child(Guid.NewGuid().ToString());

                childStorage
                    .PutFile(this.imageUri)
                    .AddOnProgressListener(this)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);

            }
            else
            {

                this.StartSignupAccountActivity();

            }
            
        }

        private void StartSignupAccountActivity()
        {

            Intent intent = new Intent(this, typeof(SignupAccountActivity));

            this.StartActivity(intent);

        }

        private void SetUrlImageUser(string url)
        {

            //var user = Configuration.FirebaseConfig.Auth.CurrentUser;

            //var updates = new UserProfileChangeRequest.Builder()
            //    .SetPhotoUri(AndroidUri.Parse(url))
            //    .Build();

            //try
            //{

            //    await user.UpdateProfileAsync(updates);

            //}
            //catch (Java.Lang.Exception ex)
            //{

            //    Toast.MakeText(this, $"Ha ocurrido un error. Descripción: {ex.Message}", ToastLength.Long).Show();

            //}

            Configuration.UserConfig.currentUserEntity.imageUrl = url;

        }

        #endregion

        #region Override functions

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            this.ImageResult(requestCode, resultCode, data);

        }

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

        #region Implementations of interfaces

        void IOnProgressListener.OnProgress(Java.Lang.Object snapshot)
        {

            var taskSnapShot = (UploadTask.TaskSnapshot)snapshot;

            long progress = JavaMath.Round(100.0 * taskSnapShot.BytesTransferred / taskSnapShot.TotalByteCount);

            this.progress.SetMessage($"Procesando {progress} %");

        }

        void IOnSuccessListener.OnSuccess(Java.Lang.Object result)
        {

            var url = ((UploadTask.TaskSnapshot)result).DownloadUrl.ToString();

            this.SetUrlImageUser(url);

            this.progress.Dismiss();

            this.StartSignupAccountActivity();

        }

        void IOnFailureListener.OnFailure(Java.Lang.Exception e)
        {

            Toast.MakeText(this, $"Error al subir la imagen, intente nuevamente. Descripción: {e.Message}", ToastLength.Long).Show();

        }

        #endregion
        
    }

}