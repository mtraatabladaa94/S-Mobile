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
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

using System.Threading.Tasks;


namespace SMobile.Android.Service
{
    class SearchProducts
    {
       
        FirebaseClient firebaseClient = new FirebaseClient(Configuration.FirebaseConfig.FIREBASE_URL);
        
        public async Task<List<Models.Entities.CompaniesEntity>> Search_Company()
        {

            var company = await this.firebaseClient.Child("Companies").OnceAsync<Models.Entities.CompaniesEntity>();
            List<Models.Entities.CompaniesEntity> companylist = new List<Models.Entities.CompaniesEntity>();
            foreach (var c in company)
            {
                companylist.Add(
                    new Models.Entities.CompaniesEntity()
                    {
                        uid = c.Key,
                        Name = c.Object.Name,
                       
                    }
                    );
            }
            return companylist;

        }
        public async Task<List<Models.Entities.CompaniesEntity>> Search_Company_Name(string name)
        {

            var company = await this.firebaseClient.Child("Companies").OnceAsync<Models.Entities.CompaniesEntity>();
            List<Models.Entities.CompaniesEntity> companylist = new List<Models.Entities.CompaniesEntity>();
            foreach (var c in company)
            {
                if ((c.Object.Name.ToUpper()).Contains(name.ToUpper()))
                {
                    companylist.Add(
                        new Models.Entities.CompaniesEntity()
                        {
                            uid = c.Key,
                            Name = c.Object.Name,
                            Addres=c.Object.Addres,
                            condition=c.Object.condition,
                            Bhours=c.Object.Bhours,
                            Cellcontact=c.Object.Cellcontact,
                            Emailcontact=c.Object.Emailcontact,
                            imageUrl=c.Object.imageUrl,
                            Namecontact=c.Object.Namecontact,
                            Phone=c.Object.Phone,
                            RUC=c.Object.RUC
                        }
                        );
                }
            }
            return companylist;

        }

        //Obtener nombre de empresa por codigo
        public async Task<string> GetNameCompany(string uidcompany)
        {
            string namec = "";
            var cp = await this.firebaseClient.Child("Companies").OnceAsync<Models.Entities.CompaniesEntity>();
            foreach (var c in cp)
            {
                if (c.Key.Equals(uidcompany))
                {
                    namec = c.Object.Name;
                }
            }
            return namec;  
        }

        public async Task<List<Models.Entities.ProductEntity>> ListProducts()
        {
            Models.Entities.ProductEntity product = new Models.Entities.ProductEntity();
            var pr = await this.firebaseClient.Child("Products").OnceAsync<Models.Entities.ProductEntity>();
            var cp = await this.firebaseClient.Child("Companies").OnceAsync<Models.Entities.CompaniesEntity>();
            string keycompany = "";
            List<Models.Entities.ProductEntity> ProductsList = new List<Models.Entities.ProductEntity>();

            foreach (var c in cp)
            {
                keycompany = c.Key;

                var productsss = await this.firebaseClient.Child("Products/" + keycompany).OnceAsync<Models.Entities.ProductEntity>();
                foreach (var p1 in productsss)
                {
                    
                        ProductsList.Add(
                            new Models.Entities.ProductEntity()
                            {
                                uidCompany=keycompany,
                                uid =p1.Key,
                                Name = p1.Object.Name,
                                Features = p1.Object.Features,
                                InStock= p1.Object.InStock,
                                Price= p1.Object.Price,
                                Ranking= p1.Object.Ranking,
                                Votes= p1.Object.Votes,
                                Star1= p1.Object.Star1,
                                Star2 = p1.Object.Star2,
                                Star3 = p1.Object.Star3,
                                Star4 = p1.Object.Star4,
                                Star5 = p1.Object.Star5,
                                UrlImage =p1.Object.UrlImage

                            }
                            );
            
                }
            }
            return ProductsList;
        }

        public async Task<List<Models.Entities.ProductEntity>> Search_Product(string Tag)
        {
            Models.Entities.ProductEntity prod = new Models.Entities.ProductEntity();
            var pr = await this.firebaseClient.Child("Products").OnceAsync<Models.Entities.ProductEntity>();
            var cp = await this.firebaseClient.Child("Companies").OnceAsync<Models.Entities.CompaniesEntity>();
            string keycompany = "";

            List<Models.Entities.ProductEntity> ProductsList = new List<Models.Entities.ProductEntity>();

            foreach (var c in cp)
            {
                keycompany = c.Key;

                var productsss = await this.firebaseClient.Child("Products/"+keycompany).OnceAsync<Models.Entities.ProductEntity>();
                // FilterQuery f= this.firebaseClient.Child("Products/" + keycompany).OrderBy("Name").StartAt("ta").EndAt("t\uf8ff");
                //FirebaseQuery fq = this.firebaseClient.Child("Products/" + keycompany).OrderBy("Name").StartAt("ta").EndAt("g\uf8ff");
                // var productsss = await this.firebaseClient.Child("Products/" + keycompany).OrderBy("InStock").StartAt("T").OnceAsync<Producto>();

                //var productsss =
                //    await this.firebaseClient.Child("Products/" + keycompany)
                //    .OrderByKey()
                //    .LimitToFirst(2)
                //    .StartAt("T")
                //    .EndAt("t")
                //    .OnceAsync<Models.Entities.ProductEntity>();


                foreach (var p1 in productsss)
                {
                      if ((p1.Object.Name.ToUpper()).Contains(Tag.ToUpper())){ 

                    ProductsList.Add(
                        new Models.Entities.ProductEntity()
                        {
                            uid = p1.Key,
                            Name = p1.Object.Name,
                            Features = p1.Object.Features
                        }
                        );
                      }//Cierre IF
                }
            }
            return ProductsList;
         }

        public async Task<int> UpdateValoracion(string uid, string uidcompany, double ranking, int votes, int star, string numstar)
        {
            await firebaseClient.Child("Products/"+uidcompany).Child(uid).Child("Votes").PutAsync(votes);
            await firebaseClient.Child("Products/"+uidcompany).Child(uid).Child("Ranking").PutAsync(ranking);
            

            switch (numstar)
            {
                case "Star1":
                    await firebaseClient.Child("Products/" + uidcompany).Child(uid).Child("Star1").PutAsync(star);
                    break;
                case "Star2":
                    await firebaseClient.Child("Products/" + uidcompany).Child(uid).Child("Star2").PutAsync(star);
                    break;
                case "Star3":
                    await firebaseClient.Child("Products/" + uidcompany).Child(uid).Child("Star3").PutAsync(star);
                    break;
                case "Star4":
                    await firebaseClient.Child("Products/" + uidcompany).Child(uid).Child("Star4").PutAsync(star);
                    break;
                case "Star5":
                    await firebaseClient.Child("Products/" + uidcompany).Child(uid).Child("Star5").PutAsync(star);
                    break;
            }
            return 0;

        }

    }

}