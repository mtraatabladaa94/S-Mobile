namespace SMobile.Android.Models.FirebaseModel.ITransaction
{
    interface IRemove<T>
    {

        System.Threading.Tasks.Task Remove<Id>(T Entity, Id Uid);

    }
}