using System;
using System.Data.SqlClient;
using TeamX.Services;

namespace TeamX.Database
{
    public class UserCategoryService : IUserCategoryService
    {
        public void AddDescription(string UserId, int CategoryId, string Description)
        {

            try
            {

                var conn = AzureDatabase.GetConnection();
                SqlCommand command;
                if (HasDescription(UserId, CategoryId))
                {
                    command = new SqlCommand("UPDATE UserCategories SET Description = @desk WHERE UserId = @user AND CategoryId = @cat", conn);

                }

                else
                {
                    command = new SqlCommand("INSERT INTO UserCategories VALUES (@user, @cat, @desk)", conn);
                }


                command.Parameters.Add(new SqlParameter("user", UserId));
                command.Parameters.Add(new SqlParameter("cat", CategoryId));
                command.Parameters.Add(new SqlParameter("desk", Description));


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

        private bool HasDescription(string UserId, int CategoryId)
        {
            try
            {
                bool value = false;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM UserCategories WHERE UserId = @user AND CategoryId = @cat ", conn);
                command.Parameters.Add(new SqlParameter("user", UserId));
                command.Parameters.Add(new SqlParameter("cat", CategoryId));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    value = reader.Read();
                }
                AzureDatabase.CloseConnection(conn);
                return value;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public string GetDescription(string UserId, int CategoryId)
        {
            try
            {
                string desk = null;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM UserCategories WHERE UserId = @user AND Category = @cat ", conn);
                command.Parameters.Add(new SqlParameter("user", UserId));
                command.Parameters.Add(new SqlParameter("cat", CategoryId));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        desk = reader.GetString(2);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return desk;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }
    }
}
