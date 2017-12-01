using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ImageViews.Rounded;
using Square.Picasso;
using System.Collections.Generic;

namespace SMobile.Android.Helpers.Offers
{
    public class OffersRecyclerViewHolder : RecyclerView.ViewHolder
    {

        #region Components for item view

        public CardView cardView;
        public ImageView profileImageView { get; set; }
        public TextView usernameTextView { get; set; }
        public TextView userdescriptionTextView { get; set; }
        public ImageView coverImageView { get; set; }
        public TextView startDateTextView { get; set; }
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

            this.startDateTextView = view.FindViewById<TextView>(Resource.Id.startDateTextView);

            this.endDateTextView = view.FindViewById<TextView>(Resource.Id.endDateTextView);

            this.titleoffersTextView = view.FindViewById<TextView>(Resource.Id.titleoffersTextView);

            this.descriptionoffersTextView = view.FindViewById<TextView>(Resource.Id.descriptionoffersTextView);

        }

        public OffersRecyclerViewHolder(View itemView, Context context)
            : base(itemView)
        {

            this.InitialComponents(itemView);

        }

    }

    public class OffersRecyclerViewAdapter : RecyclerView.Adapter
    {

        #region Components
        List<Models.Entities.OffersWithBusinessEntity> offersList;
        public Context context { get; set; }
        #endregion

        public OffersRecyclerViewAdapter(List<Models.Entities.OffersWithBusinessEntity> OfferList, Context context)
        {

            this.offersList = OfferList;

            this.context = context;

        }

        #region Override functions and properties

        public override int ItemCount => this.offersList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            this.SetData(holder as OffersRecyclerViewHolder, position);

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

            View itemView = layoutInflater.Inflate(Resource.Layout.offers_item_layout, parent, false);

            return new OffersRecyclerViewHolder(itemView, context);

        }

        #endregion

        public void SetData(OffersRecyclerViewHolder holder, int position)
        {

            holder.usernameTextView.Text = this.offersList[position].name;

            holder.userdescriptionTextView.Text = $"(+505) {this.offersList[position].phone}";

            holder.titleoffersTextView.Text = this.offersList[position].title;

            holder.descriptionoffersTextView.Text = this.offersList[position].description;

            holder.startDateTextView.Text = this.offersList[position].initialDate.ToString("dd/MM/yyyy");

            holder.endDateTextView.Text = this.offersList[position].endDate.ToString("dd/MM/yyyy");

            if (!string.IsNullOrWhiteSpace(this.offersList[position].businessImageUrl))
            {

                var transformation = new RoundedTransformationBuilder()
                .CornerRadiusDp(64)
                .Oval(true)
                .Build();

                Picasso
                .With(this.context)
                .Load(this.offersList[position].businessImageUrl)
                .Resize(64, 64)
                .CenterCrop()
                .Transform(transformation)
                .Error(Resource.Drawable.ic_business)
                .Into(holder.profileImageView);
                
            }

            if (!string.IsNullOrWhiteSpace( this.offersList[position].offerImageUrl))
            {

                Picasso
                .With(this.context)
                .Load(this.offersList[position].offerImageUrl)
                //.Error(Resource.Drawable.img_slider1)
                .Into(holder.coverImageView);

            }

        }
        
    }

}