using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.FirebaseModel.Transaction
{
    interface IRemove<T>
    {

        void Remove<Id>(T Entity, Id Uid);

    }
}