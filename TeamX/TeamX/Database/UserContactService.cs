using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamX.Models;
using TeamX.Services;

namespace TeamX.Database
{
    public class UserContactService : IUserContactService
    {
        public bool AddContact(string userId, string contactUri, string type)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("INSERT INTO UserContacts VALUES (@userid, @uri, @type)", conn);
                command.Parameters.Add(new SqlParameter("userid", userId));
                command.Parameters.Add(new SqlParameter("uri", contactUri));
                command.Parameters.Add(new SqlParameter("type", type));
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

        public void DeleteAllContacts(string userId)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM UserContacts WHERE UserId = @id", conn);
                command.Parameters.Add(new SqlParameter("id", userId));
                command.ExecuteNonQuery();
                AzureDatabase.CloseConnection(conn);
                return;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return;
            }
        }

        public bool DeleteContact(string userId, string type)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM UserContacts WHERE UserId = @id AND Type = @type ", conn);
                command.Parameters.Add(new SqlParameter("id", userId));
                command.Parameters.Add(new SqlParameter("type", type));
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

        public UserContact GetContact(string userId, string type)
        {
            try
            {
                UserContact cntc;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM UserContacts WHERE UserId=@id AND Type = @type", conn);
                command.Parameters.Add(new SqlParameter("id", userId));
                command.Parameters.Add(new SqlParameter("type", type));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Non esiste utente con il dato id
                    if (!reader.Read()) return null;

                    cntc = new UserContact(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                }
                AzureDatabase.CloseConnection(conn);
                return cntc;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public List<UserContact> GetContacts(string userId)
        {
            try
            {

                List<UserContact> cntcs = new List<UserContact>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM UserContacts WHERE UserId = @userid ", conn);
                command.Parameters.Add(new SqlParameter("userid", userId));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cntc = new UserContact(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                        cntcs.Add(cntc);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return cntcs;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public bool ModifyContact(string userId, string type, string newUri)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command;
                if (GetContact(userId, type) != null)
                    command = new SqlCommand("UPDATE UserContacts SET Uri = @uri WHERE UserId = @id AND Type = @type ", conn);
                else 
                    command = new SqlCommand("INSERT INTO UserContacts (UserId, Uri, Type) VALUES (@id, @uri, @type)", conn);

                command.Parameters.Add(new SqlParameter("id", userId));
                command.Parameters.Add(new SqlParameter("uri", newUri));
                command.Parameters.Add(new SqlParameter("type", type));
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
