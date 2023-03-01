using DatingApp.DB.Models.UserRelated;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task CreateAdminUsersIfDontExistAsync();

        Task CreateUserRolesIfDontExistAsync();

        Task<bool> RegisterAsync(User user, string password);

        Task<User> LoginAsync(string email, string password);

        Task<string> GenerateAccessTokenAsync(User user, DateTime expireDateTime);

        Task<bool> ConfirmEmailAsync(string userId);

        Task<IEnumerable<IdentityRole>> GetUserRolesAsync(User user);
    }
}
