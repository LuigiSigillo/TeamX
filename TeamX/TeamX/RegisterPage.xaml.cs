﻿using System;
using System.Collections.Generic;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class RegisterPage : ContentPage
    {
        private IUserService US;
        private IPropertyService PS;

        public RegisterPage()
        {
            InitializeComponent();
            background.Source = ImageSource.FromResource("TeamX.immagini.home.jpg");
            US = new UserService();
            PS = new PropertyService();
        }




        /// <summary>
        /// Verifies the user has correctly inserted the data in all the fields
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="email">Email.</param>
        /// <param name="name">Name.</param>
        /// <param name="pwd">Pwd.</param>
        /// <param name="cp_pwd">Cp pwd.</param>

        private User VerifyUser(string email, string name) {


            //Verifying all the fields are different from null
            if (email == null || name == null)
            {
                Handle_nullValues();
                return null;
            }

            if (!Validator.ValidateName(name))
            {
                Handle_WrongName();
                return null;
            }


            if (!Validator.ValidateEmail(email)) 
            {
                HandleWrongEmail();
                return null;
             }

            //Verifying email inserted is not already registered
            if (US.FindUserByEmail(email)!=null)
            {
                Handle_existingEmail();
                return null;
            }

            //Verifying nickname inserted is available
            if (US.FindUserByName(name)!=null)
            {
                Hande_existingName(name);
                return null;
            }

            //Creating the user 
            //NB: the id should be created by the DB when inserting 
            //--- it into the database 
            var pwd = Generator.GeneratePWD();
            var user = new User(email, pwd, name);

            //now send email
            Mailer.SendPasswordEmail(email, name, pwd);
            Email_Sent();

            return user;
        }





        /*Handle* methods: gestione dell'interazione con l'utente*/
        private async void Email_Sent()
        {
            await DisplayAlert("Password sent", "An autogenerated password has been sent to your email.", "OK");
            return;
        }

        private async void Handle_WrongName()
        {
            await DisplayAlert("Error", "The name should be at least 2 characters long", "OK");
            return;
        }

        async void HandleWrongEmail()
        {
            await DisplayAlert("Error", "Please insert a valid email", "OK");
            return;
        }

        private async void Handle_nullValues(){
            await DisplayAlert("Error", "Please complete all the fields.", "OK");
        }

        private async void Handle_registrationFailed(){
            await DisplayAlert("Error", "Sorry, an error occured. Please try again.", "OK"); 
        }

        private async void Handle_existingEmail()
        {
            await DisplayAlert("Error", "The email inserted is already in use.", "OK");
        }

        private async void Hande_existingName(string name)
        {
            await DisplayAlert("Error", "The name " + name + " is not available.", "OK");
        }


        /// <summary>
        /// Registers the user if it's not present a user with the same name or email.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Register_pressed(object sender, System.EventArgs e)
        {


            var user = VerifyUser(EmailEntry.Text, NicknameEntry.Text);

            if (user == null) return;


            //if all the fields are correct 
            //the user is inserted into the database 
            //and the user property is set
            if (US.InsertUser(user)) {

                //Navigating to the mainpage
                await Navigation.PushAsync(new LoginPage());
                return;
            }


            //The operation failed: show some message
            Handle_registrationFailed();
            return;


        }

        
    }
}