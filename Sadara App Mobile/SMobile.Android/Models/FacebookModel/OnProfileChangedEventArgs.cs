using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Facebook;

namespace SMobile.Android.Models.FacebookModel
{
    public class OnProfileChangedEventArgs : EventArgs
    {

        public Profile profile;

        public OnProfileChangedEventArgs(Profile profile) => this.profile = profile;

    }
}