using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;

namespace DatingApp.Services.Interfaces
{
    public interface IRecommendationStrategy
    {
        Task<IEnumerable<Tuple<User, double>>> GetRecommendedUsersAsync(FiltersDto filters);
    }
}
