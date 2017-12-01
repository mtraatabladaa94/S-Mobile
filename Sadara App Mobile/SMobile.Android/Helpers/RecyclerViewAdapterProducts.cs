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
using static Android.Support.V7.Widget.RecyclerView;
using Android.Support.V7.Widget;
using SMobile.Android.Models.Entities;
using Android.Gms.Tasks;
using Android.Graphics;
using Java.Lang;
using Square.Picasso;
using Firebase;
using Firebase.Storage;
using Android.Net;
using Android.Util;

namespace SMobile.Android.Helper
{

    class RecyclerViewAdapterProducts : RecyclerView.Adapter, IItemClickListener
    {
        private List<Models.Entities.ProductEntity> ListProducts = new List<Models.Entities.ProductEntity>();
        private List<Models.Entities.CompaniesEntity> ListCompany = new List<Models.Entities.CompaniesEntity>();

        private Context Context;
        private readonly ViewGroup root;
        private Service.SearchProducts sp = new Service.SearchProducts();

        //----
        // private StorageReference storage;

        //private RecyclerView.ViewHolder holder;
        // private Bitmap bitmap;
        // // private RecyclerViewHolder holder;
        //private ImageView ip;
        // View item;

        //----Valoración con estrellas----//--Contadores de estrellas----
        int cstar1 = 0; int cstar2 = 0; int cstar3 = 0; int cstar4 = 0; int cstar5 = 5;
        //----Cantidad de estrellas definidas----
        int cstar = 5;
        //---------//
        float ranking = 0.0f; int votes = 0;
        //----Nombre de compañia
        string companyname = "";

        public RecyclerViewAdapterProducts(List<ProductEntity> listProducts, Context context)
        {
            ListProducts = listProducts;
            this.Context = context;
        }

        public override int ItemCount => ListProducts.Count;

        public List<Tuple<string, RecyclerViewHolder, int, bool>> dictImages = new List<Tuple<string, RecyclerViewHolder, int, bool>>();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;

            //  viewHolder.imageProduct.SetImageResource();
            viewHolder.txtNameProduct.Text = ListProducts[position].Name;
            viewHolder.txtDescriptionProduct.Text = ListProducts[position].Features;
            viewHolder.txtPrecio.Text = viewHolder.txtPrecio.Text + ListProducts[position].Price;
            viewHolder.txtRanking.Text = viewHolder.txtRanking.Text + ListProducts[position].Ranking;
            //ImageView ip= holder.ItemView.FindViewById<ImageView>(Resource.Id.Id_ImageView);

            //FirebaseApp.InitializeApp(Context);

            //var i = dictImages.Count + 1;
            //dictImages.Add(new Tuple<string, RecyclerViewHolder, int, bool>(url, viewHolder, i, false));

            //DownloadImageProducts(url);

            //   viewHolder.imageProduct.SetImageBitmap(bitmap);


            //StorageReference storageReference = FirebaseStorage
            //   .Instance
            //   .GetReferenceFromUrl(Configuration.FirebaseConfig.FIREBASE_STORAGE_URL);

            //StorageReference spaceRef = storageReference.Child(ListProducts[position].UrlImage);
            if (ListProducts[position].UrlImage.Contains("http"))
            {
                Picasso.With(Context)
               .Load(ListProducts[position].UrlImage)
               .Config(Bitmap.Config.Rgb565)
               .Resize(304, 228)
               .Into(viewHolder.imageProduct);
            }
            else
            {
                viewHolder.imageProduct.SetImageResource(Resource.Drawable.ic_isotipo_sadara);
            }
            viewHolder.SetItemClickListener(this);

        }

