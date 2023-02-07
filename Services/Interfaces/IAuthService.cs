using DatingApp.Models;

namespace DatingApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(User user, string password);

        Task<User> LoginAsync(string email, string password);

        Task<string> GenerateAccessTokenAsync(User user, DateTime expireDateTime);
    }
}
