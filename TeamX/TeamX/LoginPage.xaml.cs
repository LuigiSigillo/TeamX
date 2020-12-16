using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class LoginPage : ContentPage
    {
        private IUserService US;
        private IPropertyService PS;


        public LoginPage()
        {
            InitializeComponent();


            background.Source = ImageSource.FromResource("TeamX.immagini.home.jpg");
            US = new UserService();
            PS = new PropertyService();
        }


        //Questa funzione fa due cose : 
        //--1. Verifica le credenziali e lancia messaggi di errore differenti
        //----- per email e pwd errate
        //--2. Ritorna l'utente se questo è presente nel database all'event_handler 
        //-----del login, null se l'utente non è stato trovato
        private User VerifyUser(string email_name, string pwd)
        {
            var id = US.FindUserByEmail(email_name);
            if (id == null)
            {
                id = US.FindUserByName(email_name);
            }
            //The user has not been found: 
            //The email used must be wrong 
            if (id == null)
            {
                Hanlde_WrongEmailorName();
                return null;
            }

            var usr = US.GetUser(id);

            //verifying the password has been inserted correctly

            var matching = usr.Pwd.Equals(pwd);

            //The pwd used must be wrong
            if (!matching)
            {
                Hanlde_NotMatching();
                return null;
            }



            return usr;
        }


        //Queste tre funzioni "Handle*" sono addette alla gestione degli errori 
        //livello utente (interfaccia). Sono state separate dalle funzioni di 
        //calcolo per essere indipendenti da queste e aggiornabili con più facilità
        private async void Hanlde_WrongEmailorName()
        {

            await DisplayAlert("Wrong email", "Please check the email or name inserted.", "OK");
            EmailorNameEntry.Text = null;
            PasswordEntry.Text = null;
        }

        private async void Hanlde_NotMatching()
        {

            await DisplayAlert("Wrong password", "Please check the password", "OK");
            PasswordEntry.Text = null;
        }

        private async void Handle_BlankFields()
        {
            await DisplayAlert("Empty field", "Please complete all the fields", "OK");
        }



        async void LoginPressedAsync(object sender, System.EventArgs e)
        {

            if (EmailorNameEntry.Text == null || PasswordEntry.Text == null)
            {
                Handle_BlankFields();
                return;
            }


            var user = VerifyUser(EmailorNameEntry.Text, PasswordEntry.Text);

            if (user != null)
            {

                //Saving the user in the application properties
                PS.SetUser(user);


                //Opening MainPage
                Application.Current.MainPage = new NavigationPage(new TeamsGrid());
                await Navigation.PopToRootAsync();
            }

            return;
        }

        async void Handle_Pressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }
    }
}
