using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Realms;

namespace SMobile.Android.Models.RealmModel
{
    class ProductEntity : RealmObject
    {

        [PrimaryKey]
        public string ProductId { get; set; }

        public string ProductName { get; set; }

    }
}