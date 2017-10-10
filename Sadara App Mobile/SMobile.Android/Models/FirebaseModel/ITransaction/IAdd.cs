namespace SMobile.Android.Models.FirebaseModel.ITransaction
{
    interface IAdd<T>
    {

        System.Threading.Tasks.Task<Firebase.Xamarin.Database.FirebaseObject<T>> Add(T Entity);

    }
}