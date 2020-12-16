using System;
using System.Collections.Generic;
using TeamX.Models;

namespace TeamX.Services
{
    public interface IUserContactService
    {

        bool AddContact(string userId, string contactUri, string type);

        bool DeleteContact(string userId, string type);

        void DeleteAllContacts(string userId);

        /// <summary>
        /// Modifies the contact if the contact already exists, else inserts the contact
        /// </summary>
        /// <returns><c>true</c>, if contact was modifyed, <c>false</c> if contact was added.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="type">Type.</param>
        /// <param name="newUri">New URI.</param>
        bool ModifyContact(string userId, string type, string newUri);

        UserContact GetContact(string userId, string type);

        List<UserContact> GetContacts(string userId);



    }
}
