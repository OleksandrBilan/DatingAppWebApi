namespace DatingApp.DB.Models.UserRelated
{
    public class VipRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
