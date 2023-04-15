using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Recommendations
{
    public class UserLikeDto
    {
        [Required]
        public string LikingUserId { get; set; }

        [Required]
        public string LikedUserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
