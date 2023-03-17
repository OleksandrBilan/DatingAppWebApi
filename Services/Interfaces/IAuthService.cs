using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Questionnaire;

namespace DatingApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task CreateAdminUsersIfDontExistAsync();

        Task CreateUserRolesIfDontExistAsync();

        Task<bool> RegisterAsync(User user, string password, IEnumerable<QuestionAnswerDto> questionsAnswers);

        Task<User> LoginAsync(string email, string password);

        Task<string> GenerateAccessTokenAsync(User user, DateTime expireDateTime);

        Task<bool> ConfirmEmailAsync(string userId);

        Task<IEnumerable<string>> GetUserRolesAsync(User user);

        Task<bool> CheckIfUserExistsAsync(string email);
    }
}
