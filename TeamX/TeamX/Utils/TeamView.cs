using System;
using System.Drawing;
using TeamX.Models;

namespace TeamX.Utils
{
    public class TeamView
    {
        public Team Team { get; set; }
        public string Category { get; set; }
        public string TeamName { get; set; }
        public string City { get; set; }
        public Color TextColor { get; set; }

        public TeamView(Team t, string category )
        {
            Team = t;

            TeamName = t.Name;
            City = t.City;


            if (t.NumOfMembers == t.MaxMembers) TextColor = Color.Silver;
            else TextColor = Color.FromArgb(36,47,56);

            Category = category;
        }

    }
}
