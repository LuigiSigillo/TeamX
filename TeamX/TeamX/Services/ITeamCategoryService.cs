using System;
using System.Collections.Generic;

namespace TeamX.Services
{
    public interface ITeamCategoryService
    {

        /// <summary>
        /// Adds the team in the category.
        /// </summary>
        /// <returns><c>true</c>, if team category was added, <c>false</c> otherwise.</returns>
        /// <param name="TeamID">Team identifier.</param>
        /// <param name="CategoryID">Category identifier.</param>
        void AddTeamCategory(string TeamID, int CategoryID);

        /// <summary>
        /// Removes the team from the category.
        /// </summary>
        /// <returns><c>true</c>, if team category was removed, <c>false</c> otherwise.</returns>
        /// <param name="TeamID">Team identifier.</param>
        /// <param name="CategoryID">Category identifier.</param>
        bool RemoveTeamCategory(string TeamID, int CategoryID);

        /// <summary>
        /// Removes the old TeamCategory and adds a new one
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="OldCategoryID"></param>
        /// <param name="NewCategoryID"></param>
        void UpdateTeamCategory(string TeamID, int OldCategoryID, int NewCategoryID);


        /// <summary>
        /// Gets all the categories for a team
        /// </summary>
        /// <returns>The categories.</returns>
        /// <param name="TeamID">Team identifier.</param>
        List<int> GetCategories(string TeamID);

        /// <summary>
        /// Gets all the teams for a category
        /// </summary>
        /// <returns>The teams.</returns>
        /// <param name="CategoryID">Category identifier.</param>
        List<int> GetTeams(int CategoryID);


        /// <returns><c>true</c>, if category the team is part of the category, <c>false</c> otherwise.</returns>
        /// <param name="TeamID">Team identifier.</param>
        /// <param name="CategoryID">Category identifier.</param>
        bool IsCategory(string TeamID, int CategoryID);

        /// <summary>
        /// Removes all TeamCategories where TeamID matches the passed TeamID.
        /// </summary>
        /// <param name="TeamID"></param>
        /// <returns></returns>
        bool RemoveAllCategoriesFromTeam(string TeamID);
    }
}
