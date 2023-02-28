using DatingApp.DB.Models.UserRelated;

namespace DatingApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task CreateAdminUsersIfDontExist();

        Task CreateUserRolesIfDontExist();

        Task<bool> RegisterAsync(User user, string password);

        Task<User> LoginAsync(string email, string password);

        Task<string> GenerateAccessTokenAsync(User user, DateTime expireDateTime);

        Task<bool> ConfirmEmailAsync(string userId);
    }
}
