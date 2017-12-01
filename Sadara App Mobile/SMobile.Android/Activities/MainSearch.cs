using Android.App;
using Android.OS;
using Android.Support.V7.App;
using V7Toolb = Android.Support.V7.Widget.Toolbar;
using V7Tools = Android.Support.V7.Widget.SearchView;
//using ProgressDialog = Android.App.ProgressDialog;
using Android.Views;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Support.V7.Widget;
using SMobile.Android.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;

namespace SMobile.Android.Activities
{
    [Activity(Label = "Sadara Mobile", MainLauncher = false, Theme = "@style/Main.Theme.SadaraTheme")]
    public class MainSearch : AppCompatActivity
    {
        private RecyclerView RecyclerProducts;
        private RecyclerViewAdapterProducts AdapterProducts;
        private RecyclerView.LayoutManager layoutmanager;
        private List<Models.Entities.ProductEntity> ListProducts = new List<Models.Entities.ProductEntity>();
        private List<Models.Entities.ProductEntity> ListProductSearch = new List<Models.Entities.ProductEntity>();
        private List<Models.Entities.ProductEntity> LP = new List<Models.Entities.ProductEntity>();
        private List<Models.Entities.ProductEntity> LPfilter = new List<Models.Entities.ProductEntity>();
        ProgressDialog pd;
        private TextView Empty;
        private CheckBox chk_Price;
        private CheckBox chk_ranking;

           protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Cargar el Layout
            this.SetContentView(Resource.Layout.MainSearch);
            FirebaseApp.InitializeApp(ApplicationContext);

            var toolbar = FindViewById<V7Toolb>(Resource.Id.ToolbarSearch);
            toolbar.InflateMenu(Resource.Menu.search_menu);
            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            var search = toolbar.Menu.FindItem(Resource.Id.search);
            var searchView = search.ActionView.JavaCast<V7Tools>();

            searchView.QueryTextChange += SearchView_QueryTextChange;
            searchView.QueryTextSubmit += SearchView_QueryTextSubmit;


            //RecyclerView ---Lista de Productos

            await LoadProducts();
            RecyclerProducts = FindViewById<RecyclerView>(Resource.Id.RVProducts);
            RecyclerProducts.HasFixedSize = true;
            layoutmanager = new LinearLayoutManager(this);
            RecyclerProducts.SetLayoutManager(layoutmanager);

            if (ListProducts.Count==0)
            {
                Empty = FindViewById<TextView>(Resource.Id.Empty);
                Empty.Visibility = ViewStates.Visible;
            }

            AdapterProducts = new RecyclerViewAdapterProducts(ListProducts, this);


            RecyclerProducts.SetAdapter(AdapterProducts);

            //Checkbox Precio y Checkbox valoración
            chk_Price = FindViewById<CheckBox>(Resource.Id.check_price);
            chk_ranking = FindViewById<CheckBox>(Resource.Id.check_ranking);
            chk_Price.Click += Chk_Price_Click;
            chk_ranking.Click += Chk_ranking_Click;

        }
       
      

        private void Chk_ranking_Click(object sender, EventArgs e)
        {
            if (chk_ranking.Checked)
            {
                if(ListProductSearch.Count==0)
                {
                ListProductSearch = ListProducts.OrderByDescending(r => r.Ranking).ThenByDescending(r => r.Name).ToList();
                AdapterProducts = new RecyclerViewAdapterProducts(ListProductSearch, this);
                RecyclerProducts.SetAdapter(AdapterProducts);
                chk_Price.Checked = false;
                }
                else
                {
                    LPfilter = ListProductSearch.OrderByDescending(r => r.Ranking).ThenByDescending(r => r.Name).ToList();
                    AdapterProducts = new RecyclerViewAdapterProducts(LPfilter, this);
                    RecyclerProducts.SetAdapter(AdapterProducts);
                    chk_Price.Checked = false;
                }
                
            }
            else
            {
                ListProductSearch.Clear();
                AdapterProducts = new RecyclerViewAdapterProducts(ListProducts, this);
                RecyclerProducts.SetAdapter(AdapterProducts);
            }
        }

        private void Chk_Price_Click(object sender, EventArgs e)
        {
            if (chk_Price.Checked)
            {
                if (ListProductSearch.Count == 0)
                {
                    //ordenar la lista por precio 
                    ListProductSearch = ListProducts.OrderBy(p => p.Price).ToList();
                    AdapterProducts = new RecyclerViewAdapterProducts(ListProductSearch, this);
                    RecyclerProducts.SetAdapter(AdapterProducts);
                    chk_ranking.Checked = false;
                }
                else
                {
                    LPfilter = ListProductSearch.OrderBy(r => r.Price).ToList();
                    AdapterProducts = new RecyclerViewAdapterProducts(LPfilter, this);
                    RecyclerProducts.SetAdapter(AdapterProducts);
                    chk_ranking.Checked = false;

                }
            }
            else
            {
                ListProductSearch.Clear();
                AdapterProducts = new RecyclerViewAdapterProducts(ListProducts, this);
                RecyclerProducts.SetAdapter(AdapterProducts);
            }
        }

