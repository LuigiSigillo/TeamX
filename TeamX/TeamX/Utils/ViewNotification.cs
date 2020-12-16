using System;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using TeamX.Database;
using TeamX.Models;
using Xamarin.Forms;

namespace TeamX.Utils
{
    public class ViewNotification
    {

        public Notification Ntf { get; set; }
        public User Sender { get; set; }
        public Team Team { get; set; }
        public string DescriptionLabel1 { get; set; }
        public string DescriptionLabel2 { get; set; }
        public string DescriptionLabel3 { get; set; }
        public string DescriptionLabel4 { get; set; }
        public string DescriptionLabel5 { get; set; }
        public string Message { get; set; }

        public ICommand ViewProfile_Command { get; set; }
        public ICommand ViewTeam_Command { get; set; }
        public ICommand AcceptRequest_Command { get; set; }
        public ICommand MarkSeen_Command { get; set; }
        public ICommand RefuseRequest_Command { get; set; }

        public ViewNotification(Notification ntf)
        {

            Ntf = ntf;
            Sender = (new UserService()).GetUser(ntf.SenderID);
            Team = (new TeamService()).GetTeam(ntf.TeamID);

            Message = ntf.Message;

            if (Team != null)
            {
                DescriptionLabel1 = string.Format("{0} requested to be part of your team {1}:", Sender.NickName.ToUpper(), Team.Name);
                DescriptionLabel2 = string.Format("You are now part of the team {0}:", Team.Name);
                DescriptionLabel3 = string.Format("{0} is no more part of the team {1}:",Sender.NickName, Team.Name);
                DescriptionLabel5 = string.Format("Your request to be part of {0} has been declined :", Team.Name);
            }
            DescriptionLabel4 = string.Format("The team {0} has been removed:", Message);

            ViewProfile_Command = new Command(ViewProfile_Handle); 
            ViewTeam_Command = new Command(ViewTeam_Handle);
            AcceptRequest_Command = new Command(AcceptRequest_Handle);
            MarkSeen_Command = new Command(MarkSeen_Handle);
            RefuseRequest_Command = new Command(RefuseRequest_Handle);
        }

        private void RefuseRequest_Handle(object obj)
        {

            MessagingCenter.Subscribe<SendRequest_PopUp, string>(this, "RequestMessage", SendNotification);

            PopupNavigation.Instance.PushAsync(new SendRequest_PopUp());

        }

        private void SendNotification(SendRequest_PopUp arg1, string arg2)
        {
            new NotificationService().AddNotification(new Notification(Ntf.ReceiverID, Ntf.SenderID, Ntf.TeamID, 5, arg2));

            new NotificationService().RemoveNotification(Ntf.Id);

            MessagingCenter.Send(this, "Ntf_removed");
        }

        void MarkSeen_Handle(object obj)
        {

            new NotificationService().RemoveNotification(Ntf.Id);

            MessagingCenter.Send(this, "Ntf_removed");
        }


        void ViewProfile_Handle(object obj)
        {
            MessagingCenter.Send(this, "View_profile");
        }


        void ViewTeam_Handle(object obj)
        {

            //Removing the notification from the database
            new NotificationService().RemoveNotification(Ntf.Id);

            MessagingCenter.Send(this, "Ntf_removed");

            MessagingCenter.Send(this, "View_team");

        }

        void AcceptRequest_Handle(object obj)
        {
            //Notifying the notification page the request was accepted  
            // -> the page will remove the notification

            //Removing the notification from the database
            new NotificationService().RemoveNotification(Ntf.Id);

            MessagingCenter.Send(this, "Ntf_removed");

            //Adding the user to the team
            new TeamMemberService().AddUser(Team.Id, Sender.Id);

            new TeamService().IncrementMembersNumber(Team.Id);
            //The accepted user will be notified 
            new NotificationService().AddNotification(new Notification(Ntf.ReceiverID, Ntf.SenderID, Ntf.TeamID, 2, "prova0"));

        }

    }
}
