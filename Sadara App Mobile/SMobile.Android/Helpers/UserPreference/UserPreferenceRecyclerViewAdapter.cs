using Android.Support.V7.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SMobile.Android.Helpers
{

    public class UserPreferenceRecyclerViewHolder : RecyclerView.ViewHolder
    {

        public TextView userPreferenceId { get; set; }

        public TextView userPreferenceName { get; set; }

        public CheckBox userPreferenceSelected { get; set; }


        public UserPreferenceRecyclerViewHolder(View itemView) : base(itemView)
        {

            this.userPreferenceId = itemView.FindViewById<TextView>(Resource.Id.userPreferenceIdTextView);

            this.userPreferenceName = itemView.FindViewById<TextView>(Resource.Id.userPreferenceNameTextView);

            this.userPreferenceSelected = itemView.FindViewById<CheckBox>(Resource.Id.userPreferenceSelected);

        }
    }
    public class UserPreferenceRecyclerViewAdapter : RecyclerView.Adapter
    {

        List<Models.Entities.PreferenceEntity> PreferenceList = new List<Models.Entities.PreferenceEntity>();

        public UserPreferenceRecyclerViewAdapter(List<Models.Entities.PreferenceEntity> PreferenceList)
        {
            this.PreferenceList = PreferenceList;
        }

        public override int ItemCount => this.PreferenceList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            var userPreferenceHolder = holder as UserPreferenceRecyclerViewHolder;

            userPreferenceHolder.userPreferenceId.Text = this.PreferenceList[position].uid;

            userPreferenceHolder.userPreferenceName.Text = this.PreferenceList[position].name;
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

            View itemView = layoutInflater.Inflate(Resource.Layout.UserPreferenceItem, parent, false);

            return new UserPreferenceRecyclerViewHolder(itemView);
        }
    }
}