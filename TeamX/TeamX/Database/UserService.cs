using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamX.Models;
using TeamX.Services;

namespace TeamX.Database
{
    public class UserService : IUserService
    {
        public bool DeleteUser(string id)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE Id = @id", conn);
                command.Parameters.Add(new SqlParameter("id",id));
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

        public string FindUserByEmail(string email)
        {

            try
            {
                string id;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Email=@email", conn);
                command.Parameters.Add(new SqlParameter("email", email));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Non esiste utente con la data email 
                    if(!reader.Read()) return null;

                    id = reader.GetString(0);
                }
                AzureDatabase.CloseConnection(conn);
                return id;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }

        }

        public string FindUserByName(string nickname)
        {

            try
            {
                string id;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE NickName=@name", conn);
                command.Parameters.Add(new SqlParameter("name", nickname));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Non esiste utente con il nome dato 
                    if (!reader.Read()) return null;

                    id = reader.GetString(0);
                }
                AzureDatabase.CloseConnection(conn);
                return id;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }

        }

        public User GetUser(string id)
        {

            try
            {
                User usr;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Id=@id", conn);
                command.Parameters.Add(new SqlParameter("id", id));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Non esiste utente con il dato id
                    if (!reader.Read()) return null;

                    usr = new User(reader.GetString(1),reader.GetString(2),reader.GetString(3));
                    usr.Id = reader.GetString(0);
                }
                AzureDatabase.CloseConnection(conn);
                return usr;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: "+ex.Message);
                return null;
            }

        }

        public IEnumerable<User> GetUsers()
        {

            try
            {
               
                List<User> users = new List<User>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Users ", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usr = new User(reader.GetString(1), reader.GetString(2), reader.GetString(3));
                        usr.Id = reader.GetString(0);
                        users.Add(usr);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return users;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }

        }

        public bool InsertUser(User u)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("INSERT INTO Users VALUES (@id, @email, @pwd, @nickname)", conn);
                command.Parameters.Add(new SqlParameter("id", u.Id));
                command.Parameters.Add(new SqlParameter("email", u.Email));
                command.Parameters.Add(new SqlParameter("pwd", u.Pwd));
                command.Parameters.Add(new SqlParameter("nickname", u.NickName));
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

        public bool UpdateUser(User u)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("UPDATE Users SET Email = @email, Pwd = @pwd, NickName = @nickname WHERE Id = @id", conn);
                command.Parameters.Add(new SqlParameter("id", u.Id));
                command.Parameters.Add(new SqlParameter("email", u.Email));
                command.Parameters.Add(new SqlParameter("pwd", u.Pwd));
                command.Parameters.Add(new SqlParameter("nickname", u.NickName));
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
