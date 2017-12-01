using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Firebase.Database;
using System.Threading.Tasks;
using Android.Support.V4.Widget;

using AppCompatActivity = Android.Support.V7.App.AppCompatActivity;

namespace SMobile.Android.Fragments
{
    public class BusinessFragment : Fragment, IValueEventListener
    {

        #region Components for view

        private RecyclerView recyclerView;
        private Helpers.Business.BusinessRecyclerViewAdapter adapter;
        private Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity> firebaseModels;
        //private ProgressBar progressBar;
        private SwipeRefreshLayout refresher;

        private DatabaseReference mDatabase;
        private DatabaseReference products;

        #endregion

        private void InitialComponents(View view)
        {

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.businessRecyclerView);

            this.firebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity>();

            //this.progressBar = view.FindViewById<ProgressBar>(Resource.Id.businessProgressBar);

            this.refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            this.refresher.SetColorSchemeColors(Resource.Color.slide1, Resource.Color.slide2, Resource.Color.slide3);
            this.refresher.Refresh += Refresher_Refresh;


            this.mDatabase = FirebaseDatabase
                .GetInstance(Configuration.FirebaseConfig.App, Configuration.FirebaseConfig.FIREBASE_URL)
                .Reference;
            this.products = this
                .mDatabase
                .Child(Models.Entities.BusinessEntity.BUSINESS_NAME);
            this.products.AddValueEventListener(this);

        }
        
        #region Static functions and properties
            public static BusinessFragment NewInstance() =>
                new BusinessFragment { Arguments = new Bundle() };
        #endregion

        #region Override functions and properties

        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.business_fragment_layout, container, false);
            
            return view;

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {

            this.InitialComponents(view);
            

        }

        public override void OnStart()
        {

            Helpers.Permissions.ManagePermission.RequestPermission(
                this,
                new string[] {
                    "android.permission.CALL_PHONE",
                });
            
            base.OnStart();
        }

        #endregion

        #region Private functions

        private async Task<List<Models.Entities.BusinessEntity>> GetBusinessList()
        {

            var listObject = await this.firebaseModels.List(Models.Entities.BusinessEntity.BUSINESS_NAME);

            List<Models.Entities.BusinessEntity> businessList = new List<Models.Entities.BusinessEntity>();

            foreach (var item in listObject)
            {

                businessList.Add(new Models.Entities.BusinessEntity() {

                    uid = item.Key,

                    name = item.Object.name,

                    Bhours = item.Object.Bhours,

                    phone = item.Object.phone,

                    addres = item.Object.addres,

                    imageUrl = item.Object.imageUrl,

                    RUC = item.Object.RUC,

                    lat = item.Object.lat,

                    lon = item.Object.lon,

                });

            }

            return businessList;

        }

        private async Task LoadBusinessList()
        {

            this.ShowProgressBar(true);

            this.adapter = new Helpers.Business.BusinessRecyclerViewAdapter(await this.GetBusinessList(), Context);

            this.recyclerView.HasFixedSize = true;

            var layoutManager = new LinearLayoutManager(Context);

            this.recyclerView.SetLayoutManager(layoutManager);

            this.recyclerView.SetAdapter(this.adapter);

            this.ShowProgressBar(false);

        }
        
        private void ShowProgressBar(bool IsVisible)
        {
            this.refresher.Refreshing = IsVisible;
        }

        private async void Refresher_Refresh(object sender, EventArgs e)
        {

            await this.LoadBusinessList();

            //this.ShowProgressBar(false);

        }

        #endregion

        #region Implements of Interfaces

        void IValueEventListener.OnCancelled(DatabaseError error)
        {
            
        }

        async void IValueEventListener.OnDataChange(DataSnapshot snapshot)
        {

            await this.LoadBusinessList();

        }

        #endregion

    }

}