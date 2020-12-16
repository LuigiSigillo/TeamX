using System;
using System.Collections.Generic;
using System.Text;
using TeamX.Services;
using System.Data.SqlClient;

namespace TeamX.Database
{
    class TeamCategoryService : ITeamCategoryService
    {
        public void AddTeamCategory(string TeamID, int CategoryID)
        {
            try
            {
                using (var conn = AzureDatabase.GetConnection())
                {
                    string query =
                        "INSERT INTO TeamCategories " +
                        "VALUES(@teamid, @categoryid)";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("teamid", TeamID);
                    command.Parameters.AddWithValue("categoryid", CategoryID);

                    command.ExecuteNonQuery();
                    AzureDatabase.CloseConnection(conn);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
            }
        }

        public List<int> GetCategories(string TeamID)
        {
            List<int> categories = new List<int>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                string query = "SELECT categoryID FROM TeamCategories WHERE teamID=@teamid";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("teamid", TeamID);

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int category = reader.GetInt32(0);
                        categories.Add(category);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return categories;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public List<int> GetTeams(int CategoryID)
        {
            List<int> teams = new List<int>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                string query = "SELECT teamID FROM TeamCategories WHERE categoryID=@catid";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("catid", CategoryID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int team = reader.GetInt32(0);
                        teams.Add(team);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return teams;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public bool IsCategory(string TeamID, int CategoryID)
        {
            try
            {
                using(var conn = AzureDatabase.GetConnection())
                {
                    SqlCommand command = new SqlCommand(
                        "SELECT COUNT(1) FROM TeamCategories WHERE (teamID=@teamid AND categoryID=@catid)",
                        conn);
                    command.Parameters.AddWithValue("teamid", TeamID);
                    command.Parameters.AddWithValue("catid", CategoryID);

                    int count = (int) command.ExecuteScalar();
                    AzureDatabase.CloseConnection(conn);

                    if (count == 1) return true;
                    return false;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool RemoveTeamCategory(string TeamID, int CategoryID)
        {
            try
            {
                using(var conn = AzureDatabase.GetConnection())
                {
                    SqlCommand command = new SqlCommand(
                        "DELETE FROM TeamCategories WHERE (teamID=@teamid AND categoryID=@catid)", conn);
                    command.Parameters.AddWithValue("teamid", TeamID);
                    command.Parameters.AddWithValue("catid", CategoryID);

                    int result = command.ExecuteNonQuery();
                    AzureDatabase.CloseConnection(conn);

                    if (result == 1) return true;
                    return false;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public void UpdateTeamCategory(string TeamID, int OldCategoryID, int NewCategoryID)
        {
            try
            {
                using (var conn = AzureDatabase.GetConnection())
                {
                    string query = "UPDATE TeamCategories " +
                        "SET categoryID=@newcat " +
                        "WHERE teamID=@teamid AND categoryID=@oldcat";
                    SqlCommand command = new SqlCommand(query,conn);
                    command.Parameters.AddWithValue("oldcat", OldCategoryID);
                    command.Parameters.AddWithValue("newcat", NewCategoryID);
                    command.Parameters.AddWithValue("teamid", TeamID);

                    command.ExecuteNonQuery();
                    AzureDatabase.CloseConnection(conn);
                   
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                
            }
        }

        public bool RemoveAllCategoriesFromTeam(string TeamID)
        {
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM TeamCategories WHERE TeamID=@TeamID", conn);
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
