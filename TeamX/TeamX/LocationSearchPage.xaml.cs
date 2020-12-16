using DurianCode.PlacesSearchBar;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationSearchPage : PopupPage
	{
        static string googlePlacesApi = "";
       

                                            
        public LocationSearchPage ()
		{
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    googlePlacesApi = ""; // per ora uguali
                    break;
                case Device.Android:
                    googlePlacesApi = "";
                    break;
            }

            InitializeComponent ();
            locationBar.ApiKey = googlePlacesApi;
            locationBar.Type = PlaceType.Cities;
            Results_list.ItemSelected += Results_List_ItemSelected;
            google.Source = ImageSource.FromResource("TeamX.immagini.poweredbygoogle.png");
        }
        protected override void OnAppearing()
        {
            locationBar.Focus();
            base.OnAppearing();
        }

        async private void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            Results_list.SelectedItem = null;

            var place = await Places.GetPlace(prediction.Place_ID, googlePlacesApi);
           
            if (!string.IsNullOrWhiteSpace(place.Name))
            {
                MessagingCenter.Send(this, "LocationTeam", place.Name);
                MessagingCenter.Send(this, "LocationFilter", place.Name);
                MessagingCenter.Send(this, "LocationDetails", (place.Latitude,place.Longitude));

            }
           
            await PopupNavigation.Instance.PopAsync();
        }

        private void LocationBar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            Results_list.ItemsSource = result.AutoCompletePlaces;

            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                Results_list.IsVisible = true;
        }

        private void LocationBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
                Results_list.IsVisible = false;
            else
                Results_list.IsVisible = true;
        }

      
      

        
    }
}