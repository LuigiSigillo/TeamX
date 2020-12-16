using System;
using System.Collections.Generic;
using System.Text;

namespace TeamX.Services
{
    public interface ILocationDetailsService
    {
        bool AddLocationDetails(string city, double latitude, double longitude);

        /// <summary>
        /// Gets from the database Latitude e Longitude of a specific city.
        /// </summary>
        /// <param name="city"></param>
        /// <returns>(latitude,longitude)</returns>
        (double, double) GetLocationDetails(string city);

        bool RemoveLocationDetails(string city);
        Dictionary<string, Tuple<double,double>> GetDetails();
    }
}
