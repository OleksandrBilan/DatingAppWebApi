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
        private readonly string _usersImagesFolderPath;

        public UserService(UserManager<User> userManager, AppDbContext dbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _dbContext = dbContext;

            _usersImagesFolderPath = configuration.GetValue<string>("UsersImagesFolderPath");
            if (string.IsNullOrWhiteSpace(_usersImagesFolderPath))
            {
                _usersImagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UsersImages");
                Directory.CreateDirectory(_usersImagesFolderPath);
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                var userAnswers = await _dbContext.UsersQuestionsAnswers.Where(x => x.UserId == userId).ToListAsync();
                _dbContext.UsersQuestionsAnswers.RemoveRange(userAnswers);

                var userLikes = await _dbContext.UsersLikes.Where(x => x.LikingUserId == userId || x.LikedUserId == userId).ToListAsync();
                _dbContext.UsersLikes.RemoveRange(userLikes);

                await _userManager.DeleteAsync(user);
                await _dbContext.SaveChangesAsync();
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

        public async Task UploadUserImageAsync(string userId, IFormFile image)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            string folderPath = Path.Combine(_usersImagesFolderPath, userId);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, image.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(fileStream);
        }

        private static string GetImageContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }

        public async Task<Image> GetUserImageAsync(string userId)
        {
            string userFolderPath = Path.Combine(_usersImagesFolderPath, userId);
            if (Directory.Exists(userFolderPath))
            {
                var files = Directory.GetFiles(userFolderPath);
                if (files.Any())
                {
                    var filePath = files[0];
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    var fileContentType = GetImageContentType(filePath);
                    return new Image { Bytes = fileBytes, ContentType = fileContentType };
                }
            }
            return null;
        }

        public void DeleteUserImage(string userId)
        {
            string userFolderPath = Path.Combine(_usersImagesFolderPath, userId);
            if (Directory.Exists(userFolderPath))
            {
                Directory.Delete(userFolderPath, true);
            }
        }
    }
}
