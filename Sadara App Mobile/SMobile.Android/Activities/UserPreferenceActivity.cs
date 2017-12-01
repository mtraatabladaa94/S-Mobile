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
using Android.Support.V7.Widget;
using Android;
using SMobile.Android.Configuration;
using Firebase;
using Android.Gms.Common;
using Android.Util;
using System.Threading.Tasks;
using Firebase.Iid;

namespace SMobile.Android.Activities
{

    [Activity(Label = "Sadara Mobile", MainLauncher = false, Icon = "@drawable/ic_isotipo_sadara")]
    public class UserPreferenceActivity : Activity
    {

        #region Components for view
        ProgressBar progressBar;
        Helpers.UserPreferenceRecyclerViewAdapter adapter;
        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManger;
        Button btnPreferences;

        List<Models.Entities.PreferenceSelectedEntity> preferences =
            new List<Models.Entities.PreferenceSelectedEntity>();
        Models.FirebaseModel.FirebaseModels<Models.Entities.PreferenceWithUserEntity> firebaseModel
            = new Models.FirebaseModel.FirebaseModels<Models.Entities.PreferenceWithUserEntity>();
        #endregion

        private void InitialComponents()
        {

            //Start Firebase
            this.StartFirebase();

            //Request permissions
            this.RequestPermissions();

            //ProgressBar
            this.progressBar = FindViewById<ProgressBar>(Resource.Id.userPreferenceProgressBar);

            //RecyclerView for show list preferences
            this.recyclerView = FindViewById<RecyclerView>(Resource.Id.preferencesRecyclerView);

            //Selección de Preferencias
            this.btnPreferences = FindViewById<Button>(Resource.Id.selectPreferencesButton);
            this.btnPreferences.Click += (e, handler) => this.SendSuscriptions();

        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            
            // Create your application here
            this.SetContentView(Resource.Layout.UserPreference);

            this.InitialComponents();

            this.LoadUsersPreferencesList();

            Task.Run(() =>
            {

                var instanceId = FirebaseInstanceId.Instance;

                instanceId.DeleteInstanceId();

            });

        }

        #region Private functions

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

        private async void SendSuscriptions()
        {
            

            var selectList = this.preferences.Where(c => c.selected).ToList();

            selectList.ForEach((item) =>
            {

                //Subscribir a los topics
                this.SubscribeToTopic(item.topic);

                //Almacenar suscripciones y preferencias de usuario
                this.SaveTopicInDatabase(item.uid);

            });

            Toast.MakeText(this, "Tus preferencias se han guardado", ToastLength.Long).Show();

            this.Finish();

        }

        private async void SaveTopicInDatabase(string uidPreference)
        {
            await this.firebaseModel.Add(new Models.Entities.PreferenceWithUserEntity()
            {

                uid = "ovp5YuTtwScaEj2bIwCb1ezTRPd2",

                uidPreference = uidPreference,

            });
        }

        private string GetTokenForApp()
        {

            var instanceId = Firebase.Iid.FirebaseInstanceId.GetInstance(Configuration.FirebaseConfig.App);

            var token = instanceId.Token;

            return token;

        }

        public void SubscribeToTopic(string topic)
        {

            if (this.IsPlayServicesAvailable())
            {

                var token = this.GetTokenForApp();

                if (!string.IsNullOrEmpty(token))
                {
                    Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic(topic);
                }
                else

                    Toast.MakeText(this, "No se cargo el Token", ToastLength.Long).Show();
                  
            }
            else
            {

                Toast.MakeText(this, this.msgText, ToastLength.Long).Show();

            }

        }

