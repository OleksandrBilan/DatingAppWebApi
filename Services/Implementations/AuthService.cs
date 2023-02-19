using DatingApp.DB;
using DatingApp.DB.Models;
using DatingApp.Services.Helpers;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DatingApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly EmailHelper _emailHelper;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, AppDbContext dbContext, EmailHelper emailHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _emailHelper = emailHelper;
        }

        public async Task<bool> ConfirmEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                if (user.EmailConfirmed)
                    return true;

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }

        public async Task<string> GenerateAccessTokenAsync(User user, DateTime expireDateTime)
        {
            var securityKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
            var claims = await _userManager.GetClaimsAsync(user);

            var token = new JwtSecurityToken(claims: claims,
                                             notBefore: DateTime.Now,
                                             expires: expireDateTime,
                                             signingCredentials: new SigningCredentials(
                                                                        new SymmetricSecurityKey(securityKey),
                                                                        SecurityAlgorithms.HmacSha256Signature));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                user.City = _dbContext.Cities.FirstOrDefault(c => c.Id == user.CityId);
                return user;
            }
            else
            {
                if (result.IsLockedOut)
                    throw new Exception("User is locked out");
                else
                    throw new ArgumentException("Can not log in the user");
            }
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var confirmationLink = $"{_configuration.GetValue<string>("EmailConfirmationLink")}/{user.Id}";

                await _emailHelper.SendMailAsync(
                    user.Email,
                    "Email Confirmation",
                    $"Please click on the link to confirm your email:\n {confirmationLink}");
            }
            
            return result.Succeeded;
        }
    }
}
