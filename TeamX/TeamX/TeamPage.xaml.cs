using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
    public partial class TeamPage : ContentPage
    {
        PropertyService PS;
        INotificationService NS;
        ITeamCategoryService TCS;
        ITeamMemberService TMS;
        ITeamService TS;
        IUserService US;

        string _userID;    //actual user

        bool AlreadyOpened { get; set;}

        //Ho aggiunto un riferimento alle categorie del team 
        List<int> team_categories;
        
        public TeamPage(Team t)
        {
            InitializeComponent();

            BindingContext = t;

            PS = new PropertyService();
            NS = new NotificationService();
            TCS = new TeamCategoryService();
            TMS = new TeamMemberService();
            TS = new TeamService();
            US = new UserService();

            _userID = PS.GetUser().Id;

            if (IsAdmin(t.CreatorID))
            {
                Edit_button.IsVisible = true;
            }
            else
            {
                if (NS.ExistsNotification(_userID, t.Id)
                || TMS.IsTeamMember(t.Id, _userID) || t.NumOfMembers == t.MaxMembers || TMS.GetRequests(t.Id,_userID) > 3)
                    SendRequest_button.IsVisible = false;
                else SendRequest_button.IsVisible = true;
                if (TMS.IsTeamMember(t.Id, _userID))
                    RemoveUser_button.IsVisible = true;
            }

            //Ho assegnato il riferimento alle categorie del team 
            team_categories = TCS.GetCategories(t.Id);

            CategoryLabel.Text = CategoryService.GetCategory(team_categories[0]) + " - " + DifficultyService.GetDifficulty(t.Difficulty);

            teamImage.Source = CategoryService.GetImage(team_categories[0]).Item1;

            foreach(TeamMemberButton b in CreateTMButtons(t))
            {
                if (t.CreatorID==b._user.Id) b.ChangeButtonColor(Color.FromHex("#A1A1A1"), Color.White);
                teamMembersStack.Children.Add(b);
            }

            creationDateLabel.Text = "Created on " + t.CreationDate.Date.ToString("dd MMMM yyyy");
            terminationDateLabel.Text = "Subscriptions end on " + t.TerminationDate.Date.ToString("dd MMMM yyyy");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (AlreadyOpened) return;
            MessagingCenter.Subscribe<SendRequest_PopUp, string>(this, "RequestMessage", SendNotification);
            AlreadyOpened = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<SendRequest_PopUp, string>(this, "RequestMessage");

        }

        private void SendNotification(SendRequest_PopUp arg1, string _message)
        {
            TMS.AddRequest((BindingContext as Team).Id, _userID);
            Notification notification = new Notification(PS.GetUser().Id, ((Team)BindingContext).CreatorID, ((Team)BindingContext).Id, 1, _message);
            NS.AddNotification(notification);
            SendRequest_button.IsVisible = false;
        }

        /// <summary>
        /// Check if the user is the page admin.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if user is admin, false otherwise</returns>
        private bool IsAdmin(string id)
        {
            return string.Equals(id,PS.GetUser().Id);
        }

        private async void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamDetailPage(BindingContext as Team), true);
        }

        private async void OnSendRequest(object sender, EventArgs e)
        {
            if (await DisplayAlert("Warning", "You can't send more than 3 requests to join the team. Confirm sending?", "YES", "NO"))
            {
                await PopupNavigation.Instance.PushAsync(new SendRequest_PopUp());
            }
           
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void RemoveUser_button_Clicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Exit from team", "Are you sure?", "Yes", "No");
            if (response == true)
            {
                RemovingService.RemoveUserFromTeam(_userID, (BindingContext as Team).Id);
            }
        }

        private List<TeamMemberButton> CreateTMButtons(Team team)
        {
            List<TeamMemberButton> buttons = new List<TeamMemberButton>();
            List<User> teamMembers = TMS.GetMembers(team.Id);

            foreach (User u in teamMembers)
                buttons.Add(new TeamMemberButton(u));

            return buttons;
        }
    }
}