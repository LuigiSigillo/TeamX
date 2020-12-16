using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TeamX.Database;
using TeamX.Services;
using Xamarin.Forms;

namespace TeamX.Models
{
    public class SearchService
    {
        ITeamService teamserv = new TeamService();
        ITeamCategoryService TCS = new TeamCategoryService();
        ILocationDetailsService LS = new LocationDetailsService();
        public bool n, l, c, d,dif = false; //name location category distance
        public string location, name;
        public int category,difficulty;
        public double distance;
        private Dictionary<string,Tuple<double,double>> details;
        Position position;
        
        public SearchService()
        {
            MessagingCenter.Subscribe<LocationDetailsService, (string,double,double)>(this, "NewDetail", NewDetail);
            details = LS.GetDetails();
        }

        private void NewDetail(LocationDetailsService arg1, (string c, double lat, double longi) arg2)
        {
            details.Add(arg2.c,new Tuple<double,double>(arg2.lat,arg2.longi));
        }

        async void WhereAreU()
        {
            var locator = CrossGeolocator.Current;
            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Metodo di filtraggio per nome e eventualemten con altri filtri 
        /// 
        ///
        /// </summary>
        /// <param name="filterbyname"></param>
        /// <returns></returns>

        public IEnumerable<Team> GetTeams()
        {
            var filtratiper_nome = NameFiltered(teamserv.GetActiveTeams());
            var filtratiper_nome_loc = LocationFiltered(filtratiper_nome);
            var filtratiper_nome_loc_cat = CategoryFiltered(filtratiper_nome_loc);
            var filtratiper_nome_loc_cat_dis = DistanceFiltered(filtratiper_nome_loc_cat);
            var filtratiper_nome_loc_cat_dis_diff = DifficultyFiltered(filtratiper_nome_loc_cat_dis);
            return filtratiper_nome_loc_cat_dis_diff;
        }



        public List<string> StringTeams(string filter = null)
        {
            List<string> lista = new List<string>();
            foreach (Team t in GetTeams())
            {
                lista.Add(t.Name);
            }
            return lista;
        }

        public IEnumerable<Team> Add_team(Team t)
        {
            teamserv.InsertTeam(t);
            return this.GetTeams();

        }

        public IEnumerable<Team> Update_team(Team t)
        {
            teamserv.UpdateTeam(t);
            return this.GetTeams();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private IEnumerable<Team> NameFiltered(IEnumerable<Team> team)
        {
            if (n)
                return team.Where(t => t.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));

            return team;

        }

        public void FilterByName(string na)
        {
            if (!String.IsNullOrWhiteSpace(na))
            {
                n = true;
                name = na;
            }
            else n = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private IEnumerable<Team> LocationFiltered(IEnumerable<Team> team)
        {
            if (l)
                return team.Where(t => t.City.StartsWith(location, StringComparison.CurrentCultureIgnoreCase));
            return team;
        }

        public void FilterByLocation(string loc)
        {
            if (!String.IsNullOrWhiteSpace(loc))
            {
                l = true;
                location = loc;
            }
            else l = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private IEnumerable<Team> CategoryFiltered(IEnumerable<Team> team)
        {
            if (c)
                return team.Where(t => TCS.IsCategory(t.Id, category));
            return team;
        }

        public void FilterByCategory(int cat)
        {

            if (cat > 0)
            {
                category = cat;
                c = true;
            }
            else c = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private IEnumerable<Team> DifficultyFiltered(IEnumerable<Team> team)
        {
            if (dif)
            {
                foreach(Team t in team )
                {
                    Debug.WriteLine(t.Name+t.Difficulty);
                }
                return team.Where(t => t.Difficulty == difficulty);
            }
            return team;
        }

        public void FilterByDifficulty(int diff)
        {

            if (diff > 0)
            {
                difficulty = diff;
                dif = true;
            }
            else dif = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private IEnumerable<Team> DistanceFiltered(IEnumerable<Team> team)
        {
            WhereAreU();
            if (position != null)
            {
                if (d)
                {
                    var x = team.Where(t => details.ContainsKey(t.City));
                    foreach (Team t in x)
                        Debug.WriteLine(t.Name + t.City);
                    return x.Where(t => GetDistance(details[t.City].Item1, details[t.City].Item2) <= distance);//lat long
                }  
            }
            return team;
        }

    

        public void FilterByDistance(double dist)
        {
            if (dist > 1)
            {
                distance = dist;
                d = true;
            }
            else d = false;
        }
       
        /// <summary>
        /// ho usato questa formula https://en.wikipedia.org/wiki/Great-circle_distance
        /// ritorna la distanza in km
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>

        private double GetDistance(double lat2, double lon2)
        {
            var p = 0.017453292519943295;    // Math.PI / 180

            var a = 0.5 - Math.Cos((lat2 - position.Latitude) * p) / 2 +
                    Math.Cos(position.Latitude * p) * Math.Cos(lat2 * p) *
                    (1 - Math.Cos((lon2 - position.Longitude) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
        
      
            
        
    }
}
