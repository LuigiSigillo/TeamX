using System;
using TeamX.Services;
using TeamX.Models;
using TeamX.Utils;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using TeamX.Database;
using Rg.Plugins.Popup.Services;

namespace TeamX
{
    public partial class NotificationPage : ContentPage , INotifyPropertyChanged
    {
        bool alreadyOpened { get; set; }
        private INotificationService NS;
        private ObservableCollection<ViewNotification> viewNotifications;
        public ObservableCollection<ViewNotification> ViewNotifications
        {
            get { return viewNotifications; }
            set
            {
                viewNotifications = value;
                OnPropertyChanged(nameof(ViewNotifications));
            }
        }



        public NotificationPage()
        {
            InitializeComponent();

            NS = new NotificationService();

            LoadViewNotifications();
            CheckLabel();

            BindingContext = this;


            MessagingCenter.Subscribe<ViewNotification>(this, "View_team", ViewTeam);
            MessagingCenter.Subscribe<ViewNotification>(this, "View_profile", ViewProfile);
            MessagingCenter.Subscribe<ViewNotification>(this, "Ntf_removed", Remove_ntf);
        }



        

        private void ViewProfile(ViewNotification obj)
        {
            Navigation.PushAsync(new ProfileTabbedPage(obj.Sender));
        }

        void ViewTeam(ViewNotification obj)
        {
            Navigation.PushAsync(new TeamPage(obj.Team));
        }


        void CheckLabel()
        {
            if (ViewNotifications.Count == 0)
            {
                noNtfLbl.IsVisible = true;
                NTFlistview.SeparatorColor = Color.Transparent;
            }
            else noNtfLbl.IsVisible = false;

        }


        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            NTFlistview.SelectedItem = null;
        }

        private void LoadViewNotifications()
        {
            //Prende tutte le notifiche dell'utente dal database
            var ntfs = NS.GetNotificationsReceived(new PropertyService().GetUser().Id);

            //Translating the notifications into view-notifications
            var vntfs = new ObservableCollection<ViewNotification>();
            foreach (Notification ntf in ntfs)
            {
                vntfs.Add(new ViewNotification(ntf));
            }
            ViewNotifications = vntfs;

        }

        private void Remove_ntf(ViewNotification VN)
        {
            ViewNotifications.Remove(VN);
            CheckLabel();
        }


    }
}
