using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.FirebaseModel.Transaction
{
    interface IUpdate<T>
    {

        void Update(T Entity, List<Tuple<string, string, object>> DataToChanges);

    }
}