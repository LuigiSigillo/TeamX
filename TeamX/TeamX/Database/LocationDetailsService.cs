using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TeamX.Services;
using Xamarin.Forms;

namespace TeamX.Database
{
    class LocationDetailsService : ILocationDetailsService
    {
        TeamService ts = new TeamService();
        public LocationDetailsService()
        {
            MessagingCenter.Subscribe<TeamService, string>(this, "CityToRemove",IstoRemove);
        }

        private void IstoRemove(TeamService arg1, string arg2)
        {

            if (!ts.GetTeams().Any(t => t.City.Equals(arg2)))
                RemoveLocationDetails(arg2);

        }

        public bool AddLocationDetails(string city, double latitude, double longitude)
        {
            using(var conn = AzureDatabase.GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO LocationDetails Values(@city,@lati,@longi)", conn);
                    command.Parameters.AddWithValue("city", city);
                    command.Parameters.AddWithValue("lati", latitude);
                    command.Parameters.AddWithValue("longi", longitude);

                    command.ExecuteNonQuery();
                    AzureDatabase.CloseConnection(conn);
                    MessagingCenter.Send(this,"NewDetail", (city,latitude,longitude));
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error accessing the database: " + ex.Message);
                    return false;
                }
            }
        }

        public (double, double) GetLocationDetails(string city)
        {
            double latitude=0, longitude=0;
                try
                {
                    var conn = AzureDatabase.GetConnection();
                    SqlCommand command = new SqlCommand("SELECT * FROM LocationDetails WHERE City=@city", conn);
                    command.Parameters.AddWithValue("city", city);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return (0,0);
                        latitude = reader.GetDouble(1);
                        longitude = reader.GetDouble(2);
                        
                    }
                    AzureDatabase.CloseConnection(conn);
                    return (latitude, longitude);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error accessing the database: " + ex.Message);
                    return (0,0);
                }  
        }

        public bool RemoveLocationDetails(string city)
        {
            using(var conn = AzureDatabase.GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM LocationDetails WHERE City=@city");
                    command.Parameters.AddWithValue("city", city);
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

        public Dictionary<string, Tuple<double, double>> GetDetails()
        {
            Dictionary<string, Tuple<double,double>> teams = new Dictionary<string, Tuple<double, double>>();

            try
            {
                var conn = AzureDatabase.GetConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM LocationDetails", conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        var c = reader.GetString(0);
                        Tuple<double, double> detail = new Tuple<double, double>(reader.GetDouble(1),reader.GetDouble(2));
                        teams.Add(c,detail);
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
    }
}
