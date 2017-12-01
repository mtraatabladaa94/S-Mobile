using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.Entities
{

    public class OfferEntity
    {

        public const string OFFER_NAME = "Offers";

        public override string ToString()
        {

            return OfferEntity.OFFER_NAME;

        }

        public string uid { get; set; } // Key del nodo

        public DateTime sendDate { get; set; }

        public string imageUrl { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime initialDate { get; set; }

        public DateTime endDate { get; set; }

        public string businessUid { get; set; }

    }

}