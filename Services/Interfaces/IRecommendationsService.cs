using DatingApp.DB.Models.Chats;
using DatingApp.DB.Models.Recommendations;
using DatingApp.DTOs.Recommendations;

namespace DatingApp.Services.Interfaces
{
    public interface IRecommendationsService
    {
        IEnumerable<RecommendedUser> GetRecommendedUsers(FiltersDto filters);

        Task AddUserLikeAsync(string likingUserId, string likedUserId);

        Task<IEnumerable<RecommendedUser>> GetUserLikesAsync(string userId);

        Task<IEnumerable<RecommendedUser>> GetUserMutualLikesAsync(string userId);

        Task DeleteLikeAsync(int likeId);

        Task DeleteMutualLikeAsync(int mutualLikeId);

        Task<int> CreateChatAsync(int mutualLikeId);

        Task DeleteChatAsync(int chatId);

        Task<IEnumerable<Tuple<UsersChat, int>>> GetUserChatsAsync(string userId);

        Task<UsersChat> GetChatAsync(int chatId);
    }
}
