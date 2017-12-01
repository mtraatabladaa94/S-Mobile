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
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using Firebase.Database;

namespace SMobile.Android.Fragments
{
    public class ProductsFragment : Fragment, IValueEventListener
    {

        #region Components for view

        RecyclerView recyclerView;
        Helpers.Products.ProductsRecyclerViewAdapter adapter;
        RecyclerView.LayoutManager layoutManger;
        private SwipeRefreshLayout refresher;

        private Models.FirebaseModel.FirebaseModels<Models.Entities.ProductEntity> productFirebaseModels;
        private Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity> businessFirebaseModels;

        private DatabaseReference mDatabase;
        private DatabaseReference products;

        #endregion

        private void InitialComponents(View view)
        {
            
            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.productsRecyclerView);

            this.productFirebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.ProductEntity>();
            this.businessFirebaseModels = new Models.FirebaseModel.FirebaseModels<Models.Entities.BusinessEntity>();

            this.refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            this.refresher.SetColorSchemeColors(Resource.Color.slide1, Resource.Color.slide2, Resource.Color.slide3);
            this.refresher.Refresh += Refresher_Refresh;

            this.mDatabase = FirebaseDatabase
                .GetInstance(Configuration.FirebaseConfig.App, Configuration.FirebaseConfig.FIREBASE_URL)
                .Reference;
            this.products = this
                .mDatabase
                .Child(Models.Entities.ProductEntity.PRODUCT_NAME);
            this.products.AddValueEventListener(this);

        }

        #region Static functions and properties

        public static ProductsFragment NewInstance() =>
            new ProductsFragment { Arguments = new Bundle() };

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

            View view = inflater.Inflate(Resource.Layout.products_fragment_layout, container, false);

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

        private async Task<List<Models.Entities.ProductEntity>> GetProductsList(string URL)
        {

            var listObject = await this.productFirebaseModels.List($"{Models.Entities.ProductEntity.PRODUCT_NAME}/{URL}");

            List<Models.Entities.ProductEntity> productsList = new List<Models.Entities.ProductEntity>();

            foreach (var item in listObject)
            {

                productsList.Add(new Models.Entities.ProductEntity()
                {

                    uid = item.Key,

                    Features = item.Object.Features,

                    InStock = item.Object.InStock,

                    Name = item.Object.Name,

                    Price = item.Object.Price,

                    Ranking = item.Object.Ranking,

                    Star1 = item.Object.Star1,

                    Star2 = item.Object.Star2,

                    Star3 = item.Object.Star3,

                    Star4 = item.Object.Star4,

                    Star5 = item.Object.Star5,

                    UrlImage = item.Object.UrlImage,

                    Votes = item.Object.Votes,

                });

            }

            return productsList;

        }

        private async Task<List<Models.Entities.ProductsWithBusinessEntity>> GetProductsWithBusinessList()
        {

            var business = await this.GetBusinessList();

            List<Models.Entities.ProductsWithBusinessEntity> productsWithBusiness =
                new List<Models.Entities.ProductsWithBusinessEntity>();

            foreach (var businessItem in business)
            {

                foreach (var productItem in await this.GetProductsList(businessItem.uid))
                {

                    productsWithBusiness.Add(new Models.Entities.ProductsWithBusinessEntity() {

                        //Data Business
                        businessUid = businessItem.uid,
                        nameBusiness = businessItem.name,
                        phone = businessItem.phone,
                        addres = businessItem.addres,
                        businessImageUrl = businessItem.imageUrl,
                        RUC = businessItem.RUC,
                        lat = businessItem.lat,
                        lon = businessItem.lon,

                        //Data Products
                        productUid = productItem.uid,
                        Features = productItem.Features,
                        InStock = productItem.InStock,
                        nameProduct = productItem.Name,
                        Price = productItem.Price,
                        Ranking = productItem.Ranking,
                        Star1 = productItem.Star1,
                        Star2 = productItem.Star2,
                        Star3 = productItem.Star3,
                        Star4 = productItem.Star4,
                        Star5 = productItem.Star5,
                        productImageUrl = productItem.UrlImage,
                        Votes = productItem.Votes,

                    });

                }

            }

            return productsWithBusiness;

        }

        private async Task LoadProductsList()
        {

            this.ShowProgressBar(true);

            this.adapter = new Helpers.Products.ProductsRecyclerViewAdapter(await this.GetProductsWithBusinessList(), Context);

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

            await this.LoadProductsList();

        }

        #endregion

        #region Implements of Interfaces
        
        async void IValueEventListener.OnCancelled(DatabaseError error)
        {
            
        }

        async void IValueEventListener.OnDataChange(DataSnapshot snapshot)
        {

            await this.LoadProductsList();

        }

        #endregion

    }

}