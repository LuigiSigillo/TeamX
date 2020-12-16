using System;
using System.Collections.Generic;
using System.Text;
using TeamX.Database;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX.Models
{
    public class TeamsBox : Frame
    {
        public string Name { get; set; }
        public Button teamButton;
        public Label teamn;
        static TeamCategoryService tcs = new TeamCategoryService();

        /// <summary>
        /// costruttore con argomento il team che rappresenta quel box
        /// </summary>
        /// <param name="team"></param>
        public TeamsBox (Team team) 
        {
            BindingContext = team;
            var s = new AbsoluteLayout();
            Image backg = new Image() {Aspect=Aspect.AspectFill };
           
            try { backg.Source = CategoryService.GetImage(tcs.GetCategories(team.Id)[0]).Item1; }
            catch (Exception e)
            {
                backg.Source = ImageSource.FromResource("TeamX.immagini.loading.jpg");
            }
            CornerRadius = 30;
            HeightRequest = 140;
            Padding = 0;
            
            teamButton = new Button
            {
                Text =team.Name,
                TextColor = TextColor(team),
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 80,
                WidthRequest = 200,
                FontSize=15,
                BackgroundColor = Color.Transparent
            };
            var members = new Label() { Text= " " + team.Members,
                TextColor =Color.DarkSlateGray ,FontSize=20,
                VerticalTextAlignment = TextAlignment.Center
            };

            AbsoluteLayout.SetLayoutBounds(members, new Rectangle(0.1, 1, 0.4, 0.3));
            AbsoluteLayout.SetLayoutFlags(members, AbsoluteLayoutFlags.All);
            s.Children.Add(members);

            var location = new Label() { Text =Delete_char(team.City) , TextColor = Color.DarkSlateGray,
                VerticalTextAlignment=TextAlignment.Center
            };

            AbsoluteLayout.SetLayoutBounds(location, new Rectangle(0.9, 1, 0.5, 0.3));
            AbsoluteLayout.SetLayoutFlags(location, AbsoluteLayoutFlags.All);
            s.Children.Add(location);


            AbsoluteLayout.SetLayoutBounds(backg, new Rectangle(0, 0, 1, 0.7));
            AbsoluteLayout.SetLayoutFlags(backg, AbsoluteLayoutFlags.All);
            s.Children.Add(backg);

            AbsoluteLayout.SetLayoutBounds(teamButton, new Rectangle(0, 0, 1, 0.8));
            AbsoluteLayout.SetLayoutFlags(teamButton, AbsoluteLayoutFlags.All);
            s.Children.Add(teamButton);

            s.HeightRequest = 140;
            Content = s;
            teamButton.Clicked += TeamButtonClicked; 
        }

        private string Delete_char (string stringa)
        {
            if (stringa.Length > 20)
                return stringa.Remove(20);
            else if (stringa == "")
                stringa = "Anywhere"; 
            {
                while (stringa.Length < 12)
                   stringa= String.Concat(" " + stringa);
                
                return stringa;
            }
        }

        private async void TeamButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamPage(BindingContext as Team));
            
        }

        private Color TextColor (Team team)
        {
            try {
                return CategoryService.GetImage(tcs.GetCategories(team.Id)[0]).Item2;

            }
            catch (Exception e)
            {
                return Color.White;
            }
        }
    }
}
