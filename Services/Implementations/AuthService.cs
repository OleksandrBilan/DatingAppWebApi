using DatingApp.DB.Models;
using DatingApp.DTOs;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                if (result.IsLockedOut)
                    throw new Exception("User is locked out");
            }

            return false;
        }

        public async Task<bool> Register(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
    }
}
