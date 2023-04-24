using DatingApp.DTOs.User;

namespace DatingApp.DTOs.Recommendations
{
    public class RecommendedUserDto
    {
        public UserDto User { get; set; }

        public double SimilarityScore { get; set; }

        public int? LikeId { get; set; }
    }
}
