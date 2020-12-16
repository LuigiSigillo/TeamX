using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryDescriptionPopup : PopupPage
    {
        public string _message;

        public CategoryDescriptionPopup(string message)
        {
            InitializeComponent();
            MessageEditor.Text = message;
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_message))
            {
                string message = "Confirm leaving it blank?";
                if (!await DisplayAlert("Warning", message, "YES", "NO"))
                    return;

            }
            else 
            { 
                if (_message.Length > 150)
                {
                    string message = string.Format("Maximum number of characters for Description is 150. Yours is {0}.", _message.Length);
                    await DisplayAlert("Dscription too long", message, "OK");
                    return;
                }
            }

            MessagingCenter.Send(this, "CategoryDescription", _message);
            await PopupNavigation.Instance.PopAsync();
        }

        async void Cancel_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _message = (sender as Editor).Text;
        }
    }
}
