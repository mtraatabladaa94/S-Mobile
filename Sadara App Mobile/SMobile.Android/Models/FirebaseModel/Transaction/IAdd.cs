using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.FirebaseModel.Transaction
{
    interface IAdd<T>
    {

        void Add(T Entity);

    }
}