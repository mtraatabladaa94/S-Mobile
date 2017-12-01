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
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Firebase.Database;

namespace SMobile.Android.Fragments
{

    public class OffersFragment : Fragment, IValueEventListener
    {

        #region Componetns  for view

        RecyclerView recyclerView;
        Helpers.Offers.OffersRecyclerViewAdapter adapter;
        RecyclerView.LayoutManager layoutManger;
        private SwipeRefreshLayout refresher;

        private Models.FirebaseModel.FirebaseModels<Models.Entities.OfferEntity> offerFirebaseModels;
        private Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity> businessFirebaseModels;

        private DatabaseReference mDatabase;
        private DatabaseReference products;

        #endregion

        public void InitialComponents(View view)
        {

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.offersRecyclerView);

            this.offerFirebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.OfferEntity>();
            this.businessFirebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity>();

            this.refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            this.refresher.SetColorSchemeColors(Resource.Color.slide1, Resource.Color.slide2, Resource.Color.slide3);
            this.refresher.Refresh += Refresher_Refresh;

            this.mDatabase = FirebaseDatabase
                .GetInstance(Configuration.FirebaseConfig.App, Configuration.FirebaseConfig.FIREBASE_URL)
                .Reference;
            this.products = this
                .mDatabase
                .Child(Models.Entities.OfferEntity.OFFER_NAME);
            this.products.AddValueEventListener(this);
        }

        #region Static functions

        public static OffersFragment NewInstance() => new OffersFragment { Arguments = new Bundle() };

        #endregion

        #region Overrides functions

        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.offers_fragment_layout, container, false);

            return view;

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {

            this.InitialComponents(view);
            
        }

        #endregion

        #region Private functions

        private void ShowProgressBar(bool IsVisible)
        {
            this.refresher.Refreshing = IsVisible;
        }

        #endregion

        #region Async functions

        private async Task<List<Models.Entities.BusinessEntity>> GetBusinessList()
        {

            var listObject = await this.businessFirebaseModels.List(Models.Entities.BusinessEntity.BUSINESS_NAME);

            List<Models.Entities.BusinessEntity> businessList = new List<Models.Entities.BusinessEntity>();

            foreach (var item in listObject)
            {

                businessList.Add(new Models.Entities.BusinessEntity()
                {

                    uid = item.Key,

                    name = item.Object.name,

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

        private async Task<List<Models.Entities.OfferEntity>> GetOffersList()
        {

            var listObject = await this.offerFirebaseModels.List(Models.Entities.OfferEntity.OFFER_NAME);

            List<Models.Entities.OfferEntity> offersList = new List<Models.Entities.OfferEntity>();
            
            foreach (var item in listObject)
            {

                offersList.Add(new Models.Entities.OfferEntity() {

                    uid = item.Key,

                    sendDate = item.Object.sendDate,

                    imageUrl = item.Object.imageUrl,

                    title = item.Object.title,

                    description = item.Object.description,

                    initialDate = item.Object.initialDate,

                    endDate = item.Object.endDate,

                    businessUid = item.Object.businessUid,

                });

            }

            return offersList;

        }

        private async Task<List<Models.Entities.OffersWithBusinessEntity>> GetOffersWithBusinessList()
        {

            var business = await this.GetBusinessList();

            var offers = await this.GetOffersList();

            List<Models.Entities.OffersWithBusinessEntity> offersWithBusiness =
                new List<Models.Entities.OffersWithBusinessEntity>();

            foreach (var item in offers)
            {

                var businessInOffer = business.Where(c=> c.uid == item.businessUid).FirstOrDefault();

                if (businessInOffer != null)
                {

                    offersWithBusiness.Add(new Models.Entities.OffersWithBusinessEntity() {

                        //Data Offers
                        offerUid = item.uid,
                        sendDate = item.sendDate,
                        offerImageUrl = item.imageUrl,
                        title = item.title,
                        description = item.description,
                        initialDate = item.initialDate,
                        endDate = item.endDate,

                        //Data Business
                        businessUid = item.businessUid,
                        name = businessInOffer.name,
                        phone = businessInOffer.phone,
                        addres = businessInOffer.addres,
                        businessImageUrl = businessInOffer.imageUrl,
                        RUC = businessInOffer.RUC,
                        lat = businessInOffer.lat,
                        lon = businessInOffer.lon,

                    });

                }

            }

            return offersWithBusiness;

        }

        private async Task LoadOffersList()
        {

            this.ShowProgressBar(true);
            
            this.adapter = new Helpers.Offers.OffersRecyclerViewAdapter(await this.GetOffersWithBusinessList(), Context);

            recyclerView.HasFixedSize = true;

            this.layoutManger = new LinearLayoutManager(Context);

            recyclerView.SetLayoutManager(this.layoutManger);

            recyclerView.SetAdapter(this.adapter);

            this.ShowProgressBar(false);

        }

        #endregion
        
        #region Events of components

        private async void Refresher_Refresh(object sender, EventArgs e)
        {

            await this.LoadOffersList();
            
        }

        #endregion

        #region Implements of Interfaces
        
        void IValueEventListener.OnCancelled(DatabaseError error)
        {
            
        }

        async void IValueEventListener.OnDataChange(DataSnapshot snapshot)
        {

            await this.LoadOffersList();

        }

        #endregion

    }

}