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
using SMobile.Android.Models.Entities;

namespace SMobile.Android.Helper
{
    class RecyclerViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {

        public ImageView imageProduct { get; set; }
        public TextView txtNameProduct { get; set; }
        public TextView txtDescriptionProduct { get; set; }
        public TextView txtRanking { get; set; }
        public TextView txtPrecio { get; set; }

        private IItemClickListener itemClickListener; //---//
        public RecyclerViewHolder(View itemView):base(itemView)
        {
            imageProduct = itemView.FindViewById<ImageView>(Resource.Id.Id_ImageView);
            txtNameProduct = itemView.FindViewById<TextView>(Resource.Id.NameProduct);
            txtDescriptionProduct = itemView.FindViewById<TextView>(Resource.Id.Description);
            txtPrecio = ItemView.FindViewById<TextView>(Resource.Id.txtprice);
            txtRanking = ItemView.FindViewById<TextView>(Resource.Id.txtranking);

            ItemView.SetOnClickListener(this);
            ItemView.SetOnLongClickListener(this);
        }
        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }

        //Método de interfaz IOncliclistener
        public void OnClick(View v)
        {
            itemClickListener.OnClickAsync(v, AdapterPosition, true);
            
        }
        //Método de interfaz IOnlongcliclistener
        public bool OnLongClick(View v)
        {
            itemClickListener.OnClickAsync(v, AdapterPosition, true);
            return true;
        }


    }
}