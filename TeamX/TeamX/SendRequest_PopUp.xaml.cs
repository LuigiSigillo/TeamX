using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendRequest_PopUp : PopupPage
    {
        public string _message;

		public SendRequest_PopUp ()
		{
			InitializeComponent ();
		}

        private async void Done_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_message))
            {
                if (_message.Length > 200)
                {
                    string message = string.Format("Maximum number of characters for Message is 200. Yours is {0}.", _message.Length);
                    await DisplayAlert("Message too long", message, "OK");
                    return;
                }
            }

            MessagingCenter.Send(this, "RequestMessage", _message);
            await PopupNavigation.Instance.PopAsync();
        }

        private void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _message = (sender as Editor).Text;
        }
    }
}