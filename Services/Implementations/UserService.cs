using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.User;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        public UserService(UserManager<User> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await PopulateUserInfo(user);
            return user;
        }

        public async Task PopulateUserInfo(User user)
        {
            if (user is null)
                return;

            user.Country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Code == user.CountryCode);
            user.City = await _dbContext.Cities.FirstOrDefaultAsync(c => c.Id == user.CityId);
        }

        public async Task UpdateUserInfoAsync(UpdateUserInfoDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user is not null)
            {
                user.Name = dto.Name;
                user.Description = dto.Description;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new ArgumentException("No user with such id");
            }
        }
    }
}
