using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

using Square.Picasso;
using Android.Content;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Net;
using Android.Support.V7.App;
using ImageViews.Rounded;

namespace SMobile.Android.Helpers.Business
{

    public class BusinessRecyclerViewHolder : RecyclerView.ViewHolder
    {

        #region Components for item view

        public CardView businessCardView { get; set; }

        public ImageView profileImageView { get; set; }

        public TextView nameTextView { get; set; }

        public TextView attentionTextView { get; set; }

        public TextView captionTextView { get; set; }

        public FloatingActionButton callFab { get; set; }

        #endregion

        private void InitialComponents(View itemView)
        {

            this.businessCardView = itemView.FindViewById<CardView>(Resource.Id.business_item_cardview);

            this.profileImageView = this.businessCardView.FindViewById<ImageView>(Resource.Id.profileImageView);

            this.nameTextView = this.businessCardView.FindViewById<TextView>(Resource.Id.nameTextView);

            this.attentionTextView = this.businessCardView.FindViewById<TextView>(Resource.Id.attentionTextView);

            this.captionTextView = this.businessCardView.FindViewById<TextView>(Resource.Id.captionTextView);

            this.callFab = this.businessCardView.FindViewById<FloatingActionButton>(Resource.Id.callFab);

        }

        public BusinessRecyclerViewHolder(View itemView)
            : base(itemView)
        {

            this.InitialComponents(itemView);
            
        }

    }

    class BusinessRecyclerViewAdapter : RecyclerView.Adapter
    {

        #region Components for view
        List<Models.Entities.BusinessEntity> businessList;
        Context context;
        #endregion

        public BusinessRecyclerViewAdapter(List<Models.Entities.BusinessEntity> businessList, Context context)
        {

            this.businessList = businessList;

            this.context = context;

        }

        #region Override functions

        public override int ItemCount => this.businessList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            this.SetData(holder as BusinessRecyclerViewHolder, position);

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

            View itemView = layoutInflater.Inflate(Resource.Layout.business_item_layout, parent, false);

            return new BusinessRecyclerViewHolder(itemView);

        }

        #endregion
        
        #region Private functions

        private void SetData(BusinessRecyclerViewHolder holder, int position)
        {

            if (!string.IsNullOrWhiteSpace(this.businessList[position].imageUrl))
            {

                var transformation = new RoundedTransformationBuilder()
                .CornerRadiusDp(96)
                .Oval(true)
                .Build();

                Picasso
                .With(this.context)
                .Load(this.businessList[position].imageUrl)
                .Resize(96, 96)
                .CenterCrop()
                .Transform(transformation)
                .Error(Resource.Drawable.ic_business)
                .Into(holder.profileImageView);
                
            }

            holder.nameTextView.Text = this.businessList[position].name;

            holder.attentionTextView.Text = this.businessList[position].Bhours;

            holder.captionTextView.Text
                = $"Celular • (+505 {this.businessList[position].phone})";

            holder.callFab.Click += (sender, e) => { this.CallBusiness(this.businessList[position].phone); };

        }

        private void CallBusiness(string phone)
        {

            Intent phoneCallIntent = new Intent(Intent.ActionCall);

            phoneCallIntent.SetData(Uri.Parse($"tel:{phone}"));

            context.StartActivity(phoneCallIntent);

        }

        #endregion

        #region Events for components

        #endregion

    }

}