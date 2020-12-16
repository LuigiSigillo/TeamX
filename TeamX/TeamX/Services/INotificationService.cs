using System;
using System.Collections.Generic;
using TeamX.Models;

namespace TeamX.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Gets all the available notifications for the specified user.
        /// </summary>
        /// <returns>The notifications.</returns>
        /// <param name="receiverID">User identifier.</param>
        List<Notification> GetNotificationsSent(string senderID);

        List<Notification> GetNotificationsReceived(string receiverID);

        bool ExistsNotification(string senderID, string teamid);

        /// <summary>
        /// Adds the notification in the database
        /// </summary>
        /// <returns><c>true</c>, if notification was added, <c>false</c> otherwise.</returns>
        /// <param name="ntf">Ntf.</param>
        void AddNotification(Notification ntf);

        bool RemoveAllNotificationsForUser(string UserID);

        /// <summary>
        /// Removes the notification with the specified ID
        /// </summary>
        /// <returns><c>true</c>, if notification was removed, <c>false</c> otherwise.</returns>
        /// <param name="ntfID">Ntf identifier.</param>
        bool RemoveNotification(string ntfID);

        bool RemoveAllTeamNotifications(string TeamID);
    }
}
