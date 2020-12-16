using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TeamX.Models;
using TeamX.Services;
using Xamarin.Forms;

namespace TeamX.Database
{
    class TeamService : ITeamService
    {
        public Team GetTeam(string id)
        {
            Team team = new Team();
            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Teams WHERE Id=@id", conn);
                command.Parameters.Add(new SqlParameter("id", id));
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    team.Id = reader.GetString(0);
                    team.CreatorID = reader.GetString(1);
                    team.Name = reader.GetString(2);
                    team.MaxMembers = reader.GetInt32(3);
                    team.NumOfMembers = reader.GetInt32(4);
                    try { team.City = reader.GetString(5); }
                    catch
                    {
                        team.City = "";
                    }
                    team.Description = reader.GetString(6);
                    team.CreationDate = reader.GetDateTime(7);
                    team.Difficulty = reader.GetInt32(8);
                    team.TerminationDate = reader.GetDateTime(9);
                }
                AzureDatabase.CloseConnection(conn);
                return team;
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Teams ORDER BY CreationDate DESC", conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Team team = new Team();
                        team.Id = reader.GetString(0);
                        team.CreatorID = reader.GetString(1);
                        team.Name = reader.GetString(2);
                        team.MaxMembers = reader.GetInt32(3);
                        team.NumOfMembers = reader.GetInt32(4);
                        try { team.City = reader.GetString(5);}
                        catch
                        {
                            team.City = "";
                        }
                        team.Description = reader.GetString(6);
                        team.CreationDate = reader.GetDateTime(7);
                        team.Difficulty = reader.GetInt32(8);
                        team.TerminationDate = reader.GetDateTime(9);

                        teams.Add(team);
                    }
                }
                AzureDatabase.CloseConnection(conn);
                return teams;
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<Team> GetActiveTeams()
        {
            List<Team> teams = new List<Team>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Teams " +
                    "WHERE (NumOfMembers < MaxMembers) AND (TerminationDate>@today)" +
                    "ORDER BY CreationDate DESC", conn);
                command.Parameters.AddWithValue("today", DateTime.Today);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Team team = new Team();
                        team.Id = reader.GetString(0);
                        team.CreatorID = reader.GetString(1);
                        team.Name = reader.GetString(2);
                        team.MaxMembers = reader.GetInt32(3);
                        team.NumOfMembers = reader.GetInt32(4);
                        try { team.City = reader.GetString(5); }
                        catch
                        {
                            team.City = "";
                        }
                        team.Description = reader.GetString(6);
                        team.CreationDate = reader.GetDateTime(7);
                        team.Difficulty = reader.GetInt32(8);
                        team.TerminationDate = reader.GetDateTime(9);

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

        public IEnumerable<Team> GetInactiveTeams()
        {
            List<Team> teams = new List<Team>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Teams " +
                    "WHERE (NumOfMembers = MaxMembers) AND (TerminationDate<=@today) " +
                    "ORDER BY DateOfCreation DESC", conn);
                command.Parameters.AddWithValue("today", DateTime.Today);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Team team = new Team();
                        team.Id = reader.GetString(0);
                        team.CreatorID = reader.GetString(1);
                        team.Name = reader.GetString(2);
                        team.MaxMembers = reader.GetInt32(3);
                        team.NumOfMembers = reader.GetInt32(4);
                        try { team.City = reader.GetString(5); }
                        catch
                        {
                            team.City = "";
                        }
                        team.Description = reader.GetString(6);
                        team.CreationDate = reader.GetDateTime(7);
                        team.Difficulty = reader.GetInt32(8);
                        team.TerminationDate = reader.GetDateTime(9);

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

        public bool IncrementMembersNumber(string teamID)
        {
            try
            {
                using (var conn = AzureDatabase.GetConnection())
                {
                    string query = "UPDATE Teams " +
                        "SET NumOfMembers = NumOfMembers+1 " +
                        "WHERE Id=@id";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("id", teamID);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool DecrementMembersNumber(string teamID)
        {
            try
            {
                using (var conn = AzureDatabase.GetConnection())
                {
                    string query = "UPDATE Teams " +
                        "SET NumOfMembers = NumOfMembers-1 " +
                        "WHERE Id=@id";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("id", teamID);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool InsertTeam(Team team)
        {
            try
            {
                using (var conn = AzureDatabase.GetConnection())
                {
                    string query;

                    if(team.City==null)
                    {
                        query = "INSERT INTO Teams(Id,CreatorID,Name,MaxMembers,NumOfMembers,Description,CreationDate,Difficulty,TerminationDate) " +
                                "VALUES(@Id, @CreatorID, @Name, @MaxMembers, @NumOfMembers, @Description, @CreationDate,@Difficulty,@TerminationDate)";
                    } 
                    else
                    {
                        query = "INSERT INTO Teams(Id,CreatorID,Name,MaxMembers,NumOfMembers,Description,City,CreationDate,Difficulty,TerminationDate) " +
                        "VALUES(@Id, @CreatorID, @Name, @MaxMembers, @NumOfMembers, @Description,@City,@CreationDate,@Difficulty,@TerminationDate)";
                    }

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("Id", team.Id);
                    command.Parameters.AddWithValue("CreatorID", team.CreatorID);
                    command.Parameters.AddWithValue("Name", team.Name);
                    command.Parameters.AddWithValue("MaxMembers", team.MaxMembers);
                    command.Parameters.AddWithValue("NumOfMembers", team.NumOfMembers);
                    command.Parameters.AddWithValue("Description", team.Description);
                    command.Parameters.AddWithValue("CreationDate", team.CreationDate);
                    command.Parameters.AddWithValue("Difficulty", team.Difficulty);
                    command.Parameters.AddWithValue("TerminationDate", team.TerminationDate);

                    if (team.City != null)
                        command.Parameters.AddWithValue("City", team.City);

                    command.ExecuteNonQuery();

                    AzureDatabase.CloseConnection(conn);
                } 
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool RemoveTeam(string id)
        {
            try
            {
                using(var conn = AzureDatabase.GetConnection())
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Teams WHERE Id=@id", conn);
                    command.Parameters.AddWithValue("id", id);
                    MessagingCenter.Send(this,"CityToRemove",GetTeam(id).City);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }

        public bool UpdateTeam(Team team)
        {
            try
            {
                using(var conn = AzureDatabase.GetConnection())
                {
                    string query = "UPDATE Teams " +
                        "SET Name=@name, MaxMembers=@maxmembers, NumOfMembers=@numofmembers, " +
                        "City=@city, Description=@description, Difficulty=@difficulty, TerminationDate=@terminationdate " +
                        "WHERE Id=@id";
                    SqlCommand command = new SqlCommand(query,conn);
                    command.Parameters.AddWithValue("id", team.Id);
                    command.Parameters.AddWithValue("name", team.Name);
                    command.Parameters.AddWithValue("maxmembers", team.MaxMembers);
                    command.Parameters.AddWithValue("numofmembers", team.NumOfMembers);
                    command.Parameters.AddWithValue("city", team.City);
                    command.Parameters.AddWithValue("description", team.Description);
                    command.Parameters.AddWithValue("Difficulty", team.Difficulty);
                    command.Parameters.AddWithValue("TerminationDate", team.TerminationDate);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error accessing the database: " + ex.Message);
                return false;
            }
        }
    }
}
