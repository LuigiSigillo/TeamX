using System;
using TeamX.Utils;

namespace TeamX.Models
{
    public class UserContact
    {

        public string UserID { get; private set; }
        public string ContactUri { get; set; }
        public string Type { get; set; }



        public UserContact(string userId, string contactUri, string type )
        {

            UserID = userId;
            ContactUri = contactUri;
            Type = type;
        }
    }
}
