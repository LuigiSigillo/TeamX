using DurianCode.PlacesSearchBar;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersPage : ContentPage
    {
        List<string> categories;
        List<string> difficulties;
        public static Filter filtri = new Filter();
        bool dis, cat, loc,diff = false;
        
        public FiltersPage()
        {
            InitializeComponent();


            categories = new List<string> {"Empty"};
            categories.AddRange(CategoryService.GetCategories());
            Pick.ItemsSource = categories;
            // mettere la source delle difficoltà 
            difficulties = new List<string> { "Empty" };
            difficulties.AddRange(DifficultyService.GetDifficulties());
            Pick_Diff.ItemsSource = difficulties;
            BindingContext = filtri; 

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<LocationSearchPage, string>(this, "LocationFilter", SetLocation);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LocationSearchPage, string>(this, "LocationFilter");
        }

        private void SetLocation(LocationSearchPage arg1, string arg2)
        {
            if (!string.IsNullOrWhiteSpace(arg2))
            {
                filtri.City = arg2;
                loc = true;
            }
            else 
                loc = false;
        }

        // da implementare il filtrare in base alla distanza 499307=>matricola Speranza
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            filtri.Distance = e.NewValue;
            if (filtri.Distance > 1)
                dis = true;
            else
            {
                dis = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pick_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Pick.SelectedIndex == 0)
            {
                Pick.SelectedIndex = -1;
                cat = false;  
            }
            else
            {
                cat = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pick_SelectedIndexChanged_Diff(object sender, EventArgs e)
        {
            if (Pick_Diff.SelectedIndex == 0)
            {
                Pick_Diff.SelectedIndex = -1;
                diff = false;
            }
            else
            {
                diff = true;
            }
        }

        /// <summary>
        /// apply button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Postino();
            AnyFilters();
            await Navigation.PopAsync();
        }


        private void Postino ()
        {
            MessagingCenter.Send(this, "LocationChanged", filtri.City);
            MessagingCenter.Send(this, "CategoryChanged", Pick.SelectedIndex);
            MessagingCenter.Send(this, "DistanceChanged", filtri.Distance);
            MessagingCenter.Send(this, "DifficultyChanged", Pick_Diff.SelectedIndex);
            MessagingCenter.Send(this, "Update");
        }

        private void RemoveCategory_Clicked(object sender, EventArgs e)
        {
            Pick.SelectedIndex = -1;
            cat = false;
            MessagingCenter.Send(this, "CategoryChanged", Pick.SelectedIndex);
            MessagingCenter.Send(this, "Update");
            AnyFilters();
        }

        private void RemoveDifficulty_Clicked(object sender, EventArgs e)
        {
            Pick_Diff.SelectedIndex = -1;
            diff = false;
            MessagingCenter.Send(this, "DifficultyChanged", Pick_Diff.SelectedIndex);
            MessagingCenter.Send(this, "Update");
            AnyFilters();
        }

        private void RemoveDistance_Clicked(object sender, EventArgs e)
        {
            filtri.Distance = 1;
            dis = false;
            MessagingCenter.Send(this, "DistanceChanged", filtri.Distance);
            MessagingCenter.Send(this, "Update");
            AnyFilters();
        }

        private void RemoveCity_Clicked(object sender, EventArgs e)
        {
            filtri.City = null;
            loc = false;
            MessagingCenter.Send(this, "LocationChanged", filtri.City);
            MessagingCenter.Send(this, "Update");
            AnyFilters();
        }

        async private void Location_Focused(object sender, FocusEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LocationSearchPage());
                Location.Unfocus();
        }



        /* =====================================================================*/
        private void Reset_Clicked(object sender, EventArgs e)
        {
            Pick.SelectedIndex = -1;
            filtri.City = null;
            filtri.Distance = 1;
            Pick_Diff.SelectedIndex = -1;
            MessagingCenter.Send(this,"Reset");
            MessagingCenter.Send(this, "ResetIcon",false);

        }

        private void AnyFilters ()
        {
            MessagingCenter.Send(this, "ResetIcon", (cat || dis || loc|| diff));
        }

    }
}
