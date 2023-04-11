namespace DatingApp.DB.Models.UserRelated
{
    public class RecommendedUser
    {
        public User User { get; set; }

        public double SimilarityScore { get; set; }
    }
}
