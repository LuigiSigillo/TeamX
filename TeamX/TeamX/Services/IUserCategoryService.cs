using System;
namespace TeamX.Services
{
    public interface IUserCategoryService
    {

        void AddDescription(string UserId, int CategoryId, string Description);

        string GetDescription(string UserId, int CategoryId);

    }
}
