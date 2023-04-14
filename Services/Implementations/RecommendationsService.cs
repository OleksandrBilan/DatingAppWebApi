using DatingApp.DB;
using DatingApp.DB.Models.Recommendations;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Helpers;
using DatingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddUserLikeAsync(string likingUserId, string likedUserId)
        {
            bool likingUserExists = await _dbContext.Users.AnyAsync(u => u.Id == likingUserId);
            if (!likingUserExists)
                throw new ArgumentException("No user with such id", nameof(likingUserId));

            bool likedUserExists = await _dbContext.Users.AnyAsync(u => u.Id == likedUserId);
            if (!likedUserExists)
                throw new ArgumentException("No user with such id", nameof(likedUserId));

            bool recordExists = await _dbContext.UsersLikes.AnyAsync(x => x.LikingUserId == likingUserId && x.LikedUserId == likedUserId);
            if (!recordExists)
            {
                await _dbContext.UsersLikes.AddAsync(new UserLike
                {
                    LikingUserId = likingUserId,
                    LikedUserId = likedUserId,
                });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
