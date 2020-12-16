using System;
using System.Collections.Generic;

namespace TeamX.Utils
{
    public static class ContactService
    {
        private static List<string> ContactTypes = new List<string>
        {
            "email",
            "facebook",
            "twitter",
            "linkedIn"
        };

        public static List<string> GetContactTypes()
        {
            return ContactTypes;
        }
    }
}
