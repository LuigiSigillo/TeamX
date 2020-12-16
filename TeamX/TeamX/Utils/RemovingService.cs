using System;
using TeamX.Services;
using TeamX.Database;
using TeamX.Models;
namespace TeamX.Utils
{
    public static class RemovingService
    {
        private static INotificationService NS = new NotificationService();
        private static ITeamService TS = new TeamService();
        private static ITeamMemberService TMS = new TeamMemberService();
        private static ITeamCategoryService TCS = new TeamCategoryService();


        public static void RemoveUserFromTeam(string UserId, string TeamId)
        {
            TS.DecrementMembersNumber(TeamId);
            TMS.RemoveUser(TeamId, UserId);
            foreach (User tm in TMS.GetMembers(TeamId))
            {
                var ntf = new Notification(UserId, tm.Id, TeamId, 3);
                NS.AddNotification(ntf);
            }
        }

        public static void RemoveTeam(string TeamId)
        {
            var team = TS.GetTeam(TeamId);
            NS.RemoveAllTeamNotifications(TeamId);
            foreach (User tm in TMS.GetMembers(TeamId))
            {
                if (tm.Id != team.CreatorID)
                {
                    var ntf = new Notification(team.CreatorID, tm.Id, TeamId, 4, team.Name);
                    NS.AddNotification(ntf);
                }
            }

            TMS.RemoveAllUsersFromTeam(TeamId);
            TCS.RemoveAllCategoriesFromTeam(TeamId);
            TS.RemoveTeam(TeamId);
        }

        public static void RemoveUser(string UserId)
        {


            NS.RemoveAllNotificationsForUser(UserId);


            foreach (Team t in TMS.GetAdminTeams(UserId))
            {
                RemoveTeam(t.Id);
            }



            foreach (Team t in TMS.GetNonAdminTeams(UserId))
            {
                RemoveUserFromTeam(UserId, t.Id);
            }


            new UserService().DeleteUser(UserId);
            new PropertyService().DisableUser();
        }
    }
}