        string msgText;

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);            
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)) {
                    msgText = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    msgText = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText = "Google Play Services is available.";
                return true;
            }
        }

        public void ManageIntent()
        {

            if (Intent.Extras != null)
            {

                foreach (var key in Intent.Extras.KeySet())
                {

                    var value = Intent.Extras.GetString(key);

                    Log.Debug("SadaraFirebaseIIDService", "Key: {0} Value: {1}", key, value);

                }

            }

        }

        private void StartFirebase()
        {

            FirebaseApp.GetInstance("Sadara App Mobile");

        }

        private void AddPreferenceToList(List<Models.Entities.PreferenceEntity> preferences)
        {

            this.preferences.Clear();

            preferences.ForEach(preference => 

                this.preferences.Add(new Models.Entities.PreferenceSelectedEntity()
                {

                    uid = preference.uid,

                    name = preference.name,

                    topic = preference.topic,

                })

            );

        }

        private void ShowProgressBar(Boolean IsVisible)
        {

            progressBar.Visibility = IsVisible ? ViewStates.Visible : ViewStates.Invisible;

        }

        private async Task<List<Models.Entities.PreferenceEntity>> GetPreferenceList()
        {

            var firebaseModel = new Models.FirebaseModel.FirebaseModels<Models.Entities.PreferenceEntity>();
            
            var firebaseObjectList = await firebaseModel.List(Models.Entities.PreferenceEntity.PREFERENCE_NAME);

            List<Models.Entities.PreferenceEntity> preferences
                = new List<Models.Entities.PreferenceEntity>();

            foreach (var item in firebaseObjectList)
            {

                preferences.Add(new Models.Entities.PreferenceEntity() {
                    uid = item.Key,
                    name = item.Object.name,
                    topic = item.Object.topic,
                });

            }

            return preferences;

        }

        #endregion

        #region Async functions

        private async void LoadUsersPreferencesList()
        {
            
            this.ShowProgressBar(true);

            this.AddPreferenceToList(await this.GetPreferenceList());

            this.adapter = new Helpers.UserPreferenceRecyclerViewAdapter(this.preferences);

            recyclerView.HasFixedSize = true;

            this.layoutManger = new LinearLayoutManager(this);

            recyclerView.SetLayoutManager(this.layoutManger);

            recyclerView.SetAdapter(this.adapter);

            this.ShowProgressBar(false);

        }

        private async void CreateAllReferenceByDefault()
        {

            var firebaseModel = new Models.FirebaseModel.FirebaseModels<Models.Entities.PreferenceEntity>();

            //1
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos alimenticios",
                topic = "Productos_alimenticios",
            });
            //2
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos ferreteros",
                topic = "Productos_ferreteros",
            });
            //3
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos automotrices",
                topic = "Productos_automotrices",
            });
            //4
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Servicios de hoteles y restaurantes",
                topic = "Servicios_de_hoteles_y_restaurantes",
            });
            //5
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Ocio y diversión",
                topic = "Ocio_y_diversión",
            });
            //6
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Servicios y productos agropecuarios y agrícolas",
                topic = "Servicios_y_productos_agropecuarios_y_agrícolas",
            });
            //7
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Servicios de transporte y renta de vehículos",
                topic = "Servicios_de_transporte_y_renta_de_vehículos",
            });
            //8
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos farmacéuticos y salud",
                topic = "Productos_farmacéuticos_y_salud",
            });
            //9
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos personales, moda y para el hogar",
                topic = "Productos_personales_moda_y_para_el_hogar",
            });
            //10
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Agencia de turismo",
                topic = "Agencia_de_turismo",
            });
            //11
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Eventos especiales y entretenimiento",
                topic = "Eventos_especiales_y_entretenimiento",
            });
            //12
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Servicios de publicidad y mercadeo",
                topic = "Servicios_de_publicidad_y_mercadeo",
            });
            //13
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos y servicios tecnológicos",
                topic = "Productos_y_servicios_tecnológicos",
            });
            //14
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos y servicios deportivos",
                topic = "Productos_y_servicios_deportivos",
            });
            //15
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Productos para niños",
                topic = "Productos_para_niños",
            });
            //16
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Muebles y servicios para el hogar",
                topic = "Muebles_y_servicios_para_el_hogar",
            });
            //17
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Instrumentos musicales y accesorios",
                topic = "Instrumentos_musicales_y_accesorios",
            });
            //18
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Veterinaria y productos para mascotas",
                topic = "Veterinaria_y_productos_para_mascotas",
            });
            //19
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Joyería y relojería",
                topic = "Joyería_y_relojería",
            });
            //20
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Servicios de bufet de comida y comidería",
                topic = "Servicios_de_bufet_de_comida_y_comidería",
            });
            //21
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Zapatería de moda y ortopédica",
                topic = "Zapatería_de_moda_y_ortopédica",
            });
            //22
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Tienda de abarrotería",
                topic = "Tienda_de_abarrotería",
            });
            //23
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Cosméticos y servicios de belleza",
                topic = "Cosméticos_y_servicios_de_belleza",
            });
            //24
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Alquiler de equipos y servicios para fiestas y eventos",
                topic = "Alquiler_de_equipos_y_servicios_para_fiestas_y_eventos",
            });
            //25
            await firebaseModel.Add(new Models.Entities.PreferenceEntity()
            {
                name = "Educación y capacitación",
                topic = "Educación_y_capacitación",
            });

        }

        #endregion
        
    }

}