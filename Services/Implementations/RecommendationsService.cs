using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Helpers;
using DatingApp.Services.Interfaces;

namespace DatingApp.Services.Implementations
{
    public class RecommendationsService : IRecommendationsService
    {
        private readonly AppDbContext _dbContext;

        public RecommendationsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<RecommendedUser> GetRecommendedUsers(FiltersDto filters)
        {
            return _dbContext.FormBaseQuery(filters)
                             .GetRecommendedUsersByFiltersAsync(filters)
                             .CalculateUsersSimilarityScoreAsync(filters, _dbContext)
                             .CalculateUsersSimilarityByQuestionnaireScoreAsync(filters, _dbContext)
                             .OrderBySimilarityDescending();
        }
    }
}
