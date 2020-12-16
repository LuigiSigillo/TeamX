using System;
using TeamX.Utils;

namespace TeamX.Models
{
    /// <summary>
    /// This class represents the relationship between a team and a category
    /// </summary>
    public class TeamCategory
    {
        public string TeamID { get; set; }
        public int CategoryID { get; set; }

        public TeamCategory(string teamID, int categoryID)
        {
            TeamID = teamID;
            CategoryID = categoryID;
        }
    }
}
