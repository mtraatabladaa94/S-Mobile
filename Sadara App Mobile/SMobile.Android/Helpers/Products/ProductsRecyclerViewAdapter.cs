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
using ImageViews.Rounded;
using Square.Picasso;

namespace SMobile.Android.Helpers.Products
{

    public class ProductsRecyclerViewHolder : RecyclerView.ViewHolder
    {

        #region Components for item view

        public CardView cardView { get; set; }

        public ImageView profileImageView { get; set; }

        public TextView usernameTextView { get; set; }

        public TextView userdescriptionTextView { get; set; }

        public ImageView coverImageView { get; set; }

        public TextView priceTextView { get; set; }

        public TextView endDateTextView { get; set; }

        public TextView titleoffersTextView { get; set; }

        public TextView descriptionoffersTextView { get; set; }

        #endregion

        public void InitialComponents(View view)
        {
            
            this.profileImageView = view.FindViewById<ImageView>(Resource.Id.profileImageView);

            this.usernameTextView = view.FindViewById<TextView>(Resource.Id.usernameTextView);

            this.userdescriptionTextView = view.FindViewById<TextView>(Resource.Id.userdescriptionTextView);

            this.coverImageView = view.FindViewById<ImageView>(Resource.Id.coverImageView);

            this.priceTextView = view.FindViewById<TextView>(Resource.Id.priceTextView);

            this.titleoffersTextView = view.FindViewById<TextView>(Resource.Id.titleoffersTextView);

            this.descriptionoffersTextView = view.FindViewById<TextView>(Resource.Id.descriptionoffersTextView);
            
        }

        public ProductsRecyclerViewHolder(View itemView, Context context)
            : base(itemView)
        {

            this.InitialComponents(itemView);

        }

    }

    public class ProductsRecyclerViewAdapter : RecyclerView.Adapter
    {

        #region Components
        List<Models.Entities.ProductsWithBusinessEntity> productsList;
        public Context context { get; set; }
        #endregion

        public ProductsRecyclerViewAdapter(List<Models.Entities.ProductsWithBusinessEntity> productsList, Context context)
        {

            this.productsList = productsList;

            this.context = context;

        }

        #region Overrides functions and properties

        public override int ItemCount => this.productsList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            this.SetData(holder as ProductsRecyclerViewHolder, position);

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

            View itemView = layoutInflater.Inflate(Resource.Layout.products_item_layout, parent, false);

            return new ProductsRecyclerViewHolder(itemView, context);

        }

        #endregion

        #region Private functions

        public void SetData(ProductsRecyclerViewHolder holder, int position)
        {

            holder.usernameTextView.Text = this.productsList[position].nameBusiness;

            holder.userdescriptionTextView.Text = $"(+505) {this.productsList[position].phone}";

            holder.titleoffersTextView.Text = this.productsList[position].nameProduct;

            holder.descriptionoffersTextView.Text = this.productsList[position].Features;

            holder.priceTextView.Text = this.productsList[position].Price.ToString("#,##0.0");
            
            if (!string.IsNullOrWhiteSpace(this.productsList[position].businessImageUrl))
            {

                var transformation = new RoundedTransformationBuilder()
                .CornerRadiusDp(32)
                .Oval(true)
                .Build();

                Picasso
                .With(this.context)
                .Load(this.productsList[position].businessImageUrl)
                .Resize(64, 64)
                .CenterCrop()
                .Transform(transformation)
                .Error(Resource.Drawable.ic_business)
                .Into(holder.profileImageView);

            }

            if (!string.IsNullOrWhiteSpace(this.productsList[position].productImageUrl))
            {

                Picasso
                .With(this.context)
                .Load(this.productsList[position].productImageUrl)
                .Into(holder.coverImageView);

            }

        }

        #endregion

    }

}