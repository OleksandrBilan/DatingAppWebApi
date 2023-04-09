using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.User;

namespace DatingApp.Services.Interfaces
{
    public interface IUserService
    {
        public Task DeleteUserAsync(string userId);

        public Task UpdateUserInfoAsync(UpdateUserInfoDto dto);

        public Task<User> GetUserByIdAsync(string id);

        public Task PopulateUserInfo(User user);
    }
}