        private async Task<int> LoadProducts()
        {
            //--Dialogo de progreso
            pd = new ProgressDialog(this);
            pd.Indeterminate = false;
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            pd.SetMessage("Cargando...");
            pd.SetCancelable(false);
            pd.Show();
            //Cargar todos los productos de la bd
            var sp = new Service.SearchProducts();

            LP = await sp.ListProducts();

                foreach (var p in LP)
                {
                    ListProducts.Add(new Models.Entities.ProductEntity()
                    {
                        uidCompany=p.uidCompany,
                        uid =p.uid,
                        UrlImage = p.UrlImage,
                        Name = p.Name,
                        Features = p.Features,
                        InStock=p.InStock,
                        Price=p.Price,
                        Votes=p.Votes,
                        Star1=p.Star1,
                        Star2=p.Star2,
                        Star3=p.Star3,
                        Star4=p.Star4,
                        Star5=p.Star5,
                        Ranking=p.Ranking
                       
                    });
                }
            pd.Dismiss();
             
            return 0;
        }

        public void ShowProgressDialog()
        {
            pd = new ProgressDialog(this);
            pd.Indeterminate = false;
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            pd.SetMessage("Cargando Productos...");
            pd.SetCancelable(false);
            pd.Show();
            
        }

        //-----------------------BUSQUEDA EN TOOLBAR-----------------//
        private async void SearchView_QueryTextSubmit(object sender, V7Tools.QueryTextSubmitEventArgs e)
        {
           
            string query = e.Query.ToString();

            if(query.Length==0)
            {
                AdapterProducts = new RecyclerViewAdapterProducts(ListProducts, this);
                RecyclerProducts.SetAdapter(AdapterProducts);
            }
            else { 

            var service = new Service.ServiceLuis();
            var model = await service.GetIntent(query);

            var sp = new Service.SearchProducts();
            switch(model.topScoringIntent.intent)
            {
                case "BuscarProductos":
                    try
                    {
                        var tag = model.entities.Where(x => x.type == "pname").FirstOrDefault();                  
                        //var tag2 = model.entities.Where(x => x.type == "nproduct").FirstOrDefault();
                        //var tag3 = model.entities.Where(x => x.type == "pfeatures").FirstOrDefault();
                        String Tag = tag?.entity.ToString();
                        //String Tag2 = tag2?.entity.ToString();
                        //String Tag3 = tag3?.entity.ToString();


                            //Limpiar la Lista de busqueda--Para evitar que no se muestre busqueda anterior--------
                            ListProductSearch.Clear();
                        //----------------
                        SearchProducts_Recyclerview(Tag);  
                        //LP = await sp.Search_Product(Tag);
                        //foreach (var p in LP)
                        //{
                        //    ListProductSearch.Add(new Models.Entities.ProductEntity()
                        //    {
                        //        UrlImage = p.UrlImage,
                        //        Name = p.Name,
                        //        Features = p.Features
                        //    });
                        //}
                       // RecyclerView-- - Lista de Productos
                                                                                            // LoadProducts();
                        RecyclerProducts = FindViewById<RecyclerView>(Resource.Id.RVProducts);
                        RecyclerProducts.HasFixedSize = true;
                        layoutmanager = new LinearLayoutManager(this);
                        RecyclerProducts.SetLayoutManager(layoutmanager);
                        AdapterProducts = new RecyclerViewAdapterProducts(ListProductSearch, this);     
                        RecyclerProducts.SetAdapter(AdapterProducts);

                     }
                    catch(Exception ex)
                    {
                        ex.GetBaseException();
                        
                    }
                    
                    break;
                case "acercade":
                    Toast.MakeText(this, "Sadara APP"+ model.topScoringIntent.intent, ToastLength.Long).Show();
                    break;
                case "None":
                    Toast.MakeText(this, "No hay elementos para mostrar"+ model.topScoringIntent.intent, ToastLength.Long).Show();
                    break;
                default:
                    break;
            }
           }
        }

        private void SearchView_QueryTextChange(object sender, V7Tools.QueryTextChangeEventArgs e)
        {
           
        }

        private void Toolbar_MenuItemClick(object sender, V7Toolb.MenuItemClickEventArgs e)
        {
           
        }

        public void SearchProducts_Recyclerview(string Tag)
        {
            foreach (var p1 in ListProducts)
            {
                if ((p1.Name.ToUpper()).Contains(Tag.ToUpper())) 
                    //|| 
                    //((p1.Name.ToUpper()).Contains(Tag2.ToUpper())) ||
                    //((p1.Features.ToUpper()).Contains(Tag3.ToUpper())))
                {

                    ListProductSearch.Add(
                        new Models.Entities.ProductEntity()
                        {
                            uidCompany = p1.uidCompany,
                            uid = p1.uid,
                            UrlImage = p1.UrlImage,
                            Name = p1.Name,
                            Features = p1.Features,
                            InStock = p1.InStock,
                            Price = p1.Price,
                            Votes = p1.Votes,
                            Star1 = p1.Star1,
                            Star2 = p1.Star2,
                            Star3 = p1.Star3,
                            Star4 = p1.Star4,
                            Star5 = p1.Star5,
                            Ranking = p1.Ranking

                        }
                        );
                }//Cierre IF
            }

        }
    }
}