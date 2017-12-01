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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using SMobile.Android.Models.LuisModel;

namespace SMobile.Android.Service
{
   public class ServiceLuis
    {
        public async Task<LuisModel> GetIntent(string query)
        {
            Models.LuisModel.LuisModel model = null;
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(Configuration.FirebaseConfig.LUIS_URL + query);
                model = JsonConvert.DeserializeObject<LuisModel>(json);
            }
            return model;

        }

    }
}
