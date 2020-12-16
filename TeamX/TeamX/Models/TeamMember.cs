using System;
using TeamX.Utils;

namespace TeamX.Models
{
    public class TeamMember
    {

        public string TeamID { get; set; }
        public string UserID { get; set; }
        public bool IsMember { get; set; }
        public int Requests { get; set; }

        public TeamMember(string teamId, string userId, bool isMember, int requests)
        {

            TeamID = teamId;
            UserID = userId;
            IsMember = isMember;
            Requests = requests;
        }
    }
}
