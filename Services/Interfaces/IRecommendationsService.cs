using DatingApp.DB.Models.Recommendations;
using DatingApp.DTOs.Recommendations;

namespace DatingApp.Services.Interfaces
{
    public interface IRecommendationsService
    {
        IEnumerable<RecommendedUser> GetRecommendedUsers(FiltersDto filters);

        Task AddUserLikeAsync(string likingUserId, string likedUserId);
    }
}
