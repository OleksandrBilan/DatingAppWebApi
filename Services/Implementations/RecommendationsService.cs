using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Helpers;
using DatingApp.Services.Helpers.Recommendations;
using DatingApp.Services.Interfaces;

namespace DatingApp.Services.Implementations
{
    public class RecommendationsService : IRecommendationsService
    {
        private readonly Dictionary<RecommendationTypes, IRecommendationStrategy> _recommendationStrategies;

        public RecommendationsService(AppDbContext dbContext)
        {
            _recommendationStrategies = new Dictionary<RecommendationTypes, IRecommendationStrategy>
            {
                { RecommendationTypes.FiltersRecommendation, new FiltersRecommendationStrategy(dbContext) }
            };
        }

        public async Task<IEnumerable<Tuple<User, double>>> GetRecommendedUsersAsync(RecommendationTypes recommendationType, FiltersDto filters)
        {
            return await _recommendationStrategies[recommendationType].GetRecommendedUsersAsync(filters);
        }
    }
}