        //---Método de la interfaz IItemClickListener
        public async void OnClickAsync(View dl, int position, bool isLongClick)
        {
            if (isLongClick)
            {
                LayoutInflater inflater = LayoutInflater.From(Context);

                //Cargar diálogo con detalles de producto
                dl = inflater.Inflate(Resource.Layout.DialogProduct, root);
                AlertDialog buider = new AlertDialog.Builder(Context).Create();
                buider.SetView(dl);

                buider.SetCanceledOnTouchOutside(true); //Cerrar el diálogo al tocar fuera de el

                //Elementos del diálogo
                TextView NameP = dl.FindViewById<TextView>(Resource.Id.NameProduct);
                TextView Featu = dl.FindViewById<TextView>(Resource.Id.Description);
                ImageView imageProduct = dl.FindViewById<ImageView>(Resource.Id.ImageProduct);
                TextView Price = dl.FindViewById<TextView>(Resource.Id.price);
                TextView Stock = dl.FindViewById<TextView>(Resource.Id.stock);
                TextView Votes = dl.FindViewById<TextView>(Resource.Id.votes);
                TextView Ranking = dl.FindViewById<TextView>(Resource.Id.ranking);
                TextView Company = dl.FindViewById<TextView>(Resource.Id.company);

                //RatingBar ratingbar = dl.FindViewById<RatingBar>(Resource.Id.ratingbar);
                //ratingbar.RatingBarChange += (o, e) => {
                //    float r = ratingbar.Rating;
                //    Toast.MakeText(Context, "Rating: "+r, ToastLength.Short).Show();


                //};

                //Estrellas para puntuación
                ImageView Image_Star1 = dl.FindViewById<ImageView>(Resource.Id.Image_star1);
                ImageView Image_Star2 = dl.FindViewById<ImageView>(Resource.Id.Image_star2);
                ImageView Image_Star3 = dl.FindViewById<ImageView>(Resource.Id.Image_star3);
                ImageView Image_Star4 = dl.FindViewById<ImageView>(Resource.Id.Image_star4);
                ImageView Image_Star5 = dl.FindViewById<ImageView>(Resource.Id.Image_star5);


                NameP.Text = ListProducts[position].Name;
                Featu.Text = ListProducts[position].Features;
                Price.Text = Price.Text + ListProducts[position].Price;
                Stock.Text = Stock.Text + ListProducts[position].InStock;
                Votes.Text = Votes.Text + ListProducts[position].Votes;
                Ranking.Text = Ranking.Text + ListProducts[position].Ranking;

                // imageProduct....
                //Picasso.With(Context)
                //    .Load(ListProducts[position].UrlImage)
                //    .Config(Bitmap.Config.Rgb565)
                //    .Resize(304, 228)
                //    .Into(imageProduct);
                if (ListProducts[position].UrlImage.Contains("http"))
                {
                    Picasso.With(Context)
                   .Load(ListProducts[position].UrlImage)
                   .Config(Bitmap.Config.Rgb565)
                   .Resize(304, 228)
                   .Into(imageProduct);
                }
                else
                {
                    imageProduct.SetImageResource(Resource.Drawable.ic_isotipo_sadara);
                }

                //Obteniendo nombre de la empresa.... esta forma no me gusta
                //ListCompany = await sp.GetNameCompany(ListProducts[position].uidCompany);
                //companyname = ListCompany[0].Name;
                companyname = await sp.GetNameCompany(ListProducts[position].uidCompany);

                Company.Text = Company.Text + companyname;

                Image_Star1.Click += async delegate
                {
                    cstar1 = ListProducts[position].Star1 + 1;
                    cstar2 = ListProducts[position].Star2;
                    cstar3 = ListProducts[position].Star3;
                    cstar4 = ListProducts[position].Star4;
                    cstar5 = ListProducts[position].Star5;

                    votes = cstar1 + cstar2 + cstar3 + cstar4 + cstar5; //total usuarios que han votado

                    ranking = ((cstar1 + (2 * cstar2) + (3 * cstar3) + (4 * cstar4) + (5 * cstar5)) * cstar) / (cstar * votes);
                    await sp.UpdateValoracion(ListProducts[position].uid, ListProducts[position].uidCompany, ranking, votes, cstar1, "Star1");
                    ListProducts[position].Star1 += 1;
                    ListProducts[position].Votes = votes;
                    ListProducts[position].Ranking = ranking;
                    Votes.Text = "Votos: " + ListProducts[position].Votes;
                    Ranking.Text = "Ranking: " + ListProducts[position].Ranking;



                };
                Image_Star2.Click += async delegate
                {
                    cstar1 = ListProducts[position].Star1;
                    cstar2 = ListProducts[position].Star2 + 1;
                    cstar3 = ListProducts[position].Star3;
                    cstar4 = ListProducts[position].Star4;
                    cstar5 = ListProducts[position].Star5;
                    valoracion();

                    await sp.UpdateValoracion(ListProducts[position].uid, ListProducts[position].uidCompany, ranking, votes, cstar2, "Star2");
                    ListProducts[position].Star2 += 1;
                    ListProducts[position].Votes = votes;
                    ListProducts[position].Ranking = ranking;

                    Votes.Text = "Votos: " + votes;
                    Ranking.Text = "Ranking: " + ranking;

                };
                Image_Star3.Click += async delegate
                {
                    cstar1 = ListProducts[position].Star1;
                    cstar2 = ListProducts[position].Star2;
                    cstar3 = ListProducts[position].Star3 + 1;
                    cstar4 = ListProducts[position].Star4;
                    cstar5 = ListProducts[position].Star5;
                    valoracion();

                    await sp.UpdateValoracion(ListProducts[position].uid, ListProducts[position].uidCompany, ranking, votes, cstar3, "Star3");
                    ListProducts[position].Star3 += 1;
                    ListProducts[position].Votes = votes;
                    ListProducts[position].Ranking = ranking;

                    Votes.Text = "Votos: " + votes;
                    Ranking.Text = "Ranking: " + ranking;

                };
                Image_Star4.Click += async delegate
                {
                    cstar1 = ListProducts[position].Star1;
                    cstar2 = ListProducts[position].Star2;
                    cstar3 = ListProducts[position].Star3;
                    cstar4 = ListProducts[position].Star4 + 1;
                    cstar5 = ListProducts[position].Star5;
                    valoracion();

                    await sp.UpdateValoracion(ListProducts[position].uid, ListProducts[position].uidCompany, ranking, votes, cstar4, "Star4");
                    ListProducts[position].Star4 += 1;
                    ListProducts[position].Votes = votes;
                    ListProducts[position].Ranking = ranking;

                    Votes.Text = "Votos: " + votes;
                    Ranking.Text = "Ranking: " + ranking;

                };
                Image_Star5.Click += async delegate
                {
                    cstar1 = ListProducts[position].Star1;
                    cstar2 = ListProducts[position].Star2;
                    cstar3 = ListProducts[position].Star3;
                    cstar4 = ListProducts[position].Star4;
                    cstar5 = ListProducts[position].Star5 + 1;
                    valoracion();

                    await sp.UpdateValoracion(ListProducts[position].uid, ListProducts[position].uidCompany, ranking, votes, cstar5, "Star5");
                    ListProducts[position].Star5 += 1;
                    ListProducts[position].Votes = votes;
                    ListProducts[position].Ranking = ranking;

                    Votes.Text = "Votos: " + votes;
                    Ranking.Text = "Ranking: " + ranking;

                };

                switch (ListProducts[position].Ranking)
                {
                    case double n when (n >= 1 && n < 2):
                        Image_Star1.SetImageResource(Resource.Drawable.ic_star);
                        break;
                    case double n when (n >= 2 && n < 3):
                        Image_Star1.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star2.SetImageResource(Resource.Drawable.ic_star);
                        break;
                    case double n when (n >= 3 && n < 4):
                        Image_Star1.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star2.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star3.SetImageResource(Resource.Drawable.ic_star);
                        break;
                    case double n when (n >= 4 && n < 5):
                        Image_Star1.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star2.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star3.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star4.SetImageResource(Resource.Drawable.ic_star);
                        break;

                    case 5:
                        Image_Star1.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star2.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star3.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star4.SetImageResource(Resource.Drawable.ic_star);
                        Image_Star5.SetImageResource(Resource.Drawable.ic_star);
                        break;

                }

                buider.Show(); //Muestra el diálogo

            }

        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.ProductsCardView, parent, false);
            return new RecyclerViewHolder(itemView);
        }

        public void valoracion()
        {
            votes = cstar1 + cstar2 + cstar3 + cstar4 + cstar5; //total usuarios que han votado

            ranking = ((cstar1 + (2 * cstar2) + (3 * cstar3) + (4 * cstar4) + (5 * cstar5)) * cstar) / (cstar * votes);

        }


        //public void OnFailure(Java.Lang.Exception e)
        //{
        //    Log.Warn("FirebaseStorage", "Download Failure", e);
        //}
        //public void OnSuccess(Java.Lang.Object result)
        //{

        //    string downloadURL = result.ToString();
        //}

        //public void OnSuccess(Java.Lang.Object Result)
        //{
        //    // RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
        //    var data = Result.ToArray<byte>();

        //    bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);

        //    Tuple<string, RecyclerViewHolder, int, bool> actual = this.dictImages.Where(c=> !c.Item4).OrderByDescending(c=> c.Item3).FirstOrDefault();

        //    if (actual != null)
        //    {

        //        ImageView ip = actual.Item2.ItemView.FindViewById<ImageView>(Resource.Id.Id_ImageView);

        //        ip.SetImageBitmap(bitmap);

        //    }
        //}


        //private void DownloadImageProducts(string ImageUrl)
        //{


        //    StorageReference storageReference = FirebaseStorage
        //       .Instance
        //       .GetReferenceFromUrl(ImageUrl);


        //    storageReference.GetBytes(Configuration.FirebaseConfig.ONE_MEGABYTE)
        //     .AddOnSuccessListener(this)
        //     .AddOnFailureListener(this);


        //    }

    }

}

