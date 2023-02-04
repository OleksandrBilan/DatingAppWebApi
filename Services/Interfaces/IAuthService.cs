using DatingApp.DB.Models;
using DatingApp.DTOs;

namespace DatingApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(User user, string password);

        Task<bool> LoginAsync(string email, string password);
    }
}
