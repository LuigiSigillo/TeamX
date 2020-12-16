using System;
using System.Collections.Generic;
using TeamX.Models;

namespace TeamX.Services
{
    public interface ITeamMemberService
    {
        List<User> GetMembers(string TeamID);

        List<Team> GetTeams(string UserID);

        List<Team> GetAdminTeams(string UserID);

        List<Team> GetNonAdminTeams(string UserID);

        bool AddUser(string TeamID, string UserID);

        bool AddAdmin(string TeamID, string UserID);

        bool RemoveAllTeamsForUser(string UserID);

        bool RemoveUser(string TeamID, string UserID);

        bool IsTeamMember(string TeamID, string UserID);

        bool RemoveAllUsersFromTeam(string TeamID);

        int GetRequests(string TeamID, string UserID);

        bool AddRequest(string TeamID, string UserID);
    }
}
