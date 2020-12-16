using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TeamX.Database;
using TeamX.Utils;

namespace TeamX
{
    public partial class ProfileNavigationBar : ContentView
    {
        public ProfileNavigationBar()
        {
            InitializeComponent();
            teams.Source = ImageSource.FromResource("TeamX.Icons.home.png");
            Refresh();
        }


        public void Refresh()
        {
            if (new NotificationService().GetNotificationsReceived(new PropertyService().GetUser().Id).Count > 0)
            {
                Notifications.Source = ImageSource.FromResource("TeamX.Icons.newnotification.png");
            }
            else Notifications.Source = ImageSource.FromResource("TeamX.Icons.notificationicon.png");

        }

        async private void Notification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());

        }


        void HomePage_Pressed(object sender, System.EventArgs e)
        {

            Application.Current.MainPage = new NavigationPage(new TeamsGrid());
        }

       

    }


}
