using System;
using System.Collections.Generic;
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
	public partial class TeamMemberButton : ContentView
	{
        public User _user { get; private set; }
        string iniziali;

		public TeamMemberButton (User user)
		{
			InitializeComponent ();

            iniziali = Generator.GenerateIniziali(user.NickName);
            button.Text = iniziali;

            _user = user;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await button.ScaleTo(0.8, 150);
            await button.ScaleTo(1, 150);
            await Navigation.PushAsync(new ProfileTabbedPage(_user), true);
        }

        public void ChangeButtonColor(Color backgroundColor, Color textColor)
        {
            button.BackgroundColor = backgroundColor;
            button.TextColor = textColor;
        }
    }
}