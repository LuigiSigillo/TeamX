using System.Collections.Generic;
using TeamX.Models;

namespace TeamX.Services
{
    public interface IUserService
    {
        //Get all users in the database 
        IEnumerable<User> GetUsers();

        //Gets the user with the ID == id
        //Returns NULL if the user does not exist
        User GetUser(string id);

        /// <summary>
        /// Finds the user by Nickname
        /// </summary>
        /// <returns>
        /// The id of the user with the specified id if found, else -1
        /// </returns>
        /// <param name="nickname">Nickname.</param>
        string FindUserByName(string nickname);

        /// <summary>
        /// Finds the user by email
        /// </summary>
        /// <returns>
        /// The id of the user with the specified email if found, else -1.
        /// </returns>
        /// <param name="email">Email.</param>
        string FindUserByEmail(string email);

        //Insert a new user in the database
        //Returns ture if the operation has concluded without errors
        bool InsertUser(User u);

        //Updates user information
        //Returns true if the user has been found and correctly updated
        bool UpdateUser(User u);

        /// <summary>
        /// Deletes the user with the specified id
        /// </summary>
        /// <returns><c>true</c>, if user was deleted, <c>false</c> otherwise.</returns>
        /// <param name="id">Identifier.</param>
        bool DeleteUser(string id);
    }
}