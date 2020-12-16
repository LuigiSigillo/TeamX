using System;
using TeamX.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TeamX.Utils;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TeamX
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {


            if (Current.Properties.ContainsKey("User"))
            { MainPage = new NavigationPage(new TeamsGrid()); }

            else MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
