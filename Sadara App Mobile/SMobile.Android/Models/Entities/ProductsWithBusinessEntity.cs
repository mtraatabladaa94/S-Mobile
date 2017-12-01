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

namespace SMobile.Android.Models.Entities
{

    public class ProductsWithBusinessEntity
    {

        #region Data Business

        public string businessUid { get; set; }

        public string nameBusiness { get; set; }

        public string phone { get; set; }

        public string addres { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }

        public string RUC { get; set; }

        public string businessImageUrl { get; set; }

        #endregion

        #region Data Products

        public string productUid { get; set; }

        public string Features { get; set; }

        public int InStock { get; set; }

        public string nameProduct { get; set; }

        public decimal Price { get; set; }

        public double Ranking { get; set; }

        public int Star1 { get; set; }

        public int Star2 { get; set; }

        public int Star3 { get; set; }

        public int Star4 { get; set; }

        public int Star5 { get; set; }

        public string productImageUrl { get; set; }

        public int Votes { get; set; }

        #endregion

    }

}