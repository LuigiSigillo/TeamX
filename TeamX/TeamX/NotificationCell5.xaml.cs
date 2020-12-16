using System;
using System.Collections.Generic;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class NotificationCell5 : ViewCell
    {
        private ViewNotification VN;


        public NotificationCell5()
        {
            InitializeComponent();
            VN = BindingContext as ViewNotification;
        }
    }
}
