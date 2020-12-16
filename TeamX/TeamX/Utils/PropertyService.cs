

using System;
using Newtonsoft.Json;
using TeamX.Models;
using TeamX.Services;
using Xamarin.Forms;

namespace TeamX.Utils
{
    public class PropertyService : IPropertyService
    {
        //read the notes in the interface

        public void DisableUser()
        {
           Application.Current.Properties.Remove("User");
        }

        public User GetUser()
        {

            //Deserialization: string -> Object
            string jsonUser = Application.Current.Properties["User"].ToString();
            return JsonConvert.DeserializeObject<User>(jsonUser);
        }

        public async void SetUser(User user)
        {
            //Serialization: Object -> string
            string jsonUser = JsonConvert.SerializeObject(user);
            Application.Current.Properties["User"] = jsonUser;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
