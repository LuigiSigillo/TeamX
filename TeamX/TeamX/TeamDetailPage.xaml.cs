using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamDetailPage : ContentPage
	{
        PropertyService PS = new PropertyService();
        ITeamService TS = new TeamService();
        ITeamCategoryService TCS = new TeamCategoryService();
        ITeamMemberService TMS = new TeamMemberService();
        ILocationDetailsService LS = new LocationDetailsService();

        List<string> _categories;
        int team_category_ID;
        int old_teamCategory_ID;

        Team team;

		public TeamDetailPage(Team t)
		{
            if (t == null)
                throw new ArgumentNullException(nameof(t));

			InitializeComponent ();

            MessagingCenter.Subscribe<LocationSearchPage, string>(this, "LocationTeam", SetLocation);
            MessagingCenter.Subscribe<LocationSearchPage, (double,double)>(this, "LocationDetails", SetLocationDetails);

            _categories = CategoryService.GetCategories();
            Category_Picker.ItemsSource = _categories;
            Difficulty_Picker.ItemsSource = DifficultyService.GetDifficulties();

            team = new Team
            {
                Id = t.Id,
                Name = t.Name,
                CreatorID = t.CreatorID,
                City = t.City,
                Description = t.Description,
                MaxMembers = t.MaxMembers,
                NumOfMembers = t.NumOfMembers,
                PlaceDetails = t.PlaceDetails,
                CreationDate = t.CreationDate,
                Difficulty = t.Difficulty,
                TerminationDate = t.TerminationDate
            };
            BindingContext = team;

            if (TS.GetTeam(t.Id) != null)
            {
                PageTitle_Label.Text = "Edit your team";
                Category_Picker.SelectedIndex = TCS.GetCategories(t.Id)[0] - 1;
                old_teamCategory_ID = Category_Picker.SelectedIndex + 1;
                Remove_button.IsVisible = true;
                Difficulty_Picker.SelectedIndex = t.Difficulty - 1;
            }
        }

        private void SetLocation(LocationSearchPage locationSearchPage, string location)
        {
            if (!string.IsNullOrWhiteSpace(location))
                (BindingContext as Team).City = location;
        }

        private void SetLocationDetails(LocationSearchPage arg1, (double lati,double longi) tu)
        {            
                (BindingContext as Team).PlaceDetails = tu;
        }

        private void OnCategorySelected(object sender, EventArgs e)
        {
            team_category_ID = (sender as Picker).SelectedIndex + 1;
        }

        private void OnDifficultySelected(object sender, EventArgs e)
        {
            (BindingContext as Team).Difficulty = (sender as Picker).SelectedIndex + 1;
        }

        private void OnTerminationDateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as Team).TerminationDate = (sender as DatePicker).Date;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if (team.MaxMembers == 0 || team.Name == null || team.Description == null || team_category_ID == 0 || team.Difficulty == 0)
            {
                await DisplayAlert("Warning", "Complete the required fields", "Ok");
                return;
            }

            if(team.Name.Length > 40)
            {
                string message = string.Format("Maximum number of characters for Name is 40. Yours is {0}.", team.Name.Length);
                await DisplayAlert("Name too long", message, "OK");
                return;
            }

            if (team.Description.Length > 225)
            {
                string message = string.Format("Maximum number of characters for Description is 225. Yours is {0}.", team.Description.Length);
                await DisplayAlert("Description too long", message, "OK");
                return;
            }

            //if it is a new team
            if (team.Id == null)
            {
                var response = await DisplayAlert("Complete team creation", "Choose 'Yes' to complete the creation of your team, 'Cancel' otherwise", "Yes", "Cancel");
                if (response == false) return;

                team.CreatorID = PS.GetUser().Id;   //Set the actual User as Creator/Admin of the new team
                team.Id = Generator.GenerateID();

                MessagingCenter.Send(this, "TeamAdded", team);


                TCS.AddTeamCategory(team.Id, team_category_ID);
                TMS.AddAdmin(team.Id, team.CreatorID);
                TCS.AddTeamCategory(team.Id, team_category_ID);
                LS.AddLocationDetails(team.City, team.PlaceDetails.Latitude, team.PlaceDetails.Longitude);

                await PopupNavigation.Instance.PushAsync(new TeamCreated_Popup(team));
            }
            else //if the team already exists
            {
                var response = await DisplayAlert("Complete team update", "Choose 'Yes' to complete the update of your team, 'Cancel' otherwise", "Yes", "Cancel");
                if (response == false) return;

                TCS.UpdateTeamCategory(team.Id, old_teamCategory_ID, team_category_ID);
                LS.AddLocationDetails(team.City, team.PlaceDetails.Latitude, team.PlaceDetails.Longitude);

                MessagingCenter.Send(this, "TeamUpdated", team);

                await PopupNavigation.Instance.PushAsync(new TeamUpdated_Popup(team));
            }
        }

        private async void Location_Focused(object sender, FocusEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LocationSearchPage());
            Location.Unfocus();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnRemove(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Remove team", "Are you sure?", "Yes", "No");
            if(response==true)
            {
                string teamID = (BindingContext as Team).Id;
                MessagingCenter.Send(this, "TeamRemoved",teamID);
                await Navigation.PopToRootAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}