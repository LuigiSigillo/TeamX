using System;
using System.Text.RegularExpressions;

namespace TeamX.Utils
{
    public static class Validator
    {
        public static bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be less than 8 or greater than 15 characters";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ValidateEmail(string email)
        {
                const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(email);
        }

        public static bool ValidateName(string name)
        {
            var hasMiniMaxChars = new Regex(@".{2,15}");
            return hasMiniMaxChars.IsMatch(name);
        }
    }
}
