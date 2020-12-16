using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamX.Models;
using TeamX.Services;

namespace TeamX.Database
{
    public class TeamMemberService : ITeamMemberService
    {
        private ITeamService TS = new TeamService();
        private IUserService US = new UserService();

        public bool AddUser(string TeamID, string UserID)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                string query = "UPDATE TeamMembers " +
                            "SET IsMember = 1 " +
                            "WHERE TeamID=@teamid AND UserID=@userid";
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("teamid", TeamID);
                command.Parameters.AddWithValue("userid", UserID);
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

        public List<Team> GetAdminTeams(string UserID)
        {
            try
            {

                List<Team> teams = new List<Team>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE UserId = @id ", conn);
                command.Parameters.Add(new SqlParameter("id", UserID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var team = TS.GetTeam(reader.GetString(0));
                        if (team.CreatorID == UserID)
                        { teams.Add(team); }
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

        public List<User> GetMembers(string TeamID)
        {
            try
            {

                List<User> users = new List<User>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE TeamId = @id AND IsMember = 1 ", conn);
                command.Parameters.Add(new SqlParameter("id", TeamID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = US.GetUser(reader.GetString(1));
                        { users.Add(user); }
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

        public List<Team> GetNonAdminTeams(string UserID)
        {
            try
            {

                List<Team> teams = new List<Team>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE UserId = @id AND IsMember = 1 ", conn);
                command.Parameters.Add(new SqlParameter("id", UserID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var team = TS.GetTeam(reader.GetString(0));
                        if (team.CreatorID != UserID)
                        { teams.Add(team); }
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

        public List<Team> GetTeams(string UserID)
        {
            try
            {

                List<Team> teams = new List<Team>();
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE UserId = @id AND IsMember = 1 ", conn);
                command.Parameters.Add(new SqlParameter("id", UserID));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var team = TS.GetTeam(reader.GetString(0));
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

        public bool IsTeamMember(string TeamID, string UserID)
        {
            try
            {
                bool value;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE TeamId = @teamid AND UserId = @userid AND IsMember = 1", conn);
                command.Parameters.Add(new SqlParameter("teamid", TeamID));
                command.Parameters.Add(new SqlParameter("userid", UserID));
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

        public bool RemoveUser(string TeamID, string UserID)
        {
            try
            {

                var conn = AzureDatabase.GetConnection();
                string query = "UPDATE TeamMembers " +
                            "SET IsMember = 0 " +
                            "WHERE TeamID=@teamid AND UserID=@userid";
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("teamid", TeamID);
                command.Parameters.AddWithValue("userid", UserID);
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

        public bool RemoveAllTeamsForUser(string UserID)
        {
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM TeamMembers WHERE UserID = @UserId", conn);
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

        public bool RemoveAllUsersFromTeam(string TeamID)
        {
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("DELETE FROM TeamMembers WHERE TeamID = @TeamID", conn);
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

        public int GetRequests(string TeamID, string UserID)
        {
            try
            {
                int value = 0;
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM TeamMembers WHERE TeamId = @teamid AND UserId = @userid ", conn);
                command.Parameters.Add(new SqlParameter("teamid", TeamID));
                command.Parameters.Add(new SqlParameter("userid", UserID));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        value = reader.GetInt32(3);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return value;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return 0;
            }
        }

        public bool AddRequest(string TeamID, string UserID)
        {
            try
            {

                int requests = GetRequests(TeamID, UserID);

                using (var conn = AzureDatabase.GetConnection())
                {
                    SqlCommand command;

                    if (requests == 0)
                    {
                        requests = requests + 1;
                        command = new SqlCommand("INSERT INTO TeamMembers VALUES (@teamid, @userid, 0, @requests)", conn);
                        command.Parameters.Add(new SqlParameter("teamid", TeamID));
                        command.Parameters.Add(new SqlParameter("userid", UserID));
                        command.Parameters.Add(new SqlParameter("requests", requests));
                        command.ExecuteNonQuery();
                        return true;
                    }

                    else
                    {

                        string query = "UPDATE TeamMembers " +
                            "SET Requests = Requests+1 " +
                            "WHERE TeamID=@teamid AND UserID=@userid";
                        command = new SqlCommand(query, conn);

                        command.Parameters.AddWithValue("teamid", TeamID);
                        command.Parameters.AddWithValue("userid", UserID);

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool AddAdmin(string TeamID, string UserID)
        {

            try
            {
                var conn = AzureDatabase.GetConnection();
                int requests = 0;
                SqlCommand command = new SqlCommand("INSERT INTO TeamMembers VALUES (@teamid, @userid, @ismember, @requests)", conn);
                command.Parameters.Add(new SqlParameter("teamid", TeamID));
                command.Parameters.Add(new SqlParameter("userid", UserID));
                command.Parameters.Add(new SqlParameter("ismember", 1));
                command.Parameters.Add(new SqlParameter("requests", requests));
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
