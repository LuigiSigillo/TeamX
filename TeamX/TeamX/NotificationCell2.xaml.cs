using System;
using System.Collections.Generic;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    /// <summary>
    /// Notification cell2
    /// This is the notification for the membership approved
    /// It is generated when the admin of a team accepts you in the team
    /// </summary>
    public partial class NotificationCell2 : ViewCell
    {
        private ViewNotification VN;

        public NotificationCell2()
        {
            InitializeComponent();
            VN = BindingContext as ViewNotification;
        }

       
    }
}
