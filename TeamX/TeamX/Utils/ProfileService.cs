using System;
using System.Collections.Generic;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;

namespace TeamX.Utils
{
    public class ProfileService
    {
        private string UserID;
        private ITeamMemberService TMS;
        private ITeamCategoryService TCS;



        public ProfileService(string userID)
        {
            UserID = userID;
            TMS = new TeamMemberService();
            TCS = new TeamCategoryService();

        }
        /// <summary>
        /// Gets the expertise of a user in a team. 
        /// The expertise is calculated as the sum of the times you were part of 
        /// a team which has that category.
        /// </summary>
        /// <returns>The expertise.</returns>
        /// <param name="categoryID">Category identifier.</param>
        private int GetExpertise(int categoryID)
        {
            //Prendo i team di cui l'utente ha fatto parte e li filtro per categoria
            var teams = TMS.GetTeams(UserID).FindAll((obj) => TCS.IsCategory(obj.Id,categoryID)) ;
            return teams.Count; 
        }

        /// <summary>
        /// Gets the complete list of experiences of the user 
        /// </summary>
        /// <returns>The experiences.</returns>
        public List<Experience> GetExperiences() 
        {
            var ctgs = CategoryService.GetMaxCategory();
            List<Experience> exps = new List<Experience>();
            for(int id = 1; id<=ctgs; id++)
            {
                var exp = new Experience(GetExpertise(id),id,CategoryService.GetCategory(id));
                exps.Add(exp);
            }
            return exps;
        }

        //Quante squadre sta seguendo
        public int GetFollowing()
        {
            var teams =  TMS.GetNonAdminTeams(UserID);
            if (teams == null) return 0;
            return teams.Count;
        }

        //Quante sue squadre sono seguite
        public int GetFollowed()
        {
            var teams = TMS.GetAdminTeams(UserID).FindAll((Team obj) => obj.NumOfMembers > 1);
            if (teams == null) return 0;
            return teams.Count;
        }


    }
}
