using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Recommendations
{
    public class MutualLike
    {
        public string User1Id { get; set; }
        public User User1 { get; set; }

        public string User2Id { get; set; }
        public User User2 { get; set; }
    }
}
