using System;
using System.Collections.Generic;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    /// <summary>
    /// Notification cell1
    /// This is the notification cell for the notification of team membership requests.
    /// This notification is sent to the team admin when a user sends the request.
    /// The team admin can view the user's profile or accept his request
    /// </summary>
    public partial class NotificationCell1 : ViewCell
    {
        private ViewNotification VN;


        public NotificationCell1()
        {
            InitializeComponent();
            VN = BindingContext as ViewNotification;
        }


       
    }
}
