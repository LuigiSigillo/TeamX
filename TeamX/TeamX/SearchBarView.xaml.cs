using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchBarView : ContentView
	{
        bool cambiato = false;
        BoxView lab;
		public SearchBarView ()
        {
            
            MessagingCenter.Subscribe<FiltersPage,bool>(this, "ResetIcon", CambiaIcona);
            InitializeComponent ();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    sbar.PlaceholderColor = Color.Silver;
                    sbar.TextColor = Color.Gray;
                    break;
                case Device.Android:
                    sbar.TextColor = Color.White;
                    break;
            }
            filter.Source = ImageSource.FromResource("TeamX.Icons.filtericon.png");
        }

        private void CambiaIcona(FiltersPage obj, bool attivo)
        {
            if (attivo) {
               lab = new BoxView() {CornerRadius=5, BackgroundColor=Color.Violet, };
                AbsoluteLayout.SetLayoutBounds(lab,new Rectangle(0.97,0.27,10,10));
                AbsoluteLayout.SetLayoutFlags(lab, AbsoluteLayoutFlags.PositionProportional);
                abs.Children.Add(lab);
               cambiato = true;
            }
            else if (cambiato)
            {
                abs.Children.Remove(lab);
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var nuovotesto = e.NewTextValue as string;
            MessagingCenter.Send(this, "TextChanged",nuovotesto);

        }

        async private void Filterbutton_Clicked(object sender, EventArgs e)
        {
            // pusha la pagina di add dei filters
            await Navigation.PushAsync(new FiltersPage());
        }

       


    }
}