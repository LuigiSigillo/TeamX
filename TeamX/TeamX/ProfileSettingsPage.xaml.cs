using System;
using System.Collections.Generic;
using TeamX.Database;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class ProfileSettingsPage : ContentPage
    {
        private User User;
        private ProfileService PS;
        

        public ProfileSettingsPage()
        {
            InitializeComponent();
        }

        public ProfileSettingsPage(User user)
        {

            InitializeComponent();

            UN_button.Text = Generator.GenerateIniziali(user.NickName);
            User = user; 
            user_name.Text = User.NickName;
            PS = new ProfileService(User.Id); 



        }



        void Logout_Pressed(object sender, System.EventArgs e)
        {
            new PropertyService().DisableUser();
            Navigation.PushAsync(new StartPage());
        }

        async void DeleteAccount_Pressed(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Warning", "Do you want to delete this account?", "YES", "NO"))
            {
                RemovingService.RemoveUser(User.Id);
                await Navigation.PushAsync(new StartPage());
            }
            else return;


        }
    }
}
