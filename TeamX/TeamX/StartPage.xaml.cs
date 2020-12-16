using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TeamX
{
    public partial class StartPage : ContentPage
    {


        public StartPage()
        {
            InitializeComponent();

            background.Source = ImageSource.FromResource("TeamX.immagini.home.jpg");
        }

        void LoginPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        void StartButtonPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}
