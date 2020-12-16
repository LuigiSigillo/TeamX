using System;
using TeamX.Utils;

namespace TeamX.Models
{
   public class User
    {


        public User(string email, string pwd, string nickname){

            Id = Generator.GenerateID();
            Email = email;
            Pwd = pwd;
            NickName = nickname;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string NickName { get; set; }



    }
}
