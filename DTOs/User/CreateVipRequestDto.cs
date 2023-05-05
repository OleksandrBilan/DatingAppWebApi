using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.User
{
    public class CreateVipRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int SubscriptionTypeId { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
