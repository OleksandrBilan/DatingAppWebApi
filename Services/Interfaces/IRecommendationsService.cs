using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;

namespace DatingApp.Services.Interfaces
{
    public interface IRecommendationsService
    {
        Task<IEnumerable<User>> GetRecommendedUsersByFiltersAsync(FiltersDto filters);
    }
}
