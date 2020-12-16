using System;
using System.Linq;
using System.Text;

namespace TeamX.Utils
{
    public static class Generator
    {
        public static string GenerateID()
        {
            var guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string GeneratePWD()
        {
            int length = 10;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string GenerateIniziali(string username)
        {
            string iniziali;
            var names = username.Split(' ');
            string firstname = names[0];
            string lastname;
            if (names.Count() > 1)
            {
                lastname = names[1];
                iniziali = (firstname.Substring(0, 1) + lastname.Substring(0, 1)).ToUpper();
            }
            else iniziali = firstname.Substring(0, 1).ToUpper();
            return iniziali;
        }
    }
}
