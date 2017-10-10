using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMobile.Android.Models.Entities
{

    public class UserTokenEntity
    {

        public const string USER_TOKEN_NAME = "usersTokens";

        public override string ToString()
        {

            return UserTokenEntity.USER_TOKEN_NAME;

        }

        public string uid { get; set; }

        public string token { get; set; }

    }

}