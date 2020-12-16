using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class MainPage : ContentPage
    {
        //A reference to the user active on the app
        private User User;
        private IPropertyService PS;

        public MainPage()
        {
            InitializeComponent();
            PS = new PropertyService();
            //NB: there's no control over the value of user 
            //--because the login has been done at this point
            //--so there must be a user active
            User = PS.GetUser();

        }


        protected override void OnAppearing()
        {
            //This has been inserted to test the property service
            userStuff.BindingContext = User;
        }


        //The user is removed from the application properties 
        //The page will move to start page
        //NB: this will be refactored : 
        //---the logout button will be in the settings page
        void Logout_Pressed(object sender, System.EventArgs e)
        {
            PS.DisableUser();
            Navigation.PushAsync(new StartPage());
        }

        private void Teams_Pressed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new TeamsGrid()) { BarBackgroundColor = Color.FromHex("#00EA75") });
        }
    }
}
