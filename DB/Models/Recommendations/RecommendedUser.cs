using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Recommendations
{
    public class RecommendedUser
    {
        public User User { get; set; }

        public double SimilarityScore { get; set; }

        public int? LikeId { get; set; }
    }
}
