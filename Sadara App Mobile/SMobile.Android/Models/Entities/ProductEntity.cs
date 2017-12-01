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

    public class ProductEntity
    {

        public const string PRODUCT_NAME = "Products";

        public override string ToString()
        {

            return ProductEntity.PRODUCT_NAME;

        }

        public string uid { get; set; }
        public string uidCompany { get; set; }

        public string Features { get; set; }

        public int InStock { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Ranking { get; set; }

        public int Star1 { get; set; }

        public int Star2 { get; set; }

        public int Star3 { get; set; }

        public int Star4 { get; set; }

        public int Star5 { get; set; }

        public string UrlImage { get; set; }

        public int Votes { get; set; }

    }

}