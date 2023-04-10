using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Helpers;

namespace DatingApp.Services.Interfaces
{
    public interface IRecommendationsService
    {
        Task<IEnumerable<Tuple<User, double>>> GetRecommendedUsersAsync(RecommendationTypes recommendationType, FiltersDto filters);
    }
}
