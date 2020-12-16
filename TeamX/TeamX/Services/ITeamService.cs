using System;
using System.Collections.Generic;
using System.Text;
using TeamX.Models;

namespace TeamX.Services
{
    public interface ITeamService
    {
        /// <summary>
        /// Gets all teams in the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<Team> GetTeams();
        ///
        ///
        ///
        IEnumerable<Team> GetActiveTeams();

        IEnumerable<Team> GetInactiveTeams();
        ///<summary>
        ///Gets the team with the ID==id
        ///</summary>
        ///<returns><see langword="null"/> if the team doest't exists</returns>
        Team GetTeam(string id);

        /// <summary>
        /// Insert a new team to the database
        /// </summary>
        /// <param name="team"></param>
        /// <returns>true if the operetions has concluded without errors</returns>
        bool InsertTeam(Team team);

        /// <summary>
        /// Updates team information
        /// </summary>
        /// <param name="team"></param>
        /// <returns>true if the update has been successfull</returns>
        bool UpdateTeam(Team team);

        /// <summary>
        /// Removes from the database the team with ID==id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the operation has been successfull</returns>
        bool RemoveTeam(string id);

        bool IncrementMembersNumber(string teamID);

        bool DecrementMembersNumber(string teamID);
    }
}
