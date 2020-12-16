using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamX.Models;
using TeamX.Services;

namespace TeamX.Database
{
    public class NotificationService : INotificationService
    {
        public void AddNotification(Notification ntf)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command;
                if (ntf.Message != null) {
                    command = new SqlCommand("INSERT INTO Notifications " + "(id,senderid,receiverid,teamid,type,message) "+
                        "VALUES (@id, @senderid, @receiverid, @teamid, @type, @message)", conn);
                    command.Parameters.Add(new SqlParameter("message", ntf.Message));
                    command.Parameters.Add(new SqlParameter("teamid", ntf.TeamID));
                }
                else if (ntf.TeamID != null)
                {
                    command = new SqlCommand("INSERT INTO Notifications " + "(id,senderid,receiverid,teamid,type) " +
                        " VALUES (@id, @senderid, @receiverid, @teamid, @type)", conn);
                    command.Parameters.Add(new SqlParameter("teamid", ntf.TeamID));
                }
                else
                {
                    command = new SqlCommand("INSERT INTO Notifications " + "(id,senderid,receiverid,type)"+
                        " VALUES (@id, @senderid, @receiverid, @type)", conn);

                }
                command.Parameters.Add(new SqlParameter("id", ntf.Id));
                command.Parameters.Add(new SqlParameter("senderid", ntf.SenderID));
                command.Parameters.Add(new SqlParameter("receiverid", ntf.ReceiverID));
                command.Parameters.Add(new SqlParameter("type", ntf.Type));
                command.ExecuteNonQuery();
                AzureDatabase.CloseConnection(conn);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
            }
        }

        public bool ExistsNotification(string senderID, string teamid)
        {
            var ntfs = GetNotificationsSent(senderID);
            return ntfs.Exists((obj) => string.Equals(obj.TeamID, teamid));
        }

        public List<Notification> GetNotificationsSent(string senderID)
        {
            try
            {

                List<Notification> ntfs = new List<Notification>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Notifications WHERE SenderID = @senderid ", conn);
                command.Parameters.Add(new SqlParameter("senderid", senderID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string teamId;
                        string message;
                        try
                        {
                            teamId = reader.GetString(3);
                        }
                        catch(Exception e)
                        {
                            teamId = null;
                        }
                        try
                        {
                            message = reader.GetString(5);
                        }
                        catch(Exception e)
                        {
                            message = null;
                        }
                        var ntf = new Notification(reader.GetString(1), reader.GetString(2), teamId, reader.GetInt32(4), message);
                        ntf.Id = reader.GetString(0);
                        ntfs.Add(ntf);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return ntfs;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public List<Notification> GetNotificationsReceived(string receiverID)
        {
            try
            {

                List<Notification> ntfs = new List<Notification>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Notifications WHERE ReceiverID = @receiverID ", conn);
                command.Parameters.Add(new SqlParameter("receiverID", receiverID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string teamId;
                        string message;
                        try
                        {
                            teamId = reader.GetString(3);
                        }
                        catch(Exception e)
                        {
                            teamId = null;
                        }
                        try
                        {
                            message = reader.GetString(5);
                        }
                        catch(Exception e)
                        {
                            message = null;
                        }
                        var ntf = new Notification(reader.GetString(1), reader.GetString(2), teamId, reader.GetInt32(4), message);
                        ntf.Id = reader.GetString(0); ntfs.Add(ntf);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return ntfs;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }


        public bool RemoveAllNotificationsForUser (string UserID)
        {
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM Notifications WHERE SenderID=@UserID OR ReceiverID=@UserID", conn);
                command.Parameters.AddWithValue("UserID", UserID);
                command.ExecuteNonQuery();
                AzureDatabase.CloseConnection(conn);
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool RemoveNotification(string ntfID)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM Notifications WHERE Id = @id ", conn);
                command.Parameters.Add(new SqlParameter("id", ntfID));
                command.ExecuteNonQuery();
                AzureDatabase.CloseConnection(conn);
                return true;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool RemoveAllTeamNotifications(string TeamID)
        {
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM Notifications WHERE TeamID=@TeamID", conn);
                command.Parameters.AddWithValue("TeamID", TeamID);
                command.ExecuteNonQuery();
                AzureDatabase.CloseConnection(conn);
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }
    }
}
