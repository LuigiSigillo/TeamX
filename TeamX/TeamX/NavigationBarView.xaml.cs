using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Database;
using TeamX.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationBarView : ContentView
	{
		public NavigationBarView ()
		{
			InitializeComponent ();
            user.Source = ImageSource.FromResource("TeamX.Icons.usericon.png");
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

        async private void User_Pressed(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new ProfileTabbedPage(new PropertyService().GetUser()));
           
        }
    }
}