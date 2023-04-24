using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Recommendations
{
    public class UserLike
    {
        public int Id { get; set; }

        public string LikingUserId { get; set; }
        public User LikingUser { get; set; }

        public string LikedUserId { get; set; }
        public User LikedUser { get; set; }

        public DateTime DateTime { get; set; }
    }
}
