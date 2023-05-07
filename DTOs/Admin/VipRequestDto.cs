using DatingApp.DTOs.Lookup;
using DatingApp.DTOs.User;

namespace DatingApp.DTOs.Admin
{
    public class VipRequestDto
    {
        public int Id { get; set; }

        public UserDto User { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTime CreateDateTime { get; set; }

        public SubscriptionTypeDto SubscriptionType { get; set; }
    }
}
