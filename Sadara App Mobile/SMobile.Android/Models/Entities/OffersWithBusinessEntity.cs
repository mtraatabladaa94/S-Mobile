using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.Entities
{

    public class OffersWithBusinessEntity
    {

        #region Data Offers

        public string offerUid { get; set; } // Key del nodo

        public DateTime sendDate { get; set; }

        public string offerImageUrl { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime initialDate { get; set; }

        public DateTime endDate { get; set; }

        #endregion

        #region Data Business

        public string businessUid { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string addres { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }

        public string RUC { get; set; }

        public string businessImageUrl { get; set; }

        #endregion

    }

}