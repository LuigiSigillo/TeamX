using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class ProfileDetailPage : ContentPage
    {
        private User User;
        private IUserContactService UCS = new UserContactService();
        private IUserService US = new UserService();
        private bool ContactsChanged { get; set; }

        
        private string oldName;
        private string oldPassword;

        public List<UserContact> UserContacts;

        public ProfileDetailPage(User user)
        {
            InitializeComponent();
            User = user;
            oldName = user.NickName;
            oldPassword = user.Pwd;
            NameEntry.Text = User.NickName;
            PasswordEntry.Text = User.Pwd;
            UserContacts = UCS.GetContacts(user.Id);

            PrepareContactsTable();



        }

        void PrepareContactsTable()
        {
            var cntcsTypes = ContactService.GetContactTypes();


            foreach (string cntcType in cntcsTypes)
            {
                var cntc = UCS.GetContact(User.Id, cntcType);
                var text = "";

                if (cntc != null) 
                    text = cntc.ContactUri;

                if (cntcType.Equals("email"))
                    text = User.Email;

                var entry = new EntryCell
                {
                    
                    Label = cntcType,
                    Text = text,
                    Placeholder = "Your " + cntcType + " here",
                    LabelColor = Color.DarkSlateGray
                    
                    

                };



                entry.Completed += Entry_Completed;
                ContactsTS.Add(entry);

            }
        }

        void Entry_Completed(object sender, EventArgs e)
        {
            var entry = (EntryCell)sender;

            if (string.Equals(entry.Label, "email"))
            {
                EmailCompleted(entry.Text);
                return;
            }

            if (string.IsNullOrEmpty(entry.Text))
            {
                DeleteContact(entry.Label);
                return;

            }

            if (ValidateUrl(entry.Text))
            {
                UCS.ModifyContact(User.Id, entry.Label, entry.Text);
                ContactsChanged = true;
                return;
            }
            entry.Text = null;
            ContactsChanged = false;
            HandleWrongUrl();
        }

        async void EmailCompleted(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Please complete the email field", "OK");
                return;

            }
            if (Validator.ValidateEmail(email))
            {
                var newUser = User;
                newUser.Email = email;
                US.UpdateUser(newUser);
                UCS.ModifyContact(User.Id,"email", email);
                ContactsChanged = true;
                return;
            }
            
            HandleWrongEmail();
                
            
        }


        async void HandleWrongEmail()
        {
            await DisplayAlert("Error", "Please insert a valid email", "OK");
            return;
        }

        bool ValidateUrl(string url)
        {
            Uri resultURI;

            if (Uri.TryCreate(url, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }

        async void HandleWrongUrl()
        {
            await DisplayAlert("Error", "Please insert a valid url", "OK");
            return;
        }

        async void DeleteContact(string type)
        {
            if (await DisplayAlert("Warning", "Do you want to delete this contact?", "YES", "NO"))
            {
                UCS.DeleteContact(User.Id, type);
                ContactsChanged = true;
            }
            else return;


        }

        async void OnSave(object sender, System.EventArgs e)
        {
            if (!Validator.ValidateName(NameEntry.Text))
            {
                await DisplayAlert("Error", "Name should be at least 2 characters long", "OK");
                return;
            }

            string error_message;
            if (!Validator.ValidatePassword(PasswordEntry.Text,out error_message))
            {
                await DisplayAlert("Error",error_message, "OK");
                return;
            }

            if (!NameEntry.Text.Equals(oldName) || ! PasswordEntry.Text.Equals(oldPassword))
            {
                SaveNameAndPassword();
            }


            if (ContactsChanged)
            {
                MessagingCenter.Send(this, "ContactsChanged");
                await DisplayAlert("Profile Updated", User.NickName, "OK");
            }

            await Navigation.PopAsync();
        }

        private void SaveNameAndPassword()
        {

            var usr = new User(User.Email, PasswordEntry.Text, NameEntry.Text);
            usr.Id = User.Id;
            User = usr;
            US.UpdateUser(usr);
            new PropertyService().SetUser(usr);
            MessagingCenter.Send(this, "UserNameUpdated", usr);
        }


    }
}
