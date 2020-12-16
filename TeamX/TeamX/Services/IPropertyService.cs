using System;
using TeamX.Models;

namespace TeamX.Services
{
    public interface IPropertyService
    {

        //This method will return the current active (logged in) user
        User GetUser();


        //This method will set the current active user
        //--Needed at login or register
        void SetUser(User user);


        //This methos will disable the current active user
        //--Needed at logout
        void DisableUser(); 


    }
}
