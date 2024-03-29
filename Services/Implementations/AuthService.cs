﻿using DatingApp.DB;
using DatingApp.DB.Models.Questionnaire;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Questionnaire;
using DatingApp.Services.Helpers;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly EmailHelper _emailHelper;
        private readonly IUserService _userService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, 
                           IConfiguration configuration, AppDbContext dbContext, EmailHelper emailHelper, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _emailHelper = emailHelper;
            _userService = userService;
        }

        #region Roles and admin users creation

        public async Task CreateUserRolesIfDontExistAsync()
        {
            var userRoles = new string[] { "Admin", "User", "VIP" };
            foreach (var role in userRoles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private class Credentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public async Task CreateAdminUsersIfDontExistAsync()
        {
            var credentials = _configuration.GetSection("AdminUsers").Get<IEnumerable<Credentials>>();
            if (credentials is not null)
            {
                foreach (var adminCredentials in credentials)
                {
                    var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminCredentials.Email);
                    if (existingUser is null)
                    {
                        var adminUser = new User
                        {
                            Email = adminCredentials.Email,
                            UserName = adminCredentials.Email,
                            BirthDate = DateTime.Now,
                            CountryCode = "XX",
                            CityId = 1,
                            Description = "Administrator",
                            Name = adminCredentials.Email,
                            SexId = 1,
                            EmailConfirmed = true
                        };

                        var result = await _userManager.CreateAsync(adminUser, adminCredentials.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(adminUser, "Admin");
                        }
                    }
                }
            }
        }

        #endregion

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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
                await _userService.PopulateUserInfo(user);

                var subscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(s => s.UserId == user.Id);
                if (subscription is not null)
                {
                    if (subscription.ExpireDateTime < DateTime.Now)
                    {
                        _dbContext.UsersSubscriptions.Remove(subscription);
                        await _userManager.RemoveFromRoleAsync(user, "VIP");
                        await _dbContext.SaveChangesAsync();
                    }
                }

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

        public async Task<string> RegisterAsync(User user, string password, IEnumerable<QuestionAnswerDto> questionsAnswers)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Code == user.Country.Code);
            if (country is null)
                _dbContext.Countries.Add(user.Country);
            else
                user.Country = country;

            var city = await _dbContext.Cities.FirstOrDefaultAsync(c => c.Name == user.City.Name && c.CountryCode == user.City.CountryCode);
            if (city is null)
                _dbContext.Cities.Add(user.City);
            else
                user.City = city;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (questionsAnswers is not null && questionsAnswers.Any())
                {
                    var userQuestionsAnswers = questionsAnswers.Select(x => new UserQuestionAnswer { UserId = user.Id, QuestionId = x.QuestionId, AnswerId = x.AnswerId });
                    await _dbContext.UsersQuestionsAnswers.AddRangeAsync(userQuestionsAnswers);
                }

                await _userManager.AddToRoleAsync(user, "User");

                var confirmationLink = $"{_configuration.GetValue<string>("EmailConfirmationLink")}/{user.Id}";

                await _emailHelper.SendMailAsync(
                    user.Email,
                    "Email Confirmation",
                    $"Please click on the link to confirm your email:\n {confirmationLink}");

                await _dbContext.SaveChangesAsync();
                return user.Id;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckIfUserExistsAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user is not null;
        }
    }
}
