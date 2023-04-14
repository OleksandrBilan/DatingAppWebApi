﻿using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Recommendations
{
    public class UserLike
    {
        public string LikingUserId { get; set; }
        public User LikingUser { get; set; }

        public string LikedUserId { get; set; }
        public User LikedUser { get; set; }
    }
}
