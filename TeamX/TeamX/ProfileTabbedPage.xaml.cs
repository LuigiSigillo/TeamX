using System;
using System.Collections.Generic;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class ProfileTabbedPage : TabbedPage
    {
        private ProfileNavigationBar PNB = new ProfileNavigationBar();
        private bool IsAdmin { get; set; }
        private User User; 

        public ProfileTabbedPage(User user)
        {
            InitializeComponent();

            User = user;

            IsAdmin = new PropertyService().GetUser().Id == user.Id;

            Children.Add(new ProfileAboutPage(user));
            Children.Add(new ProfileTeamsPage(user));

            if(IsAdmin) 
            { 
                Children.Add(new ProfileSettingsPage(user));
                MessagingCenter.Subscribe<ViewNotification>(this, "Ntf_removed", RefreshNavigationBar);
                MessagingCenter.Subscribe<ProfileDetailPage, User>(this, "UserNameUpdated", NameUpdated);
            }


            SetNavigationBar();




        }

        private void SetNavigationBar()
        {
            if (IsAdmin)
            {

                NavigationPage.SetTitleView(this, PNB);
                NavigationPage.SetHasBackButton(this, false);
                return;
            }

            NavigationPage.SetTitleView(this, new UserNavigationBar(User));return;


        }

        private void RefreshNavigationBar(ViewNotification obj)
        {
            PNB.Refresh();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void NameUpdated(ProfileDetailPage arg1, User arg2)
        {
            Title = "Profile page";
        }

    }
}
