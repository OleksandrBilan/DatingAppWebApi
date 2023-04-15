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
                             .GetRecommendedUsersByFilters(filters)
                             .ExcludeLikedUsers(filters, _dbContext)
                             .CalculateUsersSimilarityScore(filters, _dbContext)
                             .CalculateUsersSimilarityByQuestionnaireScore(filters, _dbContext)
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

            bool sameRecordExists = await _dbContext.UsersLikes.AnyAsync(x => x.LikingUserId == likingUserId && x.LikedUserId == likedUserId);
            if (!sameRecordExists)
            {
                var mutualLike = await _dbContext.UsersLikes.FirstOrDefaultAsync(x => x.LikingUserId == likedUserId && x.LikedUserId == likingUserId);
                if (mutualLike is not null)
                {
                    await _dbContext.MutualLikes.AddAsync(new MutualLike { User1Id = likingUserId, User2Id = likedUserId });
                    _dbContext.UsersLikes.Remove(mutualLike);
                }
                else
                {
                    await _dbContext.UsersLikes.AddAsync(new UserLike
                    {
                        LikingUserId = likingUserId,
                        LikedUserId = likedUserId,
                        DateTime = DateTime.Now,
                    });
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task<IEnumerable<RecommendedUser>> MapToRecommendedUsers(IEnumerable<string> userIds, string currentUserId)
        {
            var recommendedUsers = await _dbContext.Users.Where(u => userIds.Any(id => id == u.Id))
                                                         .Include(u => u.Country)
                                                         .Include(u => u.City)
                                                         .Select(u => new RecommendedUser { User = u, SimilarityScore = 0d })
                                                         .ToListAsync();

            var filters = new FiltersDto { UserId = currentUserId, UseQuestionnaire = true };
            return recommendedUsers.CalculateUsersSimilarityScore(filters, _dbContext)
                                   .CalculateUsersSimilarityByQuestionnaireScore(filters, _dbContext);
        }

        public async Task<IEnumerable<RecommendedUser>> GetUserLikesAsync(string userId)
        {
            var userIds = await _dbContext.UsersLikes.Where(x => x.LikedUserId == userId)
                                                     .Select(x => x.LikingUserId)
                                                     .ToListAsync();
            return await MapToRecommendedUsers(userIds, userId);
        }

        public async Task<IEnumerable<RecommendedUser>> GetUserMutualLikesAsync(string userId)
        {
            var userIds1 = await _dbContext.MutualLikes.Where(x => x.User2Id == userId)
                                                       .Select(x => x.User1Id)
                                                       .ToListAsync();

            var userIds2 = await _dbContext.MutualLikes.Where(x => x.User1Id == userId)
                                                       .Select(x => x.User2Id)
                                                       .ToListAsync();

            var userIds = userIds1.Concat(userIds2);
            return await MapToRecommendedUsers(userIds, userId);
        }
    }
}
