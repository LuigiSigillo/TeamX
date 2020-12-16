using System;
using TeamX.Utils;

namespace TeamX.Models
{
    /// <summary>
    /// Notification.
    /// <see cref="Utils.ActiveNotificationBuilde"/>
    /// </summary>
    public class Notification
    {

        public string Id { get; set; }
        public string SenderID { get; private set; }
        public string ReceiverID { get; private set; }
        public string TeamID { get; private set; }
        public int Type { get; private set; }
        public string Message { get; private set; } //Message è opzionale

        public Notification(string sender, string receiver, string team, int type, string message = null)
        {
            Id = Generator.GenerateID();
            SenderID = sender;
            ReceiverID = receiver;
            TeamID = team;
            Type = type;
            Message = message;
        }
    }
}
