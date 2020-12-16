using System;
using System.Collections.Generic;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class NotificationCell4 : ViewCell
    {
        private ViewNotification VN;


        public NotificationCell4()
        {
            InitializeComponent();
            VN = BindingContext as ViewNotification;
        }
    }
}