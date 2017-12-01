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
    class CompaniesEntity
    {
        public string uid { get; set; }
        public string Addres { get; set; }
        public string Name { get; set; }
        public string Bhours { get; set; }
        public string Phone{get; set;}
        public string RUC { get; set; }
        public string condition { get; set; }
        public string imageUrl { get; set; }
        public string Cellcontact { get; set; }
        public string Emailcontact { get; set; }
        public string Namecontact { get; set; }

    }
}