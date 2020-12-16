using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamUpdated_Popup : PopupPage
	{
        Team _team;



        public TeamUpdated_Popup (Team team)
		{
			InitializeComponent ();
            _team = team;
        }

        private async void GoToTeam(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync(true);
            await Navigation.PushAsync(new TeamPage(_team), true);
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}