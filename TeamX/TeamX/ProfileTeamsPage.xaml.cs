using System;
using System.Collections.Generic;
using System.Windows.Input;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class ProfileTeamsPage : ContentPage
    {

        private User User;
        private int Following;
        private int Followed;

        private ProfileService PS;
        private ITeamCategoryService TCS = new TeamCategoryService();
        private ITeamMemberService TMS = new TeamMemberService();

        public List<TeamViewGroup> TeamViewGroups { get; set; }


        public ProfileTeamsPage()
        {
            InitializeComponent();

        }

        public ProfileTeamsPage(User user)
        {
            InitializeComponent();

            UN_button.Text = Generator.GenerateIniziali(user.NickName);
            User = user;
            BindingContext = User;
            PS = new ProfileService(User.Id);

            user_name.Text = User.NickName;

            PrepareTeamsListView();


            Followed = PS.GetFollowed();
            Following = PS.GetFollowing();

            FollowingLbl.Text = Following.ToString() + " teams";
            FollowedLbl.Text = Followed.ToString() + " teams";

            EditBtn.IsVisible = user.Id == new PropertyService().GetUser().Id;

            EditBtn.Source = ImageSource.FromResource("TeamX.Icons.useredit.png");


        }

        private void PrepareTeamsListView()
        {
            var teams = TMS.GetTeams(User.Id);
            TeamViewGroups = new List<TeamViewGroup>();
            for (int i = 1; i < CategoryService.GetMaxCategory(); i++)
            {
                var kat = CategoryService.GetCategory(i);
                var teams4cat = teams.FindAll((obj) => TCS.IsCategory(obj.Id, i));
                //Aggiungo il gruppo solo se è presente almeno una squadra di quel gruppo
                if (teams4cat.Count > 0)
                {
                    TeamViewGroup tvg = new TeamViewGroup(kat, i.ToString());
                    foreach (Team team in teams4cat)
                    {
                        var tv = new TeamView(team, kat);
                        tvg.Add(tv);

                    }
                    TeamViewGroups.Add(tvg);
                }
            }
            TeamsLV.ItemsSource = TeamViewGroups;
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            TeamsLV.SelectedItem = null;
        }

        async void TeamTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as TeamView;
            await Navigation.PushAsync(new TeamPage(item.Team));
            TeamsLV.SelectedItem = null;
        }

        void Edit_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProfileDetailPage(User));
        }


    }
}
