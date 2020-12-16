using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamX.Database;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamsBox : ContentView
	{
        public string Name { get; set; }
        static TeamCategoryService tcs = new TeamCategoryService();

        public TeamsBox() { }


        public TeamsBox (Team t)
		{
            InitializeComponent();
            BindingContext = t;
            try { backg.Source = CategoryService.GetImage(tcs.GetCategories(t.Id)[0]).Item1; }
            catch (Exception e) {
                backg.Source = ImageSource.FromResource("TeamX.immagini.loading2.jpg"); }

            member.Text = "   " + t.Members;
            teamButton.TextColor = TextColor(t);
            location.Text = Delete_char(t.City);
            if (string.IsNullOrWhiteSpace(location.Text))
            {
                location.Text = "No location";
            }

        }
        
        private string Delete_char(string stringa)
        {
            if (stringa.Length > 20)
                return stringa.Remove(20);
            else
            {
                while (stringa.Length < 12)
                    stringa = String.Concat(" " + stringa);

                return stringa;
            }
        }

        private async void TeamButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamPage(BindingContext as Team));

        }

        private Color TextColor(Team team)
        {
            try
            {
                return CategoryService.GetImage(tcs.GetCategories(team.Id)[0]).Item2;

            }
            catch (Exception e)
            {
                return Color.White;
            }
        }
    }
}