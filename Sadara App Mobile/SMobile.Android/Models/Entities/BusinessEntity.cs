using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.Entities
{

    public class BusinessEntity
    {

        public const string BUSINESS_NAME = "Companies";

        public override string ToString()
        {

            return BusinessEntity.BUSINESS_NAME;

        }

        public string uid { get; set; }

        public string name { get; set; }

        public string Bhours { get; set; }

        public string phone { get; set; }

        public string addres { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }

        public string RUC { get; set; }

        public string imageUrl { get; set; }

    }

}