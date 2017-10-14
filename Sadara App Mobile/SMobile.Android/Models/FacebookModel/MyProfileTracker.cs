using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Facebook;

namespace SMobile.Android.Models.FacebookModel
{
    public class MyProfileTracker : ProfileTracker
    {

        public event EventHandler<OnProfileChangedEventArgs> OnProfileChanged;

        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile currentProfile)
        {

            if (this.OnProfileChanged != null)
            {

                OnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(currentProfile));

            }

        }

    }
}