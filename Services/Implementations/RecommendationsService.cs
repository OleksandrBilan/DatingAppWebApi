using DatingApp.DB;
using DatingApp.DB.Models.Chats;
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
                             .ExcludeExistingChatsUsers(filters, _dbContext)
                             .CalculateUsersSimilarityScore(filters, _dbContext)
                             .CalculateUsersSimilarityByQuestionnaireScore(filters, _dbContext)
                             .OrderBySimilarityDescending();
        }

        #region Likes

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

        private IEnumerable<RecommendedUser> CalculateSimilarityScore(IEnumerable<RecommendedUser> recommendedUsers, string currentUserId)
        {
            if (recommendedUsers is null || !recommendedUsers.Any())
                return recommendedUsers;

            var filters = new FiltersDto { UserId = currentUserId, UseQuestionnaire = true };
            return recommendedUsers.CalculateUsersSimilarityScore(filters, _dbContext)
                                   .CalculateUsersSimilarityByQuestionnaireScore(filters, _dbContext);
        }

        public async Task<IEnumerable<RecommendedUser>> GetUserLikesAsync(string userId)
        {
            var recommendedUsers = await _dbContext.UsersLikes.Where(x => x.LikedUserId == userId)
                                                              .Include(x => x.LikingUser)
                                                              .ThenInclude(u => u.Country)
                                                              .Include(x => x.LikingUser)
                                                              .ThenInclude(u => u.City)
                                                              .Select(x => new RecommendedUser { User = x.LikingUser, SimilarityScore = 0d, LikeId = x.Id })
                                                              .ToListAsync();
            return CalculateSimilarityScore(recommendedUsers, userId);
        }

        public async Task<IEnumerable<RecommendedUser>> GetUserMutualLikesAsync(string userId)
        {
            var recommendedUsers1 = await _dbContext.MutualLikes.Where(x => x.User2Id == userId)
                                                                .Include(x => x.User1)
                                                                .ThenInclude(u => u.Country)
                                                                .Include(x => x.User1)
                                                                .ThenInclude(u => u.City)
                                                                .Select(x => new RecommendedUser { User = x.User1, SimilarityScore = 0d, LikeId = x.Id })
                                                                .ToListAsync();

            var recommendedUsers2 = await _dbContext.MutualLikes.Where(x => x.User1Id == userId)
                                                                .Include(x => x.User2)
                                                                .ThenInclude(u => u.Country)
                                                                .Include(x => x.User2)
                                                                .ThenInclude(u => u.City)
                                                                .Select(x => new RecommendedUser { User = x.User2, SimilarityScore = 0d, LikeId = x.Id })
                                                                .ToListAsync();

            var recommendedUsers = recommendedUsers1.Concat(recommendedUsers2);
            return CalculateSimilarityScore(recommendedUsers, userId);
        }

        public async Task DeleteLikeAsync(int likeId)
        {
            var like = await _dbContext.UsersLikes.FirstOrDefaultAsync(x => x.Id == likeId);
            if (like is not null)
            {
                _dbContext.UsersLikes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMutualLikeAsync(int mutualLikeId)
        {
            var like = await _dbContext.MutualLikes.FirstOrDefaultAsync(x => x.Id == mutualLikeId);
            if (like is not null)
            {
                _dbContext.MutualLikes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion

        #region Chats

        public async Task<int> CreateChatAsync(int mutualLikeId)
        {
            var mutualLike = await _dbContext.MutualLikes.FirstOrDefaultAsync(x => x.Id == mutualLikeId);
            if (mutualLike is null)
                throw new ArgumentException("No mutual like with such id", nameof(mutualLikeId));

            var chat = new UsersChat
            {
                User1Id = mutualLike.User1Id,
                User2Id = mutualLike.User2Id,
                CreatedDateTime = DateTime.Now,
            };
            await _dbContext.Chats.AddAsync(chat);
            _dbContext.MutualLikes.Remove(mutualLike);
            await _dbContext.SaveChangesAsync();
            return chat.Id;
        }

        public async Task DeleteChatAsync(int chatId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
            if (chat is not null)
            {
                var chatMessages = await _dbContext.Messages.Where(m => m.ChatId == chatId).ToListAsync();
                _dbContext.Messages.RemoveRange(chatMessages);

                _dbContext.Chats.Remove(chat);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tuple<UsersChat, int>>> GetUserChatsAsync(string userId)
        {
            var chats = await _dbContext.Chats.Where(c => c.User1Id == userId || c.User2Id == userId)
                                              .Include(c => c.User1)
                                              .Include(c => c.User2)
                                              .Include(c => c.Messages)
                                              .OrderByDescending(c => c.CreatedDateTime)
                                              .ToListAsync();

            var chatsWithUnreadMessagesCount = chats.Select(c => new Tuple<UsersChat, int>(c, c.Messages is null ? 0 : c.Messages.Count(m => m.SenderId != userId && m.StatusId == 1)));
            return chatsWithUnreadMessagesCount;
        }

        public async Task<UsersChat> GetChatAsync(int chatId)
        {
            var chat = await _dbContext.Chats.Where(c => c.Id == chatId)
                                             .Include(c => c.User1)
                                             .Include(c => c.User2)
                                             .Include(c => c.Messages)
                                             .ThenInclude(m => m.Status)
                                             .FirstOrDefaultAsync();
            if (chat is not null)
                chat.Messages = chat.Messages?.OrderBy(m => m.DateTime)?.ToList();
            return chat;
        }

        public async Task<bool> SetChatMessagesReadAsync(int chatId, string userId)
        {
            var chat = await _dbContext.Chats.Where(c => c.Id == chatId)
                                             .Include(c => c.User1)
                                             .Include(c => c.User2)
                                             .Include(c => c.Messages)
                                             .ThenInclude(m => m.Status)
                                             .FirstOrDefaultAsync();

            if (chat is not null && chat.Messages is not null && chat.Messages.Count > 0)
            {
                var unreadMessages = chat.Messages.Where(m => m.SenderId != userId && m.StatusId == 1);
                if (unreadMessages.Any())
                {
                    foreach (var message in unreadMessages)
                    {
                        message.StatusId = 2;
                    }
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
